namespace Testing.Core.Domain.Services;

public record OrderItemDto(string Sku, int Quantity, decimal UnitPrice);
public record OrderCheckoutRequest(Guid OrderId, string CustomerEmail, List<OrderItemDto> Items);
public record OrderCheckoutResponse(bool Success, string OrderId, string TransactionId, decimal TotalAmount);

public class PaymentFailedException : Exception
{
    public PaymentFailedException(string message) : base(message) { }
}

public class OutOfStockException : Exception
{
    public OutOfStockException(string message) : base(message) { }
}

/// <summary>
/// Контракт зовнішнього платіжного шлюзу (Stripe / LiqPay тощо).
/// </summary>
public interface IPaymentGateway
{
    Task<string> ChargeAsync(string customerEmail, decimal amount, CancellationToken ct);
}

/// <summary>
/// Контракт складського сервісу резервування товарів.
/// </summary>
public interface IInventoryService
{
    Task<bool> ReserveStockAsync(IEnumerable<OrderItemDto> items, CancellationToken ct);
    Task ReleaseStockAsync(IEnumerable<OrderItemDto> items, CancellationToken ct);
}

/// <summary>
/// Контракт сервісу відправки Email/SMS сповіщень клієнту.
/// </summary>
public interface INotificationService
{
    Task SendOrderConfirmationAsync(string customerEmail, string orderId, decimal totalAmount, CancellationToken ct);
}

/// <summary>
/// Контракт журналу аудиту бізнес-подій.
/// </summary>
public interface IAuditLogger
{
    void LogAudit(string eventName, string details);
}

/// <summary>
/// Сервіс оформлення замовлення (System Under Test для демонстрації Moq та NSubstitute),
/// що координує склад, платіжний шлюз, сповіщення та журнал аудиту.
/// </summary>
public class OrderCheckoutService
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IInventoryService _inventoryService;
    private readonly INotificationService _notificationService;
    private readonly IAuditLogger? _auditLogger;

    public OrderCheckoutService(
        IPaymentGateway paymentGateway,
        IInventoryService inventoryService,
        INotificationService notificationService,
        IAuditLogger? auditLogger = null)
    {
        _paymentGateway = paymentGateway;
        _inventoryService = inventoryService;
        _notificationService = notificationService;
        _auditLogger = auditLogger;
    }

    public async Task<OrderCheckoutResponse> ProcessOrderAsync(OrderCheckoutRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Items is null || request.Items.Count == 0)
            throw new ArgumentException("Замовлення повинно містити щонайменше одну позицію.", nameof(request));

        decimal totalAmount = request.Items.Sum(i => i.Quantity * i.UnitPrice);

        // 1. Резервування залишків на складі
        bool stockReserved = await _inventoryService.ReserveStockAsync(request.Items, ct);
        if (!stockReserved)
        {
            _auditLogger?.LogAudit("OrderFailed_OutOfStock", $"Order {request.OrderId}: недостатньо товару на складі.");
            throw new OutOfStockException("Недостатньо залишків товарів на складі для завершення замовлення.");
        }

        string transactionId;
        try
        {
            // 2. Списання коштів через зовнішній платіжний шлюз
            transactionId = await _paymentGateway.ChargeAsync(request.CustomerEmail, totalAmount, ct);
        }
        catch (Exception ex)
        {
            // Компенсуюча транзакція: повернення зарезервованого товару на склад
            await _inventoryService.ReleaseStockAsync(request.Items, ct);
            _auditLogger?.LogAudit("OrderFailed_PaymentError", $"Order {request.OrderId}: помилка оплати ({ex.Message}).");
            throw new PaymentFailedException($"Помилка проведення оплати: {ex.Message}");
        }

        // 3. Відправка листа-підтвердження та запис в аудит
        await _notificationService.SendOrderConfirmationAsync(request.CustomerEmail, request.OrderId.ToString(), totalAmount, ct);
        _auditLogger?.LogAudit("OrderCompleted", $"Order {request.OrderId}: успішно оплачено {totalAmount} грн (Tx: {transactionId}).");

        return new OrderCheckoutResponse(true, request.OrderId.ToString(), transactionId, totalAmount);
    }

    /// <summary>
    /// Оформлення замовлення з повторною спробою оплати (Retry) при тимчасовому збої шлюзу.
    /// Демонструє роботу SetupSequence(...) та Verify(..., Times.Exactly(2)) у бібліотеці Moq.
    /// </summary>
    public async Task<OrderCheckoutResponse> ProcessOrderWithRetryAsync(
        OrderCheckoutRequest request,
        int maxPaymentAttempts = 2,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        decimal totalAmount = request.Items.Sum(i => i.Quantity * i.UnitPrice);

        bool stockReserved = await _inventoryService.ReserveStockAsync(request.Items, ct);
        if (!stockReserved)
            throw new OutOfStockException("Недостатньо залишків товарів на складі.");

        Exception? lastException = null;
        for (int attempt = 1; attempt <= maxPaymentAttempts; attempt++)
        {
            try
            {
                string txId = await _paymentGateway.ChargeAsync(request.CustomerEmail, totalAmount, ct);
                await _notificationService.SendOrderConfirmationAsync(request.CustomerEmail, request.OrderId.ToString(), totalAmount, ct);
                _auditLogger?.LogAudit("OrderCompletedWithRetry", $"Order {request.OrderId} оплачено з {attempt}-ї спроби (Tx: {txId}).");
                return new OrderCheckoutResponse(true, request.OrderId.ToString(), txId, totalAmount);
            }
            catch (Exception ex)
            {
                lastException = ex;
                _auditLogger?.LogAudit("PaymentAttemptFailed", $"Спроба #{attempt} невдала: {ex.Message}");
            }
        }

        await _inventoryService.ReleaseStockAsync(request.Items, ct);
        throw new PaymentFailedException($"Помилка проведення оплати після {maxPaymentAttempts} спроб: {lastException?.Message}");
    }
}
