using DependencyInjection.Services.Common;

namespace DependencyInjection.Services.Payments;

/// <summary>
/// Реалізація обробки банківських карток через платіжний шлюз.
/// </summary>
public class CreditCardPaymentService : IPaymentService
{
    private readonly ILogger _logger;

    public CreditCardPaymentService(ILogger logger)
    {
        _logger = logger;
    }

    public PaymentResult ProcessPayment(string customerName, decimal amount)
    {
        _logger.Log($"Платіжний шлюз: авторизація транзакції для клієнта '{customerName}' на суму {amount:C}...");
        string txId = $"TX-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
        _logger.Log($"Платіж успішно авторизовано. Номер транзакції: {txId}.");
        return new PaymentResult(true, txId, "Оплата пройшла успішно");
    }
}
