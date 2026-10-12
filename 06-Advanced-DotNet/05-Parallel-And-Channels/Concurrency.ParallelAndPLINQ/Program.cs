using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  .NET 10 CONCURRENCY: PARALLEL.FOREACHASYNC ТА PLINQ");
Console.WriteLine("=================================================================\n");

// -------------------------------------------------------------
// 1. ДЕМОНСТРАЦІЯ PARALLEL.FOREACHASYNC (.NET 6+)
// -------------------------------------------------------------
Console.WriteLine("--- 1. Контрольований асинхронний паралелізм (Parallel.ForEachAsync) ---");

var urls = Enumerable.Range(1, 20).Select(i => $"https://api.service.internal/data/{i}").ToList();
var processedResults = new ConcurrentBag<string>();
int totalCompleted = 0;

var parallelOptions = new ParallelOptions
{
    MaxDegreeOfParallelism = 4 // Обмежуємо кількість одночасних асинхронних задач 4-ма потоками
};

var sw = Stopwatch.StartNew();

await Parallel.ForEachAsync(urls, parallelOptions, async (url, ct) =>
{
    int currentId = Interlocked.Increment(ref totalCompleted);
    // Імітуємо I/O операцію (HTTP запит)
    await Task.Delay(100, ct);

    processedResults.Add($"[OK] {url} (Оброблено в потоці #{Environment.CurrentManagedThreadId})");
});

sw.Stop();
Console.WriteLine($"Успішно оброблено {processedResults.Count} URL-адрес за {sw.ElapsedMilliseconds} мс із обмеженням MaxDegreeOfParallelism = 4.");
Console.WriteLine($"Зразок результату: {processedResults.First()}");

// -------------------------------------------------------------
// 2. ДЕМОНСТРАЦІЯ PLINQ (PARALLEL LINQ ДЛЯ CPU-BOUND НАВАНТАЖЕННЯ)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 2. Паралельний LINQ (PLINQ) для CPU-Bound задач ---");

var numbers = Enumerable.Range(100_000, 50_000).ToList();

Console.WriteLine($"Обробка {numbers.Count} чисел: фільтрація простих чисел через PLINQ...");
var plinqSw = Stopwatch.StartNew();

var primes = numbers.AsParallel()
    .WithDegreeOfParallelism(Environment.ProcessorCount)
    .Where(IsPrime)
    .Select(n => (Number: n, Sqrt: Math.Round(Math.Sqrt(n), 4)))
    .ToList();

plinqSw.Stop();
Console.WriteLine($"Знайдено {primes.Count} простих чисел за {plinqSw.ElapsedMilliseconds} мс.");
Console.WriteLine($"Перші 3 простих числа з обчисленням:");
foreach (var p in primes.Take(3))
{
    Console.WriteLine($"  Просте число: {p.Number}, Квадратний корінь: {p.Sqrt}");
}

// -------------------------------------------------------------
// 3. ЗБЕРЕЖЕННЯ ПОРЯДКУ: ASORDERED() VS ПРИРОДНИЙ PLINQ
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 3. Збереження порядку результатів (AsOrdered) у PLINQ ---");

var sampleList = Enumerable.Range(1, 10).ToList();

var unorderedResults = sampleList.AsParallel()
    .Select(x => x * 10)
    .ToList();

var orderedResults = sampleList.AsParallel()
    .AsOrdered()
    .Select(x => x * 10)
    .ToList();

Console.WriteLine($"Без AsOrdered (порядок недетермінований): {string.Join(", ", unorderedResults)}");
Console.WriteLine($"З AsOrdered   (порядок гарантований):    {string.Join(", ", orderedResults)}");

Console.WriteLine("\n=================================================================");
Console.WriteLine("  Concurrency.ParallelAndPLINQ успішно завершив виконання!");
Console.WriteLine("=================================================================");

static bool IsPrime(int number)
{
    if (number <= 1) return false;
    if (number == 2) return true;
    if (number % 2 == 0) return false;

    var boundary = (int)Math.Floor(Math.Sqrt(number));
    for (int i = 3; i <= boundary; i += 2)
    {
        if (number % i == 0) return false;
    }
    return true;
}
