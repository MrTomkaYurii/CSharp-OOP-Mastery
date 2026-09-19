using Microsoft.Extensions.DependencyInjection;

namespace DI.CoreConcepts.Services;

/// <summary>
/// Інтерфейс служби відправки повідомлень для демонстрації Keyed Services (.NET 8+).
/// </summary>
public interface INotificationSender
{
    string ChannelName { get; }
    void Send(string recipient, string message);
}

public class EmailNotificationSender : INotificationSender
{
    public string ChannelName => "Email";
    public void Send(string recipient, string message) =>
        Console.WriteLine($"    [Email] Відправлено лист до <{recipient}>: {message}");
}

public class SmsNotificationSender : INotificationSender
{
    public string ChannelName => "SMS";
    public void Send(string recipient, string message) =>
        Console.WriteLine($"    [SMS] Відправлено SMS на номер <{recipient}>: {message}");
}

public class TelegramNotificationSender : INotificationSender
{
    public string ChannelName => "Telegram";
    public void Send(string recipient, string message) =>
        Console.WriteLine($"    [Telegram] Відправлено повідомлення користувачу @{recipient}: {message}");
}

/// <summary>
/// Менеджер сповіщень, що використовує Keyed Services через атрибут [FromKeyedServices].
/// </summary>
public class AlertDispatchService
{
    private readonly INotificationSender _urgentSender;
    private readonly INotificationSender _standardSender;

    public AlertDispatchService(
        [FromKeyedServices("telegram")] INotificationSender urgentSender,
        [FromKeyedServices("email")] INotificationSender standardSender)
    {
        _urgentSender = urgentSender;
        _standardSender = standardSender;
    }

    public void DispatchUrgentAlert(string user, string text)
    {
        Console.WriteLine("  Термінове сповіщення через пріоритетний канал:");
        _urgentSender.Send(user, text);
    }

    public void DispatchStandardDigest(string user, string text)
    {
        Console.WriteLine("  Регулярний дайджест через стандартний канал:");
        _standardSender.Send(user, text);
    }
}
