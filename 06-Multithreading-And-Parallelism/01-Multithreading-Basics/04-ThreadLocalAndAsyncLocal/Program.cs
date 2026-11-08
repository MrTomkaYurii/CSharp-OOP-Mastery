using System.Threading;
using System.Threading.Tasks;

namespace ThreadLocalAndAsyncLocal;

/// <summary>
/// Демонстрація ізоляції стану між потоками (ThreadLocal)
/// та збереження контексту вздовж асинхронних викликів (AsyncLocal).
/// </summary>
public static class AmbientContextDemo
{
    // 1. ThreadLocal<T>: кожен фізичний потік отримує СВОЮ власну копію значення.
    // Проте під час 'await' виконання може перестрибнути на ІНШИЙ потік із ThreadPool!
    public static readonly ThreadLocal<string> PerThreadName = new(
        valueFactory: () => $"Init-Thread-{Environment.CurrentManagedThreadId}");

    // 2. AsyncLocal<T>: значення "подорожує" разом із логічним потоком виконання (ExecutionContext)
    // навіть коли після await код продовжує роботу на іншому потоці ThreadPool!
    public static readonly AsyncLocal<string?> CorrelationId = new();
}

internal class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 04. ІЗОЛЯЦІЯ ДАНИХ ПОТОКУ: THREADLOCAL<T> ПРОТИ ASYNCLOCAL<T>");
        Console.WriteLine(" Як передавати CorrelationId або контекст користувача без спільного стану");
        Console.WriteLine("==========================================================================\n");

        // 1. Демонстрація ThreadLocal<T>
        Console.WriteLine("--- 1. ThreadLocal<T>: Ізольоване сховище для кожного фізичного потоку ---");
        var t1 = new Thread(() =>
        {
            AmbientContextDemo.PerThreadName.Value = "Worker-Alpha";
            Thread.Sleep(30);
            Console.WriteLine($"  [Потік #{Environment.CurrentManagedThreadId}] Значення ThreadLocal: {AmbientContextDemo.PerThreadName.Value}");
        });

        var t2 = new Thread(() =>
        {
            AmbientContextDemo.PerThreadName.Value = "Worker-Beta";
            Thread.Sleep(30);
            Console.WriteLine($"  [Потік #{Environment.CurrentManagedThreadId}] Значення ThreadLocal: {AmbientContextDemo.PerThreadName.Value}");
        });

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();
        Console.WriteLine($"  [Головний потік #{Environment.CurrentManagedThreadId}] Значення ThreadLocal: {AmbientContextDemo.PerThreadName.Value}\n");

        // 2. Демонстрація AsyncLocal<T> при перемиканні потоків під час await
        Console.WriteLine("--- 2. AsyncLocal<T>: Збереження контексту крізь асинхронні переходи (await) ---");

        Task requestA = SimulateHttpRequestAsync("REQ-1001-UA", delayMs: 60);
        Task requestB = SimulateHttpRequestAsync("REQ-2002-EU", delayMs: 40);

        await Task.WhenAll(requestA, requestB);

        Console.WriteLine("\nВисновок:");
        Console.WriteLine("  - ThreadLocal<T> прив'язаний до конкретного фізичного потоку ОС (ідеально для генераторів Random, буферів).");
        Console.WriteLine("  - AsyncLocal<T> прив'язаний до логічного ланцюжка Task/await (ідеально для TraceId / CorrelationId у логуванні).");
    }

    private static async Task SimulateHttpRequestAsync(string requestId, int delayMs)
    {
        // Записуємо ідентифікатор запиту у логічний контекст поточного асинхронного виклику
        AmbientContextDemo.CorrelationId.Value = requestId;

        Console.WriteLine($"  [Старт {requestId}] Потік до await: #{Environment.CurrentManagedThreadId,-2} | CorrelationId = {AmbientContextDemo.CorrelationId.Value}");

        await Task.Delay(delayMs);

        // Навіть якщо після await код виконується на іншому потоці пулу, AsyncLocal зберігає правильний CorrelationId!
        Console.WriteLine($"  [Фініш {requestId}] Потік після await: #{Environment.CurrentManagedThreadId,-2} | CorrelationId = {AmbientContextDemo.CorrelationId.Value}");
    }
}
