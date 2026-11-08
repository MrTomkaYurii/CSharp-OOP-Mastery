using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace MultipleAndKeyedServices;

// ============================================================================
// ПРОЄКТ 03: Множинні реалізації інтерфейсів, TryAdd та Keyed Services (.NET 8+)
// ============================================================================
// Демонструє:
// 1. Впровадження всіх реалізацій одного контракту через IEnumerable<INotificationChannel>
//    (Патерн Composite / Broadcast).
// 2. Поведінку при запиті одиничного INotificationChannel (повертається остання зареєстрована реалізація)
//    та безпечну реєстрацію за замовчуванням через TryAddScoped / TryAddEnumerable.
// 3. Іменовані (Keyed) сервіси (.NET 8+): AddKeyedScoped<INotificationChannel, ...>("sms")
//    та атрибут [FromKeyedServices("sms")] для точкового вибору стратегії без Service Locator.
// 4. Реєстрацію через фабричний делегат sp => new ... для умовного конструювання об'єктів.
// ============================================================================

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  03. Множинні реалізації (IEnumerable<T>), TryAdd та Keyed Services (.NET 8+)║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝\n");

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // 1. Реєстрація множинних реалізацій одного інтерфейсу для широкомовного сповіщення
        builder.Services.AddScoped<INotificationChannel, EmailNotificationChannel>();
        builder.Services.AddScoped<INotificationChannel, SmsNotificationChannel>();
        builder.Services.AddScoped<INotificationChannel, TelegramNotificationChannel>();

        // TryAddEnumerable запобігає випадковому дублюванню тієї ж пари (Інтерфейс -> Реалізація)
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Scoped<INotificationChannel, TelegramNotificationChannel>());

        // 2. Реєстрація Keyed Services (іменованих стратегій за строковим ключем)
        builder.Services.AddKeyedScoped<INotificationChannel, EmailNotificationChannel>("email");
        builder.Services.AddKeyedScoped<INotificationChannel, SmsNotificationChannel>("sms");
        builder.Services.AddKeyedScoped<INotificationChannel, TelegramNotificationChannel>("telegram");

        // 3. Фабричний делегат реєстрації: динамічне налаштування резервного шлюзу
        builder.Services.AddKeyedScoped<INotificationChannel>("urgent-fallback", (sp, key) =>
        {
            var sms = sp.GetRequiredKeyedService<INotificationChannel>("sms");
            return new UrgentPrefixDecoratorChannel(sms, prefix: $"[КЛЮЧ:{key}] ТЕРМІНОВО");
        });

        builder.Services.AddScoped<BroadcastNotificationDispatcher>();
        builder.Services.AddScoped<SecurityAlertService>();

        using IHost host = builder.Build();

        using IServiceScope scope = host.Services.CreateScope();
        IServiceProvider sp = scope.ServiceProvider;

        // --- ЧАСТИНА 1: Широкомовна розсилка через IEnumerable<INotificationChannel> ---
        Console.WriteLine("1. Впровадження IEnumerable<INotificationChannel> (усі зареєстровані канали):");
        var broadcaster = sp.GetRequiredService<BroadcastNotificationDispatcher>();
        broadcaster.BroadcastToAll("Сервер бази даних планово оновлено до версії 10.2.");

        // --- ЧАСТИНА 2: Вибір конкретної стратегії через [FromKeyedServices] ---
        Console.WriteLine("\n2. Точковий вибір стратегії через [FromKeyedServices(\"sms\")] та фабричний ключ:");
        var securityService = sp.GetRequiredService<SecurityAlertService>();
        securityService.SendTwoFactorCode("+380501234567", "849201");
        securityService.SendCriticalBreachAlert("Виявлено спробу несанкціонованого доступу до шлюзу!");
    }
}

/// <summary>
/// Єдиний абстрактний контракт каналу доставки повідомлень.
/// </summary>
public interface INotificationChannel
{
    string ChannelName { get; }
    void Send(string message);
}

public sealed class EmailNotificationChannel : INotificationChannel
{
    public string ChannelName => "Email (SMTP)";
    public void Send(string message) =>
        Console.WriteLine($"    [{ChannelName}] Відправлено лист: {message}");
}

public sealed class SmsNotificationChannel : INotificationChannel
{
    public string ChannelName => "SMS-Шлюз";
    public void Send(string message) =>
        Console.WriteLine($"    [{ChannelName}] Відправлено SMS: {message}");
}

public sealed class TelegramNotificationChannel : INotificationChannel
{
    public string ChannelName => "Telegram-Бот";
    public void Send(string message) =>
        Console.WriteLine($"   ✈️ [{ChannelName}] Відправлено у Telegram: {message}");
}

/// <summary>
/// Декоратор, що створюється через фабричний делегат у контейнері DI.
/// </summary>
public sealed class UrgentPrefixDecoratorChannel : INotificationChannel
{
    private readonly INotificationChannel _inner;
    private readonly string _prefix;

    public string ChannelName => $"Декоратор({_inner.ChannelName})";

    public UrgentPrefixDecoratorChannel(INotificationChannel inner, string prefix)
    {
        _inner = inner;
        _prefix = prefix;
    }

    public void Send(string message) =>
        _inner.Send($"{_prefix}: {message}");
}

/// <summary>
/// Сервіс, що демонструє інжекцію колекції всіх реалізацій <see cref="IEnumerable{INotificationChannel}"/>.
/// </summary>
public sealed class BroadcastNotificationDispatcher
{
    private readonly IReadOnlyList<INotificationChannel> _channels;

    public BroadcastNotificationDispatcher(IEnumerable<INotificationChannel> channels)
    {
        _channels = channels.ToList();
    }

    public void BroadcastToAll(string announcement)
    {
        Console.WriteLine($"   Знайдено активних каналів у контейнері: {_channels.Count}");
        foreach (INotificationChannel channel in _channels)
        {
            channel.Send(announcement);
        }
    }
}

/// <summary>
/// Сервіс безпеки, який за допомогою атрибута <see cref="FromKeyedServicesAttribute"/>
/// запитує конкретні реалізації <see cref="INotificationChannel"/> за їхнім ключем.
/// </summary>
public sealed class SecurityAlertService
{
    private readonly INotificationChannel _smsChannel;
    private readonly INotificationChannel _urgentFallbackChannel;

    public SecurityAlertService(
        [FromKeyedServices("sms")] INotificationChannel smsChannel,
        [FromKeyedServices("urgent-fallback")] INotificationChannel urgentFallbackChannel)
    {
        _smsChannel = smsChannel;
        _urgentFallbackChannel = urgentFallbackChannel;
    }

    public void SendTwoFactorCode(string phone, string code)
    {
        _smsChannel.Send($"Одноразовий код підтвердження для {phone}: {code}");
    }

    public void SendCriticalBreachAlert(string details)
    {
        _urgentFallbackChannel.Send(details);
    }
}
