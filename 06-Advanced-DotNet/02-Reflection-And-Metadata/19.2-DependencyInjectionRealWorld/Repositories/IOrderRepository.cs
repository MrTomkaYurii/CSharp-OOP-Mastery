using DependencyInjection.Models;

namespace DependencyInjection.Repositories;

/// <summary>
/// Репозиторій для збереження та отримання замовлень.
/// </summary>
public interface IOrderRepository
{
    void Add(Order order);
    Order? GetByNumber(string orderNumber);
    IReadOnlyList<Order> GetAll();
}
