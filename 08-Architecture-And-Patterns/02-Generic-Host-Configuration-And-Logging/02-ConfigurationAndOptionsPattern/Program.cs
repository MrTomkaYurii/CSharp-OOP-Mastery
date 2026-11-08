using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConfigurationAndOptionsPattern;

// ============================================================================
// ПРОЄКТ 02: Конфігураційні файли та Патерн Options
//            (IConfiguration, appsettings.json, IOptions, ValidateOnStart)
// ============================================================================
// Демонструє:
// 1. Каскадне об'єднання джерел конфігурації у Generic Host:
//    appsettings.json -> appsettings.{Environment}.json -> Змінні середовища -> Аргументи CLI.
// 2. Роботу з ієрархічними секціями IConfiguration.GetSection("...").
// 3. Патерн Options та різницю між:
//    - IOptions<T>         (Singleton, читається один раз при старті)
//    - IOptionsSnapshot<T> (Scoped, перечитується для кожного нового Scope)
//    - IOptionsMonitor<T>  (Singleton із реактивним сповіщенням OnChange)
// 4. Декларативну валідацію конфігурації через DataAnnotations та ValidateOnStart().
// ============================================================================

internal static class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  02. Конфігураційні файли та Патерн Options (IConfiguration, IOptions<T>)    ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝\n");

        // Імітуємо змінну середовища та аргументи командного рядка для демонстрації пріоритетів
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
        Environment.SetEnvironmentVariable("DatabaseSettings__MaxRetryCount", "5");

        string[] effectiveArgs = args.Length > 0
            ? args
            : ["--OrderProcessing:MaxDiscountPercent=20.5"];

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(effectiveArgs);
        builder.Configuration.SetBasePath(AppContext.BaseDirectory);
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddCommandLine(effectiveArgs);

        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(o =>
        {
            o.SingleLine = true;
            o.TimestampFormat = "[HH:mm:ss] ";
        });

        // 1. Строга реєстрація та валідація секції DatabaseSettings
        builder.Services
            .AddOptions<DatabaseSettings>()
            .Bind(builder.Configuration.GetSection(DatabaseSettings.SectionName))
            .ValidateDataAnnotations()
            .Validate(
                settings => !settings.ConnectionString.Contains(" Drop ", StringComparison.OrdinalIgnoreCase),
                "Рядок підключення містить заборонені інструкції!")
            .ValidateOnStart();

        // 2. Строга реєстрація та валідація секції OrderProcessingSettings
        builder.Services
            .AddOptions<OrderProcessingSettings>()
            .Bind(builder.Configuration.GetSection(OrderProcessingSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddScoped<ScopedConfigurationInspector>();

        using IHost host = builder.Build();

        // Запуск хоста виконує всі зареєстровані перевірки ValidateOnStart()!
        await host.StartAsync();

        // --- ЧАСТИНА 1: Ієрархічне читання через IConfiguration ---
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        Console.WriteLine("1. Каскадне злиття налаштувань (IConfiguration):");
        Console.WriteLine($"   • Середовище хоста (IHostEnvironment): {builder.Environment.EnvironmentName}");
        Console.WriteLine($"   • Назва системи (з appsettings.json):  {config["ApplicationInfo:SystemTitle"]}");
        Console.WriteLine($"   • Тег середовища (перекрито у Dev):   {config["ApplicationInfo:EnvironmentTag"]}\n");

        // --- ЧАСТИНА 2: IOptions<T>, IOptionsSnapshot<T>, IOptionsMonitor<T> ---
        Console.WriteLine("2. Строго типізовані налаштування у межах Scoped-операції:");
        using (IServiceScope scope = host.Services.CreateScope())
        {
            var inspector = scope.ServiceProvider.GetRequiredService<ScopedConfigurationInspector>();
            inspector.PrintCurrentConfiguration();
        }

        // --- ЧАСТИНА 3: Демонстрація захисту ValidateOnStart при хибній конфігурації ---
        Console.WriteLine("\n3. Перевірка механізму Fail-Fast (ValidateOnStart) на помилкових даних:");
        DemonstrateFailFastValidation();

        await host.StopAsync();
    }

    private static void DemonstrateFailFastValidation()
    {
        var invalidBuilder = Host.CreateApplicationBuilder();
        invalidBuilder.Logging.ClearProviders();

        // Навмисно подаємо невалідні значення у пам'яті (BatchSize = -5, порожній ConnectionString)
        invalidBuilder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["DatabaseSettings:ConnectionString"] = "",
            ["DatabaseSettings:CommandTimeoutSeconds"] = "9999", // Перевищує [Range(1, 300)]
            ["DatabaseSettings:MaxRetryCount"] = "0"
        });

        invalidBuilder.Services
            .AddOptions<DatabaseSettings>()
            .Bind(invalidBuilder.Configuration.GetSection(DatabaseSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        using IHost invalidHost = invalidBuilder.Build();

        try
        {
            // При старті хоста миттєво спрацює OptionsValidationException
            invalidHost.Start();
        }
        catch (OptionsValidationException ex)
        {
            Console.WriteLine("   ⚡ Перехоплено OptionsValidationException ще ДО початку роботи бізнес-сервісів!");
            foreach (string failure in ex.Failures)
            {
                Console.WriteLine($"      ❌ Причина відмови: {failure}");
            }
        }
    }
}

