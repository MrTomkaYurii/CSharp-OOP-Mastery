using DependencyInjection.Data;
using DependencyInjection.Models;
using DependencyInjection.Services.Common;

namespace DependencyInjection.Repositories;

/// <summary>
/// Реалізація репозиторію замовлень.
/// Взаємодіє зі структурованою колекцією Orders у контексті бази даних IDatabaseContext.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly IDatabaseContext _dbContext;
    private readonly ILogger _logger;

    public OrderRepository(IDatabaseContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public void Add(Order order)
    {
        _dbContext.Orders.Add(order);
        _logger.Log($"Замовлення {order.OrderNumber} збережено в колекцію бази даних.");
        _dbContext.SaveChanges();
    }

    public Order? GetByNumber(string orderNumber)
    {
        return _dbContext.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
    }

    public IReadOnlyList<Order> GetAll()
    {
        return _dbContext.Orders.ToList();
    }
}
