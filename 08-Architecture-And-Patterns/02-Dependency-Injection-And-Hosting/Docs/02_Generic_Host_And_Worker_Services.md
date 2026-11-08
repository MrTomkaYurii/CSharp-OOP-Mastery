# Розділ 2: Generic Host та Фонова Обробка (Worker Services)

## 1. Що таке Generic Host?
**Generic Host** (`Microsoft.Extensions.Hosting`) — це основа для сучасних консольних додатків, черг, мікросервісів та фонових служб без прив'язки до HTTP-сервера. Він стандартизує:
- Впровадження залежностей (Dependency Injection).
- Конфігурацію додатку (JSON файли, змінні оточення, аргументи командного рядка).
- Логування (`ILogger<T>`, Serilog, структуровані логи).
- Керування життєвим циклом (Запуск, коректне завершення Graceful Shutdown).

### Сучасний підхід: `HostApplicationBuilder` (.NET 7+)
У ранніх версіях використовувався `Host.CreateDefaultBuilder(args)`. Починаючи з .NET 7/8/9/10, рекомендованим стандартом є **`HostApplicationBuilder`**:
```csharp
var builder = Host.CreateApplicationBuilder(args);

// Конфігурація сервісів, опцій, логування
builder.Services.AddHostedService<OrderProcessingWorker>();

var host = builder.Build();
await host.RunAsync();
```

---

## 2. Патерн Опцій (Options Pattern) та Валідація
Замість передачі рядків або `IConfiguration` безпосередньо в класи, використовується типізований клас налаштувань:

```csharp
public class WorkerOptions
{
    public const string SectionName = "WorkerSettings";

    [Required]
    public string WorkerName { get; set; } = string.Empty;

    [Range(1, 60)]
    public int IntervalSeconds { get; set; } = 5;
}
```

### Реєстрація з валідацією при старті (`ValidateOnStart`):
```csharp
builder.Services.AddOptions<WorkerOptions>()
    .Bind(builder.Configuration.GetSection(WorkerOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart(); // Падає при запуску, якщо конфігурація некоректна (Fail-Fast principle)
```

---

## 3. Фонова обробка: `BackgroundService` vs `IHostedService`

| Інтерфейс / Базовий клас | Опис | Особливості застосування |
| :--- | :--- | :--- |
| `IHostedService` | Низькорівневий контракт із двома методами: `StartAsync(CancellationToken)` та `StopAsync(CancellationToken)`. | Швидкі операції запуску/зупинки, реєстрація слухачів подій, ініціалізація підключень. Метод `StartAsync` **не повинен блокувати потік**. |
| `BackgroundService` | Абстрактний клас, що реалізує `IHostedService` та надає метод `ExecuteAsync(CancellationToken)`. | Довготривалі цикли, воркери обробки черг, періодичні cron-задачі. |

### Архітектура довготривалого BackgroundService зі Scoped-сервісами
```csharp
public class OrderWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Створюємо ізольований scope для кожної ітерації
            using (var scope = _scopeFactory.CreateScope())
            {
                var processor = scope.ServiceProvider.GetRequiredService<IOrderProcessor>();
                await processor.ProcessPendingOrdersAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
```

---

## 4. Керування життєвим циклом через `IHostApplicationLifetime`

Служба `IHostApplicationLifetime` дозволяє реагувати на критичні фази роботи процесу:
- **`ApplicationStarted`**: викликається після успішної ініціалізації всіх служб `IHostedService`.
- **`ApplicationStopping`**: надсилається сигнал про зупинку додатку (SIGTERM, Ctrl+C). Тут необхідно зупинити прийом нових задач та дати завершитися поточним операціям (Graceful Shutdown).
- **`ApplicationStopped`**: викликається, коли всі служби завершили роботу безпосередньо перед виходом із процесу.
- **`StopApplication()`**: програмний запит на зупинку хоста зсередини бізнес-коду.
