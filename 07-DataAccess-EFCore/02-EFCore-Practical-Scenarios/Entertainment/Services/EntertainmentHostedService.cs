using Entertainment.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.HostedServices;

/// <summary>
/// Головний Hosted Service додатку, який запускає меню та обробляє запити
/// </summary>
public class EntertainmentHostedService : IHostedService
{
    private readonly IMenuService _menuService;
    private readonly ILogger<EntertainmentHostedService> _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private Task? _executingTask;

    public EntertainmentHostedService(
        IMenuService menuService,
        ILogger<EntertainmentHostedService> logger,
        IHostApplicationLifetime appLifetime)
    {
        _menuService = menuService;
        _logger = logger;
        _appLifetime = appLifetime;
    }

    /// <summary>
    /// START
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Запуск EntertainmentHostedService");

        // Запускаємо виконання в окремій задачі
        _executingTask = ExecuteAsync(cancellationToken);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Execute
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Даємо час іншим сервісам завершити ініціалізацію
            await Task.Delay(500, cancellationToken);

            _logger.LogInformation("Запуск інтерактивного меню");

            // Запускаємо меню
            await _menuService.RunAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Виконання EntertainmentHostedService скасовано");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при виконанні EntertainmentHostedService");
        }
        finally
        {
            // Зупиняємо додаток після завершення меню
            _appLifetime.StopApplication();
        }
    }

    /// <summary>
    /// STOP
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Зупинка EntertainmentHostedService");

        if (_executingTask != null)
        {
            // Чекаємо завершення виконання
            await _executingTask;
        }
    }
}