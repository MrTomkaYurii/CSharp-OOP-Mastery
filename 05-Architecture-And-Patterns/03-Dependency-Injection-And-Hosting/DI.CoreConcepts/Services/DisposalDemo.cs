namespace DI.CoreConcepts.Services;

/// <summary>
/// Ресурс, що демонструє виклик Dispose / DisposeAsync контейнером в порядку LIFO.
/// </summary>
public class ManagedResource : IDisposable, IAsyncDisposable
{
    public string Name { get; }
    public bool IsDisposed { get; private set; }

    public ManagedResource(string name)
    {
        Name = name;
        Console.WriteLine($"      [+] Створено ресурс: {Name}");
    }

    public void Dispose()
    {
        if (IsDisposed) return;
        IsDisposed = true;
        Console.WriteLine($"      [-] Dispose() викликано для: {Name}");
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        if (IsDisposed) return ValueTask.CompletedTask;
        IsDisposed = true;
        Console.WriteLine($"      [-] DisposeAsync() викликано для: {Name}");
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
