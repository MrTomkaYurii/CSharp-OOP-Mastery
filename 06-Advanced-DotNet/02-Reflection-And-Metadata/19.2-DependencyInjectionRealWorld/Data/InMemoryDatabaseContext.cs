using DependencyInjection.Models;
using DependencyInjection.Services.Common;

namespace DependencyInjection.Data;

/// <summary>
/// Емуляція реляційної бази даних у пам'яті (In-Memory Database Context).
/// Інкапсулює типізовані колекції об'єктів та зберігає їх стан між транзакціями.
/// Реєструється в IoC-контейнері як Singleton для спільного доступу різних репозиторіїв.
/// </summary>
public class InMemoryDatabaseContext : IDatabaseContext
{
    private readonly ILogger _logger;
    private readonly int _contextId = Random.Shared.Next(100, 999);
    private readonly List<Product> _products = new();
    private readonly List<Customer> _customers = new();
    private readonly List<Order> _orders = new();

    public ICollection<Product> Products => _products;
    public ICollection<Customer> Customers => _customers;
    public ICollection<Order> Orders => _orders;

    public InMemoryDatabaseContext(ILogger logger)
    {
        _logger = logger;
        SeedInitialData();
        _logger.Log($"Ініціалізовано контекст InMemoryDatabaseContext #{_contextId} ({_products.Count} товарів, {_customers.Count} покупців).");
    }

    public void SaveChanges()
    {
        _logger.Log($"[DbContext #{_contextId}] Зміни успішно збережено в In-Memory сховищі.");
    }

    private void SeedInitialData()
    {
        _products.Add(new Product(1, "Ноутбук Dell XPS 15", 48000.00m, 5));
        _products.Add(new Product(2, "Смартфон iPhone 15 Pro", 42000.00m, 8));
        _products.Add(new Product(3, "Навушники Sony WH-1000XM5", 12500.00m, 15));
        _products.Add(new Product(4, "Монітор Dell UltraSharp 27", 19000.00m, 4));

        _customers.Add(new Customer(1, "Олександр Коваленко", "o.kovalenko@example.com", 65000.00m));
        _customers.Add(new Customer(2, "Марія Шевченко", "m.shevchenko@example.com", 30000.00m));
        _customers.Add(new Customer(3, "Іван Бондаренко", "i.bondarenko@example.com", 8000.00m));
    }
}
