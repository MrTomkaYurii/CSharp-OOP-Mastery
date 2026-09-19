namespace DI.GenericHost.Worker.Services;

public record OrderItem(Guid OrderId, string Customer, decimal Amount, DateTime CreatedAt);

public interface IOrderProcessingService
{
    Task<int> ProcessBatchAsync(int batchSize, CancellationToken ct);
}

public class OrderProcessingService : IOrderProcessingService
{
    private readonly ILogger<OrderProcessingService> _logger;

    public OrderProcessingService(ILogger<OrderProcessingService> logger)
    {
        _logger = logger;
    }

    public async Task<int> ProcessBatchAsync(int batchSize, CancellationToken ct)
    {
        _logger.LogInformation("Обробка пачки з {Count} замовлень у виділеному Scoped-контексті...", batchSize);

        // Імітуємо асинхронну обробку бізнес-логіки
        await Task.Delay(300, ct);

        for (int i = 1; i <= batchSize; i++)
        {
            var order = new OrderItem(Guid.NewGuid(), $"Клієнт #{i}", 150m * i, DateTime.UtcNow);
            _logger.LogDebug("  -> Замовлення {OrderId} для {Customer} на суму {Amount} грн успішно оброблено.",
                order.OrderId, order.Customer, order.Amount);
        }

        _logger.LogInformation("Пачка успішно оброблена.");
        return batchSize;
    }
}
