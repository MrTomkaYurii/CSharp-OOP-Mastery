using System.Threading;

namespace ThreadCreationAndLifecycle;

/// <summary>
/// Клас воркера, який моделює генерацію звіту в окремому потоці ОС.
/// </summary>
public sealed class ReportWorker
{
    private readonly string _reportName;
    private readonly int _pagesCount;

    public ReportWorker(string reportName, int pagesCount)
    {
        _reportName = reportName;
        _pagesCount = pagesCount;
    }

    /// <summary>
    /// Метод, який виконуватиметься у виділеному потоці (ThreadStart).
    /// </summary>
    public void GenerateReport()
    {
        Thread current = Thread.CurrentThread;
        Console.WriteLine($"  [{current.Name} | ID: {current.ManagedThreadId} | Background: {current.IsBackground}] Початок генерації '{_reportName}'...");

        for (int page = 1; page <= _pagesCount; page++)
        {
            // Thread.Sleep переводить поточний потік у стан WaitSleepJoin,
            // звільняючи ядро процесора для інших потоків ОС.
            Thread.Sleep(80);
            Console.WriteLine($"  [{current.Name}] Опрацьовано сторінку {page}/{_pagesCount} (Стан: {current.ThreadState})");
        }

        Console.WriteLine($"  [{current.Name}] Генерацію '{_reportName}' завершено!");
    }
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Thread.CurrentThread.Name = "MainThread";

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 01. СТВОРЕННЯ ПОТОКІВ (SYSTEM.THREADING.THREAD) ТА ЇХ ЖИТТЄВИЙ ЦИКЛ");
        Console.WriteLine(" Як працюють потоки ОС, Foreground vs Background та очікування через Join()");
        Console.WriteLine("==========================================================================\n");

        // 1. Інспекція головного потоку програми
        Thread main = Thread.CurrentThread;
        Console.WriteLine("--- 1. Характеристики головного потоку (MainThread) ---");
        Console.WriteLine($"  Name:             {main.Name}");
        Console.WriteLine($"  ManagedThreadId:  {main.ManagedThreadId}");
        Console.WriteLine($"  Priority:         {main.Priority}");
        Console.WriteLine($"  IsBackground:     {main.IsBackground} (Foreground-потік тримає процес живим)");
        Console.WriteLine($"  IsThreadPool:     {main.IsThreadPoolThread}\n");

        // 2. Створення та запуск виділеного Foreground-потоку
        Console.WriteLine("--- 2. Запуск виділеного потоку та життєвий цикл (ThreadState) ---");
        var worker = new ReportWorker("Фінансовий звіт Q4", pagesCount: 3);

        // Створюємо об'єкт Thread. У цей момент потік ОС ще не запущений (Unstarted).
        Thread reportThread = new Thread(worker.GenerateReport)
        {
            Name = "ReportWorker-1",
            IsBackground = false // Foreground потік: програма не закриється, поки він не завершиться
        };

        Console.WriteLine($"Стан потоку до виклику Start(): {reportThread.ThreadState}");
        reportThread.Start();
        Console.WriteLine($"Стан потоку відразу після Start(): {reportThread.ThreadState}");

        // Join() блокує головний потік, поки reportThread повністю не завершить роботу.
        reportThread.Join();
        Console.WriteLine($"Стан потоку після завершення та Join(): {reportThread.ThreadState}\n");

        // 3. Різниця між Foreground та Background потоками
        Console.WriteLine("--- 3. Фоновий потік (IsBackground = true) ---");
        Thread telemetryThread = new Thread(() =>
        {
            Console.WriteLine($"  [Telemetry | ID: {Thread.CurrentThread.ManagedThreadId}] Фоновий моніторинг активний (IsBackground = {Thread.CurrentThread.IsBackground}).");
            Thread.Sleep(50);
            Console.WriteLine("  [Telemetry] Збір метрик успішно виконано.");
        })
        {
            Name = "TelemetryBgThread",
            IsBackground = true // Фоновий потік автоматично зупиняється при виході з Main()
        };

        telemetryThread.Start();
        telemetryThread.Join(); // Чекаємо завершення для чистого виводу в консоль

        Console.WriteLine("\nВисновок: Кожен екземпляр класу Thread виділяє ~1 МБ пам'яті під стек викликів ОС.");
        Console.WriteLine("Тому для тисяч коротких операцій використовують ThreadPool (див. приклад 02).");
    }
}
