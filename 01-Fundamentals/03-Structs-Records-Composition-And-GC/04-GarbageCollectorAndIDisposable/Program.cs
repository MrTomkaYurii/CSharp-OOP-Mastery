using System.Runtime.CompilerServices;

namespace GarbageCollectorAndIDisposable;

/// <summary>
/// Демонструє детерміноване звільнення ресурсів через інтерфейс IDisposable.
/// На відміну від Garbage Collector (який звільняє лише пам'ять у непередбачуваний момент),
/// метод Dispose() викликається миттєво при виході з блоку 'using'.
/// </summary>
public sealed class DatabaseSessionMock : IDisposable
{
    public string SessionId { get; }
    private bool _disposed;

    public DatabaseSessionMock(string sessionId)
    {
        SessionId = sessionId;
        Console.WriteLine($"  [Ресурс відкрито] Сесія '{SessionId}' утримує підключення.");
    }

    public void ExecuteQuery(string sql)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Console.WriteLine($"  [Виконання SQL у '{SessionId}']: {sql}");
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Console.WriteLine($"  [Dispose викликано] Сесія '{SessionId}' миттєво звільнила сокет і пам'ять!");
        GC.SuppressFinalize(this);
    }
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 04. ЖИТТЄВИЙ ЦИКЛ ОБ'ЄКТА У ПАМ'ЯТІ: GARBAGE COLLECTOR ТА IDISPOSABLE");
        Console.WriteLine(" Як працюють покоління купи (Gen 0, Gen 1, Gen 2) та детермінований using");
        Console.WriteLine("==========================================================================\n");

        // 1. Покоління збирача сміття (Generations 0, 1, 2)
        Console.WriteLine("--- 1. Просування об'єкта між поколіннями Garbage Collector (Gen 0 -> Gen 1 -> Gen 2) ---");
        object longLivedCache = new byte[1024];
        Console.WriteLine($"  Покоління відразу після 'new':      Gen {GC.GetGeneration(longLivedCache)}");

        GC.Collect(0, GCCollectionMode.Forced, blocking: true);
        Console.WriteLine($"  Покоління після першого GC(0):      Gen {GC.GetGeneration(longLivedCache)} (об'єкт вижив і перейшов у Gen 1)");

        GC.Collect(1, GCCollectionMode.Forced, blocking: true);
        Console.WriteLine($"  Покоління після другого GC(1):      Gen {GC.GetGeneration(longLivedCache)} (об'єкт перейшов у довгоживуче Gen 2)\n");

        // 2. Перевірка збирання короткоживучого об'єкта через WeakReference
        Console.WriteLine("--- 2. Перевірка звільнення пам'яті через слабке посилання (WeakReference) ---");
        WeakReference weakRef = CreateTemporaryObjectInIsolatedMethod();
        Console.WriteLine($"  Об'єкт ще в пам'яті до GC.Collect()? {weakRef.IsAlive}");

        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine($"  Об'єкт в пам'яті після GC.Collect()? {weakRef.IsAlive} (пам'ять успішно очищено!)\n");

        // 3. Детерміноване звільнення некерованих ресурсів через using та IDisposable
        Console.WriteLine("--- 3. Конструкція using та миттєвий виклик IDisposable.Dispose() ---");
        using (var session = new DatabaseSessionMock("DB-CONN-01"))
        {
            session.ExecuteQuery("SELECT Id, Name FROM Customers");
        } // <-- Тут автоматично у блоці finally викликається session.Dispose()!

        GC.KeepAlive(longLivedCache);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateTemporaryObjectInIsolatedMethod()
    {
        var tempBuffer = new byte[4096];
        return new WeakReference(tempBuffer);
    }
}
