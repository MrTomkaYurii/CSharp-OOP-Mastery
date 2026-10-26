using DependencyInjection.Services.Common;

namespace DependencyInjection.Services.Notifications;

/// <summary>
/// Реалізація сервісу відправки Email-сповіщень.
/// Демонструє впровадження через Property Injection у головному сервісі замовлень.
/// </summary>
public class EmailNotificationService : INotificationService
{
    private readonly ILogger _logger;

    public EmailNotificationService(ILogger logger)
    {
        _logger = logger;
    }

    public void SendNotification(string recipient, string subject, string body)
    {
        _logger.Log($"Email на <{recipient}>: [{subject}] {body}");
    }
}
