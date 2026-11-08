namespace DependencyInjection.Services.Payments;

/// <summary>
/// Інтерфейс платіжного шлюзу (DIP: високорівневі сервіси залежать від абстракції оплати).
/// </summary>
public interface IPaymentService
{
    PaymentResult ProcessPayment(string customerName, decimal amount);
}
