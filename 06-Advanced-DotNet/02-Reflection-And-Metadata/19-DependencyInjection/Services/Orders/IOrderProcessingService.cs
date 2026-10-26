using DependencyInjection.Models;

namespace DependencyInjection.Services.Orders;

/// <summary>
/// Інтерфейс головного бізнес-сервісу обробки замовлень.
/// </summary>
public interface IOrderProcessingService
{
    /// <summary>
    /// Оформлює замовлення для клієнта на обраний товар:
    /// перевіряє клієнта, товар та залишки, списує оплату, зменшує залишок,
    /// зберігає замовлення в базі даних та надсилає квитанцію.
    /// </summary>
    Order? PlaceOrder(int customerId, int productId, int quantity);

    /// <summary>
    /// Виводить у консоль поточний перелік замовлень із бази даних.
    /// </summary>
    void PrintOrderHistory();
}
