namespace DI.CoreConcepts.Services;

/// <summary>
/// Інтерфейс сервісу з часом життя Transient.
/// Новий екземпляр створюється щоразу при запиті з контейнера.
/// </summary>
public interface ITransientOperation
{
    Guid Id { get; }
    string GetOperationDetails();
}

/// <summary>
/// Інтерфейс сервісу з часом життя Scoped.
/// Один екземпляр створюється на межі одного scope (наприклад, HTTP-запит або IServiceScope).
/// </summary>
public interface IScopedOperation
{
    Guid Id { get; }
    string GetOperationDetails();
}

/// <summary>
/// Інтерфейс сервісу з часом життя Singleton.
/// Єдиний екземпляр створюється один раз для всього життєвого циклу кореневого контейнера.
/// </summary>
public interface ISingletonOperation
{
    Guid Id { get; }
    string GetOperationDetails();
}

public class TransientOperation : ITransientOperation
{
    public Guid Id { get; } = Guid.NewGuid();
    public string GetOperationDetails() => $"[Transient] ID: {Id:N}";
}

public class ScopedOperation : IScopedOperation
{
    public Guid Id { get; } = Guid.NewGuid();
    public string GetOperationDetails() => $"[Scoped]    ID: {Id:N}";
}

public class SingletonOperation : ISingletonOperation
{
    public Guid Id { get; } = Guid.NewGuid();
    public string GetOperationDetails() => $"[Singleton] ID: {Id:N}";
}

/// <summary>
/// Сервіс звітності, що демонструє отримання залежностей різних життєвих циклів.
/// </summary>
public class OperationReporter
{
    private readonly ITransientOperation _transient;
    private readonly IScopedOperation _scoped;
    private readonly ISingletonOperation _singleton;

    public OperationReporter(
        ITransientOperation transient,
        IScopedOperation scoped,
        ISingletonOperation singleton)
    {
        _transient = transient;
        _scoped = scoped;
        _singleton = singleton;
    }

    public void Report(string callerContext)
    {
        Console.WriteLine($"  [{callerContext}]");
        Console.WriteLine($"    Transient: {_transient.Id:N}");
        Console.WriteLine($"    Scoped:    {_scoped.Id:N}");
        Console.WriteLine($"    Singleton: {_singleton.Id:N}");
    }
}
