using DependencyInjection.Data;
using DependencyInjection.Models;
using DependencyInjection.Services.Common;

namespace DependencyInjection.Repositories;

/// <summary>
/// Реалізація репозиторію товарів поверх контексту бази даних IDatabaseContext.
/// Дотримується Single Responsibility Principle (SRP): відповідає лише за запити та операції над товарами.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly IDatabaseContext _dbContext;
    private readonly ILogger _logger;

    public ProductRepository(IDatabaseContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IReadOnlyList<Product> GetAll()
    {
        return _dbContext.Products.ToList();
    }

    public Product? GetById(int id)
    {
        return _dbContext.Products.FirstOrDefault(p => p.Id == id);
    }

    public void UpdateStock(int productId, int delta)
    {
        var product = GetById(productId);
        if (product == null)
        {
            throw new KeyNotFoundException($"Товар з ідентифікатором {productId} не знайдено.");
        }

        product.StockQuantity += delta;
        _logger.Log($"Оновлено складський залишок '{product.Name}': тепер {product.StockQuantity} шт.");
        _dbContext.SaveChanges();
    }
}
