using DI.GenericHost.Worker.Configuration;
using DI.GenericHost.Worker.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DI.GenericHost.Worker.Workers;

/// <summary>
/// Фонова служба (BackgroundService), що виконує періодичну обробку замовлень
/// у власному ізольованому IServiceScope для запобігання витоку пам'яті.
/// </summary>
public class OrderProcessingWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptions<WorkerOptions> _options;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<OrderProcessingWorker> _logger;

    public OrderProcessingWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<WorkerOptions> options,
        IHostApplicationLifetime lifetime,
        ILogger<OrderProcessingWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options;
        _lifetime = lifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = _options.Value;
        _logger.LogInformation("Воркер '{WorkerName}' розпочав роботу. Інтервал: {Interval}с, розмір пачки: {BatchSize}.",
            settings.WorkerName, settings.IntervalSeconds, settings.BatchSize);

        int currentIteration = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            currentIteration++;
            _logger.LogInformation("--- Ітерація #{Iteration} з {MaxIterations} ---", currentIteration, settings.MaxIterations);

            try
            {
                // Створюємо новий Scoped контейнер для поточної ітерації
                using (var scope = _scopeFactory.CreateScope())
                {
                    var processor = scope.ServiceProvider.GetRequiredService<IOrderProcessingService>();
                    int processed = await processor.ProcessBatchAsync(settings.BatchSize, stoppingToken);
                    _logger.LogInformation("Успішно оброблено {Count} замовлень в ітерації #{Iteration}.", processed, currentIteration);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Роботу воркера було скасовано за запитом зупинки хоста.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка під час обробки пачки замовлень.");
            }

            if (currentIteration >= settings.MaxIterations)
            {
                _logger.LogInformation("Досягнуто ліміт демонстраційних ітерацій ({Max}). Запит на штатна зупинка додатку...", settings.MaxIterations);
                _lifetime.StopApplication();
                break;
            }

            _logger.LogInformation("Очікування {Seconds} секунд до наступної ітерації...", settings.IntervalSeconds);
            await Task.Delay(TimeSpan.FromSeconds(settings.IntervalSeconds), stoppingToken);
        }

        _logger.LogInformation("Воркер '{WorkerName}' завершив цикл виконання ExecuteAsync.", settings.WorkerName);
    }
}
