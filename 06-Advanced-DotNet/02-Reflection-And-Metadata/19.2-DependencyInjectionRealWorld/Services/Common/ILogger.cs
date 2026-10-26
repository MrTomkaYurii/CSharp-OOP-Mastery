namespace DependencyInjection.Services.Common;

/// <summary>
/// Інтерфейс системного логування повідомлень.
/// </summary>
public interface ILogger
{
    void Log(string message);
}
