using Entertainment.Configuration;
using Entertainment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.HostedServices;

/// <summary>
/// Hosted Service для ініціалізації бази даних при старті додатку
/// та видалення при зупинці
/// </summary>
public class DatabaseInitializationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializationService> _logger;
    private readonly IOptions<DatabaseInitializationOptions> _options;
    private readonly bool _deleteOnShutdown;

    public DatabaseInitializationService(
        IServiceProvider serviceProvider,
        ILogger<DatabaseInitializationService> logger,
        IOptions<DatabaseInitializationOptions> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options;
        _deleteOnShutdown = options.Value.DeleteOnShutdown;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Початок ініціалізації бази даних...");

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EntertainmentDbContext>();

        try
        {
            // Видаляємо стару базу даних якщо потрібно
            var options = _options.Value;

            if (options.AutoMigrate)
            {
                _logger.LogInformation("Видалення існуючої бази даних...");
                await context.Database.EnsureDeletedAsync(cancellationToken);

                _logger.LogInformation("Застосування міграцій...");
                await context.Database.MigrateAsync(cancellationToken);
            }

            _logger.LogInformation("База даних успішно ініціалізована");
            _logger.LogInformation("Seed дані будуть додані автоматично через OnModelCreating");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при ініціалізації бази даних");
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!_deleteOnShutdown)
        {
            _logger.LogInformation("Збереження бази даних при зупинці");
            return;
        }

        _logger.LogInformation("Початок видалення бази даних...");

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EntertainmentDbContext>();

        try
        {
            await context.Database.EnsureDeletedAsync(cancellationToken);
            _logger.LogInformation("База даних успішно видалена");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при видаленні бази даних");
        }
    }
}