using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  .NET 10 CONCURRENCY: СИНХРОНІЗАЦІЯ (SEMAPHORESLIM) ТА CANCELLATION");
Console.WriteLine("=================================================================\n");

// -------------------------------------------------------------
// 1. АСИНХРОННИЙ РЕЙТ-ЛІМІТИНГ З SEMAPHORESLIM
// -------------------------------------------------------------
Console.WriteLine("--- 1. Асинхронне обмеження паралелізму через SemaphoreSlim (Rate Limiting) ---");

// Дозволяємо одночасно виконувати не більше 3-х запитів до зовнішнього ресурсу
using var rateLimiter = new SemaphoreSlim(initialCount: 3, maxCount: 3);

var tasks = Enumerable.Range(1, 9).Select(async id =>
{
    Console.WriteLine($"[Запит #{id}] Очікування вільного слоту в SemaphoreSlim...");
    await rateLimiter.WaitAsync();

    try
    {
        Console.WriteLine($"  >>> [Запит #{id}] Увійшов у критичну секцію! Виконується операція...");
        await Task.Delay(200); // Імітація роботи
    }
    finally
    {
        Console.WriteLine($"  <<< [Запит #{id}] Завершено. Слот звільнено.");
        rateLimiter.Release();
    }
}).ToList();

await Task.WhenAll(tasks);
Console.WriteLine("Всі 9 запитів через SemaphoreSlim успішно завершено.");

// -------------------------------------------------------------
// 2. CANCELLATIONTOKENSOURCE: ТАЙМАУТ ТА ПОВ'ЯЗАНІ ТОКЕНИ
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 2. Розширене скасування: Linked Tokens та таймаути ---");

// Створюємо базовий токен користувача (наприклад, натискання кнопки "Скасувати")
using var userCts = new CancellationTokenSource();

// Створюємо пов'язаний токен із таймаутом 300 мс
using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));

// Об'єднуємо обидва токени: операція буде перервана ЯКЩО спрацює користувач АБО спливе таймаут
using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(userCts.Token, timeoutCts.Token);

Console.WriteLine("Запуск довготривалої асинхронної операції з таймаутом 300 мс...");

try
{
    await ExecuteLongRunningTaskAsync(linkedCts.Token);
    Console.WriteLine("Операція несподівано завершилася без таймауту.");
}
catch (OperationCanceledException)
{
    if (timeoutCts.IsCancellationRequested)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[СКАСОВАНО ТАЙМАУТОМ]: Операцію було примусово перервано після 300 мс.");
        Console.ResetColor();
    }
    else if (userCts.IsCancellationRequested)
    {
        Console.WriteLine("[СКАСОВАНО КОРИСТУВАЧЕМ]: Отримано сигнал скасування від клієнта.");
    }
}

// -------------------------------------------------------------
// 3. НЕБЛОКУЮЧА СИНХРОНІЗАЦІЯ (LOCK-FREE INTERLOCKED)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 3. Атомарні операції без блокування (Interlocked) ---");

long sharedCounter = 0;
var parallelWorkers = Enumerable.Range(1, 10).Select(_ => Task.Run(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        Interlocked.Increment(ref sharedCounter);
    }
})).ToList();

await Task.WhenAll(parallelWorkers);
Console.WriteLine($"Очікуване значення: 10000 | Фактичне атомарне значення: {sharedCounter}");

Console.WriteLine("\n=================================================================");
Console.WriteLine("  Concurrency.SynchronizationAndCancellation успішно завершив роботу!");
Console.WriteLine("=================================================================");

static async Task ExecuteLongRunningTaskAsync(CancellationToken ct)
{
    for (int step = 1; step <= 10; step++)
    {
        ct.ThrowIfCancellationRequested();
        Console.WriteLine($"  Крок виконання #{step}...");
        await Task.Delay(100, ct);
    }
}
