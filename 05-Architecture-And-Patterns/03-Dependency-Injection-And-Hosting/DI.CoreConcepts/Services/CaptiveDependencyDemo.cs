using Microsoft.Extensions.DependencyInjection;

namespace DI.CoreConcepts.Services;

/// <summary>
/// Сервіс, який є Scoped (наприклад, контекст користувача або транзакція БД).
/// </summary>
public interface IUserSessionScope
{
    Guid SessionId { get; }
    string CurrentUser { get; set; }
}

public class UserSessionScope : IUserSessionScope
{
    public Guid SessionId { get; } = Guid.NewGuid();
    public string CurrentUser { get; set; } = "Гість";
}

/// <summary>
/// Небезпечний Singleton, який помилково захоплює Scoped-сервіс.
/// Це класичний антипатерн "Captive Dependency": Singleton житиме вічно і буде
/// тримати посилання на Scoped сервіс з першого виклику, викликаючи витік пам'яті
/// та підміну контексту користувача між різними запитами.
/// </summary>
public class BadCaptiveSingleton
{
    public IUserSessionScope CapturedSession { get; }

    public BadCaptiveSingleton(IUserSessionScope session)
    {
        CapturedSession = session;
    }
}

/// <summary>
/// Правильне рішення: Singleton інжектує IServiceScopeFactory або IServiceProvider
/// і створює scope виключно за потребою, коли виконує конкретну операцію.
/// </summary>
public class SafeScopeAwareSingleton
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SafeScopeAwareSingleton(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void ExecuteInNewScope(Action<IUserSessionScope> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var session = scope.ServiceProvider.GetRequiredService<IUserSessionScope>();
        action(session);
    }
}
