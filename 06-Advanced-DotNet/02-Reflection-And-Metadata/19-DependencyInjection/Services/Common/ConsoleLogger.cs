namespace DependencyInjection.Services.Common;

/// <summary>
/// Консольна реалізація сервісу логування.
/// Зберігає унікальний ідентифікатор екземпляра для демонстрації життєвого циклу Singleton.
/// </summary>
public class ConsoleLogger : ILogger
{
    private readonly int _instanceId = Random.Shared.Next(1000, 9999);

    public void Log(string message)
    {
        Console.WriteLine($"      [Logger #{_instanceId}] {message}");
    }
}