/// <summary>
/// Строго типізована модель конфігурації бази даних із декларативними атрибутами валідації.
/// </summary>
public sealed class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";

    [Required(ErrorMessage = "Рядок підключення ConnectionString є обов'язковим.")]
    public string ConnectionString { get; set; } = string.Empty;

    [Range(1, 300, ErrorMessage = "Таймаут команди має бути в діапазоні від 1 до 300 секунд.")]
    public int CommandTimeoutSeconds { get; set; } = 30;

    [Range(1, 10, ErrorMessage = "Кількість повторних спроб MaxRetryCount має бути від 1 до 10.")]
    public int MaxRetryCount { get; set; } = 3;

    public bool EnableSensitiveDataLogging { get; set; }
}

/// <summary>
/// Строго типізована модель конфігурації обробки замовлень.
/// </summary>
public sealed class OrderProcessingSettings
{
    public const string SectionName = "OrderProcessing";

    [Range(1, 500, ErrorMessage = "Розмір пакету BatchSize має бути від 1 до 500.")]
    public int BatchSize { get; set; } = 20;

    [Range(0.0, 50.0, ErrorMessage = "Максимальна знижка має бути в межах 0..50%.")]
    public double MaxDiscountPercent { get; set; } = 10.0;

    [Required, EmailAddress(ErrorMessage = "Некоректний формат електронної пошти підтримки.")]
    public string SupportEmail { get; set; } = string.Empty;
}

/// <summary>
/// Сервіс, що демонструє інжекцію трьох різновидів інтерфейсів Options.
/// </summary>
public sealed class ScopedConfigurationInspector
{
    private readonly DatabaseSettings _singletonDbOptions;
    private readonly OrderProcessingSettings _scopedOrderSnapshot;
    private readonly IOptionsMonitor<OrderProcessingSettings> _orderMonitor;

    public ScopedConfigurationInspector(
        IOptions<DatabaseSettings> dbOptions,
        IOptionsSnapshot<OrderProcessingSettings> orderSnapshot,
        IOptionsMonitor<OrderProcessingSettings> orderMonitor)
    {
        _singletonDbOptions = dbOptions.Value;
        _scopedOrderSnapshot = orderSnapshot.Value;
        _orderMonitor = orderMonitor;
    }

    public void PrintCurrentConfiguration()
    {
        Console.WriteLine("   [IOptions<DatabaseSettings>] (Singleton-фіксація):");
        Console.WriteLine($"     • ConnectionString:           {_singletonDbOptions.ConnectionString} (з appsettings.Development.json)");
        Console.WriteLine($"     • CommandTimeoutSeconds:      {_singletonDbOptions.CommandTimeoutSeconds} с (з appsettings.json)");
        Console.WriteLine($"     • MaxRetryCount:              {_singletonDbOptions.MaxRetryCount} (перекрито через змінну середовища)");
        Console.WriteLine($"     • EnableSensitiveDataLogging: {_singletonDbOptions.EnableSensitiveDataLogging}");

        Console.WriteLine("   [IOptionsSnapshot<OrderProcessingSettings>] (Scoped-знімок):");
        Console.WriteLine($"     • BatchSize:                  {_scopedOrderSnapshot.BatchSize} (з appsettings.Development.json)");
        Console.WriteLine($"     • MaxDiscountPercent:         {_scopedOrderSnapshot.MaxDiscountPercent}% (перекрито через CLI-аргумент)");
        Console.WriteLine($"     • SupportEmail:               {_scopedOrderSnapshot.SupportEmail}");

        Console.WriteLine("   [IOptionsMonitor<OrderProcessingSettings>] (Монітор реального часу):");
        Console.WriteLine($"     • Поточне значення CurrentValue.BatchSize = {_orderMonitor.CurrentValue.BatchSize}");
    }
}
