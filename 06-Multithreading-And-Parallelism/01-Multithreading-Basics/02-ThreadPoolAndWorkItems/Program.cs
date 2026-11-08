using System.Diagnostics;
using System.Threading;

namespace ThreadPoolAndWorkItems;

/// <summary>
/// Демонстраційний сервіс обробки зображень через пул потоків (ThreadPool).
/// Показує, як невелика кількість потоків повторно використовується для десятків задач.
/// </summary>
public sealed class ImageThumbnailProcessor
{
    /// <summary>
    /// Обробляє один файл у потоці з пулу потоків .NET.
    /// </summary>
    public void ProcessImage(int imageId, CountdownEvent completionSignal)
    {
        try
        {
            Thread current = Thread.CurrentThread;
            // Імітація швидкої роботи процесора
            Thread.SpinWait(50_000);

            Console.WriteLine(
                $"  Зображення #{imageId:D2} оброблено потоком ManagedThreadId = {current.ManagedThreadId,2} " +
                $"(IsThreadPoolThread: {current.IsThreadPoolThread}, IsBackground: {current.IsBackground})");
        }
        finally
        {
            // Сигналізуємо лічильнику, що одне завдання з черги виконано
            completionSignal.Signal();
        }
    }
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 02. ПУЛ ПОТОКІВ (SYSTEM.THREADING.THREADPOOL)");
        Console.WriteLine(" Чому повторне використання потоків у рази ефективніше за new Thread()");
        Console.WriteLine("==========================================================================\n");

        // 1. Інспекція конфігурації ThreadPool
        ThreadPool.GetMinThreads(out int minWorker, out int minIo);
        ThreadPool.GetMaxThreads(out int maxWorker, out int maxIo);

        Console.WriteLine("--- 1. Налаштування системного ThreadPool ---");
        Console.WriteLine($"  Кількість логічних ядер CPU: {Environment.ProcessorCount}");
        Console.WriteLine($"  Мінімум потоків (Worker / I/O): {minWorker} / {minIo}");
        Console.WriteLine($"  Максимум потоків (Worker / I/O): {maxWorker} / {maxIo}\n");

        // 2. Постановка 10 задач у чергу ThreadPool.QueueUserWorkItem
        Console.WriteLine("--- 2. Постановка 10 завдань у чергу ThreadPool ---");
        Console.WriteLine("Зверніть увагу: одні й ті самі ManagedThreadId виконують декілька завдань поспіль!\n");

        const int tasksCount = 10;
        var processor = new ImageThumbnailProcessor();
        using var countdown = new CountdownEvent(tasksCount);

        for (int i = 1; i <= tasksCount; i++)
        {
            int imageId = i; // Локальна копія змінної циклу для уникнення замикання на мутований індекс
            ThreadPool.QueueUserWorkItem(_ => processor.ProcessImage(imageId, countdown));
        }

        // Очікуємо, поки всі 10 елементів черги викличуть countdown.Signal()
        countdown.Wait();

        Console.WriteLine($"\n  Активних потоків у пулі після виконання: {ThreadPool.ThreadCount}");
        Console.WriteLine($"  Загальна кількість виконаних завдань пулом: {ThreadPool.CompletedWorkItemCount}\n");

        // 3. Порівняння накладних витрат: new Thread() vs ThreadPool
        Console.WriteLine("--- 3. Порівняння швидкодії: 200 new Thread() проти 200 задач у ThreadPool ---");
        const int benchmarkItems = 200;

        var sw = Stopwatch.StartNew();
        var manualThreads = new Thread[benchmarkItems];
        for (int i = 0; i < benchmarkItems; i++)
        {
            manualThreads[i] = new Thread(() => { });
            manualThreads[i].Start();
        }
        for (int i = 0; i < benchmarkItems; i++)
        {
            manualThreads[i].Join();
        }
        sw.Stop();
        long manualThreadsMs = sw.ElapsedMilliseconds;

        sw.Restart();
        using var poolDone = new CountdownEvent(benchmarkItems);
        for (int i = 0; i < benchmarkItems; i++)
        {
            ThreadPool.QueueUserWorkItem(_ => poolDone.Signal());
        }
        poolDone.Wait();
        sw.Stop();
        long poolMs = sw.ElapsedMilliseconds;

        Console.WriteLine($"  Створення та запуск 200 нових потоків ОС (new Thread): {manualThreadsMs} ms");
        Console.WriteLine($"  Виконання 200 завдань через ThreadPool:               {poolMs} ms");
        Console.WriteLine("Висновок: ThreadPool уникає системних викликів ядра ОС на створення та знищення потоків.");
    }
}
