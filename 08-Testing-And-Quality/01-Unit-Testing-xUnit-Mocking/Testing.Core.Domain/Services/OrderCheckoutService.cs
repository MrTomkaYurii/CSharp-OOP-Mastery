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

public interface IPaymentGateway
{
    Task<string> ChargeAsync(string customerEmail, decimal amount, CancellationToken ct);
}

public interface IInventoryService
{
    Task<bool> ReserveStockAsync(IEnumerable<OrderItemDto> items, CancellationToken ct);
    Task ReleaseStockAsync(IEnumerable<OrderItemDto> items, CancellationToken ct);
}

public interface INotificationService
{
    Task SendOrderConfirmationAsync(string customerEmail, string orderId, decimal totalAmount, CancellationToken ct);
}

/// <summary>
/// Сервіс оформлення замовлення, що координує платіж, склад та сповіщення.
/// </summary>
public class OrderCheckoutService
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IInventoryService _inventoryService;
    private readonly INotificationService _notificationService;

    public OrderCheckoutService(
        IPaymentGateway paymentGateway,
        IInventoryService inventoryService,
        INotificationService notificationService)
    {
        _paymentGateway = paymentGateway;
        _inventoryService = inventoryService;
        _notificationService = notificationService;
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
            throw new OutOfStockException("Недостатньо залишків товарів на складі для завершення замовлення.");
        }

        string transactionId;
        try
        {
            // 2. Списання коштів
            transactionId = await _paymentGateway.ChargeAsync(request.CustomerEmail, totalAmount, ct);
        }
        catch (Exception ex)
        {
            // Компенсуюча транзакція: повернення зарезервованого товару на склад
            await _inventoryService.ReleaseStockAsync(request.Items, ct);
            throw new PaymentFailedException($"Помилка проведення оплати: {ex.Message}");
        }

        // 3. Відправка листа-підтвердження
        await _notificationService.SendOrderConfirmationAsync(request.CustomerEmail, request.OrderId.ToString(), totalAmount, ct);

        return new OrderCheckoutResponse(true, request.OrderId.ToString(), transactionId, totalAmount);
    }
}
