namespace DependencyInjection.Services.Notifications;

/// <summary>
/// Інтерфейс сервісу відправки сповіщень клієнтам.
/// </summary>
public interface INotificationService
{
    void SendNotification(string recipient, string subject, string body);
}
