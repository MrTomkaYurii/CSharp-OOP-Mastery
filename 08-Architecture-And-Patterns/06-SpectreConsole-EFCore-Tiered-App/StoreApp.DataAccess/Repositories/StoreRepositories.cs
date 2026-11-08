using Microsoft.EntityFrameworkCore;
using StoreApp.DataAccess.Context;
using StoreApp.DataAccess.Entities;

namespace StoreApp.DataAccess.Repositories;

/// <summary>
/// Контракт репозиторію товарів (Data Access Layer).
/// </summary>
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllWithCategoryReadOnlyAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetLowStockProductsReadOnlyAsync(CancellationToken ct = default);
    Task<Product?> GetBySkuTrackedAsync(string sku, CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetBySkusTrackedAsync(IEnumerable<string> skus, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

/// <summary>
/// Контракт репозиторію замовлень із підтримкою явних транзакцій бази даних.
/// </summary>
public interface IOrderRepository
{
    Task<CustomerOrder> CreateOrderInTransactionAsync(
        CustomerOrder order,
        IReadOnlyDictionary<int, int> stockDeductions,
        CancellationToken ct = default);

    Task<IReadOnlyList<CustomerOrder>> GetAllOrdersWithDetailsReadOnlyAsync(CancellationToken ct = default);
}

/// <summary>
/// Реалізація репозиторію товарів на базі Entity Framework Core (SQLite).
/// Використовує <c>AsNoTracking()</c> для швидких операцій читання та <c>Include()</c> для жадібного завантаження категорій.
/// </summary>
public sealed class ProductRepository : IProductRepository
{
    private readonly StoreDbContext _context;

    public ProductRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> GetAllWithCategoryReadOnlyAsync(CancellationToken ct = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Category.Name)
            .ThenBy(p => p.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Product>> GetLowStockProductsReadOnlyAsync(CancellationToken ct = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.StockQuantity <= p.ReorderLevel)
            .OrderBy(p => p.StockQuantity)
            .ToListAsync(ct);
    }

    public async Task<Product?> GetBySkuTrackedAsync(string sku, CancellationToken ct = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Sku == sku, ct);
    }

    public async Task<IReadOnlyList<Product>> GetBySkusTrackedAsync(IEnumerable<string> skus, CancellationToken ct = default)
    {
        string[] skuArray = skus.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => skuArray.Contains(p.Sku))
            .ToListAsync(ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}

/// <summary>
/// Реалізація репозиторію замовлень із гарантуванням атомарності списання залишків та збереження замовлення.
/// </summary>
public sealed class OrderRepository : IOrderRepository
{
    private readonly StoreDbContext _context;

    public OrderRepository(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerOrder> CreateOrderInTransactionAsync(
        CustomerOrder order,
        IReadOnlyDictionary<int, int> stockDeductions,
        CancellationToken ct = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(ct);

        try
        {
            foreach ((int productId, int quantityToDeduct) in stockDeductions)
            {
                Product product = await _context.Products.FirstAsync(p => p.Id == productId, ct);
                product.StockQuantity -= quantityToDeduct;
            }

            _context.CustomerOrders.Add(order);
            await _context.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);
            return order;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<IReadOnlyList<CustomerOrder>> GetAllOrdersWithDetailsReadOnlyAsync(CancellationToken ct = default)
    {
        return await _context.CustomerOrders
            .AsNoTracking()
            .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                    .ThenInclude(p => p.Category)
            .OrderByDescending(o => o.CreatedAtUtc)
            .ToListAsync(ct);
    }
}
