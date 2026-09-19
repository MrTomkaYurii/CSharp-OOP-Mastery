using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DI.GenericHost.Worker.Workers;

/// <summary>
/// Демонстрація реалізації IHostedService напряму та підписки на події життєвого циклу хоста.
/// </summary>
public class LifecycleMonitorService : IHostedService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<LifecycleMonitorService> _logger;

    public LifecycleMonitorService(
        IHostApplicationLifetime lifetime,
        ILogger<LifecycleMonitorService> logger)
    {
        _lifetime = lifetime;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[IHostedService] LifecycleMonitorService.StartAsync викликано.");

        _lifetime.ApplicationStarted.Register(() =>
            _logger.LogInformation("[ПОДІЯ ХОСТА] >>> ApplicationStarted: Всі сервіси ініціалізовано, хост готовий приймати навантаження."));

        _lifetime.ApplicationStopping.Register(() =>
            _logger.LogInformation("[ПОДІЯ ХОСТА] >>> ApplicationStopping: Отримано сигнал зупинки. Починається Graceful Shutdown."));

        _lifetime.ApplicationStopped.Register(() =>
            _logger.LogInformation("[ПОДІЯ ХОСТА] >>> ApplicationStopped: Всі фонові задачі зупинено, хост повністю вимкнено."));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[IHostedService] LifecycleMonitorService.StopAsync викликано. Звільнення ресурсів монітора.");
        return Task.CompletedTask;
    }
}
