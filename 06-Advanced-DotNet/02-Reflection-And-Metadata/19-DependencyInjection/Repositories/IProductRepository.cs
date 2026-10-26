using DependencyInjection.Models;

namespace DependencyInjection.Repositories;

/// <summary>
/// Репозиторій для роботи з колекцією товарів.
/// </summary>
public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(int id);
    void UpdateStock(int productId, int delta);
}
