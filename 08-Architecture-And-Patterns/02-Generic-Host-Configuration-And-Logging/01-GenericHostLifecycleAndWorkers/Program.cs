using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHostLifecycleAndWorkers;

// ============================================================================
// ПРОЄКТ 01: Архітектура Generic Host та Фонові служби (IHost, BackgroundService)
// ============================================================================
// Демонструє:
// 1. Побудову консольного застосунку на основі Host.CreateApplicationBuilder.
// 2. Відстеження життєвого циклу через IHostApplicationLifetime:
//    - ApplicationStarted (хост повністю запущено)
//    - ApplicationStopping (ініційовано граційне завершення роботи)
//    - ApplicationStopped (усі фонові служби завершили роботу)
// 3. Низькорівневий контракт IHostedService (StartAsync / StopAsync) для ініціалізації інфраструктури.
// 4. Високорівневий базовий клас BackgroundService (ExecuteAsync) для тривалих циклічних воркерів.
// 5. Коректну кооперативну зупинку через CancellationToken та IHostApplicationLifetime.StopApplication().
// ============================================================================

internal static class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  01. Архітектура Generic Host та Фонові служби (IHost, BackgroundService) ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════╝\n");

        // 1. Створення будівельника Generic Host
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Налаштування компактного консольного логування для наочності
        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "[HH:mm:ss.fff] ";
        });
        builder.Logging.SetMinimumLevel(LogLevel.Information);

        // Налаштування параметрів життєвого циклу хоста (таймаут на граційну зупинку)
        builder.Services.Configure<HostOptions>(options =>
        {
            options.ShutdownTimeout = TimeSpan.FromSeconds(10);
        });

        // 2. Реєстрація спільного стану черги телеметрії та фонових служб
        builder.Services.AddSingleton<TelemetryQueue>();

        // IHostedService запускаються в порядку їх реєстрації у контейнері!
        builder.Services.AddHostedService<HostLifecycleObserverService>();
        builder.Services.AddHostedService<InfrastructureWarmupHostedService>();
        builder.Services.AddHostedService<TelemetryProcessingWorker>();

        using IHost host = builder.Build();

        // 3. Запуск хоста (працює до виклику StopApplication у воркері або Ctrl+C)
        await host.RunAsync();

        Console.WriteLine("\n[Main] Метод host.RunAsync() успішно повернув керування, ресурси IHost звільнено.");
    }
}

/// <summary>
/// Потокобезпечна внутрішня черга подій телеметрії для демонстрації роботи фонового воркера.
/// </summary>
public sealed class TelemetryQueue
{
    private readonly Queue<string> _items = new();

    public TelemetryQueue()
    {
        _items.Enqueue("Пакет #1: Датчик температури ЦП (48.5 °C)");
        _items.Enqueue("Пакет #2: Використання оперативної пам'яті (4.2 ГБ)");
        _items.Enqueue("Пакет #3: Статус підключення до локального сховища (OK)");
    }

    public bool TryDequeue(out string? item)
    {
        lock (_items)
        {
            return _items.TryDequeue(out item);
        }
    }

    public int Count
    {
        get
        {
            lock (_items)
            {
                return _items.Count;
            }
        }
    }
}

/// <summary>
/// Служба спостереження за подіями життєвого циклу хоста через <see cref="IHostApplicationLifetime"/>.
/// </summary>
public sealed class HostLifecycleObserverService : IHostedService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<HostLifecycleObserverService> _logger;

    public HostLifecycleObserverService(
        IHostApplicationLifetime lifetime,
        ILogger<HostLifecycleObserverService> logger)
    {
        _lifetime = lifetime;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Підписуємося на три ключові фази життєвого циклу Generic Host
        _lifetime.ApplicationStarted.Register(() =>
        {
            _logger.LogInformation("🟢 [Подія Lifetime] ApplicationStarted: Усі IHostedService стартували, хост готовий до роботи.");
        });

        _lifetime.ApplicationStopping.Register(() =>
        {
            _logger.LogWarning("🟡 [Подія Lifetime] ApplicationStopping: Отримано сигнал зупинки! Зберігаємо проміжний стан...");
        });

        _lifetime.ApplicationStopped.Register(() =>
        {
            _logger.LogInformation("🔴 [Подія Lifetime] ApplicationStopped: Хост повністю зупинено.");
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Звільнення підписок спостерігача життєвого циклу.");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Приклад прямого впровадження <see cref="IHostedService"/> для одноразової ініціалізації
/// перед запуском основних циклічних воркерів.
/// </summary>
public sealed class InfrastructureWarmupHostedService : IHostedService
{
    private readonly ILogger<InfrastructureWarmupHostedService> _logger;

    public InfrastructureWarmupHostedService(ILogger<InfrastructureWarmupHostedService> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛠️ [Warmup] Перевірка локальних каталогів та прогрів кешу перед стартом воркерів...");
        await Task.Delay(150, cancellationToken);
        _logger.LogInformation("✅ [Warmup] Інфраструктуру успішно підготовлено.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🧹 [Warmup] Закриття інфраструктурних дескрипторів під час зупинки хоста.");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Приклад довгоживучої фонової служби на базі абстрактного класу <see cref="BackgroundService"/>.
/// Виконує періодичну обробку черги з використанням <see cref="PeriodicTimer"/> та коректно реагує на <see cref="CancellationToken"/>.
/// </summary>
public sealed class TelemetryProcessingWorker : BackgroundService
{
    private readonly TelemetryQueue _queue;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<TelemetryProcessingWorker> _logger;

    public TelemetryProcessingWorker(
        TelemetryQueue queue,
        IHostApplicationLifetime lifetime,
        ILogger<TelemetryProcessingWorker> logger)
    {
        _queue = queue;
        _lifetime = lifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🚀 [Worker] Фоновий воркер обробки телеметрії розпочав цикл ExecuteAsync.");

        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(250));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                if (_queue.TryDequeue(out string? packet))
                {
                    _logger.LogInformation("📦 [Worker] Оброблено: {Packet} (залишилось у черзі: {Remaining})",
                        packet, _queue.Count);
                }
                else
                {
                    _logger.LogInformation("🏁 [Worker] Усі пакети телеметрії оброблено. Ініціюємо граційну зупинку хоста...");
                    _lifetime.StopApplication();
                    break;
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Очікуваний виняток при скасуванні токена під час зупинки хоста
            _logger.LogWarning("⏹️ [Worker] Цикл воркера перервано за сигналом stoppingToken.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛡️ [Worker] Виконується StopAsync воркера: фіксація контрольної точки перед виходом...");
        await base.StopAsync(cancellationToken);
    }
}
