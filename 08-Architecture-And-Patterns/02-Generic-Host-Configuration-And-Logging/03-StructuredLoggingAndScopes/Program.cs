using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StructuredLoggingAndScopes;

// ============================================================================
// ПРОЄКТ 03: Система логування в .NET
//            (ILogger<T>, Рівні логування, Структуроване логування, Scopes та [LoggerMessage])
// ============================================================================
// Демонструє:
// 1. Взаємодію ILogger<T> та ILoggerFactory (категорії логів за повним ім'ям типу).
// 2. Рівні логування (Trace, Debug, Information, Warning, Error, Critical)
//    та гранулярну фільтрацію за категоріями через конфігурацію Logging:LogLevel.
// 3. Структуроване логування параметрів за шаблоном (Message Templates) замість інтерполяції.
// 4. Контекстні області видимості logger.BeginScope(...) для наскрізного трасування (CorrelationId).
// 5. Високопродуктивну генерацію коду логування на етапі компіляції через атрибут [LoggerMessage].
// ============================================================================

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  03. Система логування в .NET (ILogger, Структуровані шаблони, Scopes)       ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝\n");

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Налаштовуємо фільтрацію рівнів логування за категоріями через конфігурацію
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Logging:LogLevel:Default"] = "Information",
            ["Logging:LogLevel:Microsoft"] = "Warning",
            // Для сервісу оплати вмикаємо детальний рівень Debug
            ["Logging:LogLevel:StructuredLoggingAndScopes.OrderCheckoutService"] = "Debug"
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(options =>
        {
            // Вмикаємо відображення контекстних областей (Scopes) у консолі!
            options.IncludeScopes = true;
            options.SingleLine = false;
            options.TimestampFormat = "[HH:mm:ss.fff] ";
        });

        builder.Services.AddTransient<OrderCheckoutService>();

        using IHost host = builder.Build();

        // 1. Створення кастомної категорії через ILoggerFactory
        ILoggerFactory loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
        ILogger auditLogger = loggerFactory.CreateLogger("Security.AuditTrail");
        auditLogger.LogInformation("Запущено сеанс аудиту безпеки. Користувач: {UserName}", "admin_ua");

        // 2. Виконання бізнес-сценарію зі структурованим логуванням та областями (Scopes)
        OrderCheckoutService checkoutService = host.Services.GetRequiredService<OrderCheckoutService>();

        checkoutService.ProcessOrder(orderId: 1042, customerId: "CUST-901", amount: 4850.00m);
        Console.WriteLine(new string('─', 78));
        checkoutService.ProcessOrder(orderId: 1043, customerId: "CUST-902", amount: -150.00m);
    }
}

/// <summary>
/// Сервіс оформлення замовлення, який демонструє рівні логування, структуровані шаблони,
/// контекстні області <see cref="ILogger.BeginScope{TState}"/> та генератор <see cref="LoggerMessageAttribute"/>.
/// </summary>
public sealed class OrderCheckoutService
{
    private readonly ILogger<OrderCheckoutService> _logger;

    public OrderCheckoutService(ILogger<OrderCheckoutService> logger)
    {
        _logger = logger;
    }

    public void ProcessOrder(int orderId, string customerId, decimal amount)
    {
        string correlationId = $"REQ-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        // Відкриваємо контекстну область (Scope): усі записи всередині блоку using
        // автоматично отримають метадані CorrelationId, OrderId та CustomerId!
        using IDisposable? scope = _logger.BeginScope(
            "CorrelationId={CorrelationId}, OrderId={OrderId}, Customer={CustomerId}",
            correlationId, orderId, customerId);

        // Рівень Trace буде відфільтровано (оскільки для цього класу мінімальний рівень — Debug)
        _logger.LogTrace("Сирі байти запиту перевірено (цей запис відфільтровано конфігурацією).");

        // Рівень Debug відобразиться завдяки правилу Logging:LogLevel:StructuredLoggingAndScopes.OrderCheckoutService = Debug
        _logger.LogDebug("Початок перевірки кошика клієнта {CustomerId} перед резервуванням.", customerId);

        if (amount <= 0)
        {
            _logger.LogWarning("Некоректна сума замовлення {OrderId}: передано {Amount} грн.", orderId, amount);

            try
            {
                throw new InvalidOperationException($"Сума замовлення #{orderId} має бути додатною (отримано {amount}).");
            }
            catch (InvalidOperationException ex)
            {
                // Передаємо об'єкт винятку першим аргументом у LogError
                _logger.LogError(
                    new EventId(5001, "InvalidOrderAmount"),
                    ex,
                    "Помилка обробки замовлення {OrderId} на суму {Amount} грн",
                    orderId,
                    amount);
                return;
            }
        }

        // Структуроване логування з іменованими заповнювачами (Message Template)
        _logger.LogInformation("Оброблено замовлення {OrderId} на суму {Amount} грн", orderId, amount);

        // Виклик високопродуктивного source-generated лог-методу (без алокації масивів params та боксингу!)
        HighPerformanceLogs.LogPaymentAuthorized(_logger, orderId, amount, "UAH");
    }
}

/// <summary>
/// Високопродуктивні делегати логування, згенеровані компілятором за допомогою <see cref="LoggerMessageAttribute"/>.
/// Усувають боксинг значущих типів (int, decimal) та повторний парсинг шаблону повідомлення.
/// </summary>
internal static partial class HighPerformanceLogs
{
    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Information,
        Message = "⚡ [SourceGenerated] Платіж для замовлення #{OrderId} на суму {Amount} {Currency} авторизовано шлюзом.")]
    public static partial void LogPaymentAuthorized(
        ILogger logger,
        int orderId,
        decimal amount,
        string currency);
}
