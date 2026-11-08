using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSlimAndReaderWriterLock;

/// <summary>
/// Кеш конфігурації, який читається дуже часто багатьма потоками, а оновлюється рідко.
/// ReaderWriterLockSlim дозволяє десяткам потоків читати одночасно (ReadLock),
/// але на час запису (WriteLock) надає ексклюзивний доступ лише одному потоку.
/// </summary>
public sealed class CurrencyRatesCache : IDisposable
{
    private readonly ReaderWriterLockSlim _rwLock = new();
    private readonly Dictionary<string, decimal> _rates = new()
    {
        ["USD"] = 41.20m,
        ["EUR"] = 44.50m
    };

    public decimal GetRate(string currencyCode)
    {
        _rwLock.EnterReadLock();
        try
        {
            // Декілька потоків можуть перебувати тут одночасно!
            return _rates.TryGetValue(currencyCode, out decimal rate) ? rate : 0m;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    public void UpdateRate(string currencyCode, decimal newRate)
    {
        _rwLock.EnterWriteLock();
        try
        {
            // Ексклюзивна секція: жоден читач або інший письменник не може увійти сюди
            _rates[currencyCode] = newRate;
            Console.WriteLine($"  [WriteLock] Курс {currencyCode} оновлено до {newRate:F2} грн");
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    public void Dispose() => _rwLock.Dispose();
}

internal class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 03. ОБМЕЖЕННЯ ПАРАЛЕЛІЗМУ (SEMAPHORESLIM) ТА READERWRITERLOCKSLIM");
        Console.WriteLine(" Коли звичайного ексклюзивного lock недостатньо для високої пропускної здатності");
        Console.WriteLine("==========================================================================\n");

        // 1. SemaphoreSlim: пул із максимум 2 одночасних підключень
        Console.WriteLine("--- 1. SemaphoreSlim(2, 2): Пропускаємо не більше 2 завдань одночасно ---");
        using var semaphore = new SemaphoreSlim(initialCount: 2, maxCount: 2);

        var workers = new Task[5];
        for (int i = 1; i <= 5; i++)
        {
            int workerId = i;
            workers[i - 1] = Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    Console.WriteLine($"  [Завдання #{workerId}] Увійшло в пул (вільних слотів залишилось: {semaphore.CurrentCount})");
                    await Task.Delay(50);
                }
                finally
                {
                    semaphore.Release();
                    Console.WriteLine($"  [Завдання #{workerId}] Звільнило слот.");
                }
            });
        }

        await Task.WhenAll(workers);

        // 2. ReaderWriterLockSlim: багато читачів, один письменник
        Console.WriteLine("\n--- 2. ReaderWriterLockSlim: Паралельне читання та ексклюзивний запис ---");
        using var cache = new CurrencyRatesCache();

        Console.WriteLine($"  Початковий курс USD (ReadLock): {cache.GetRate("USD"):F2} грн");
        cache.UpdateRate("USD", 41.65m);
        Console.WriteLine($"  Новий курс USD (ReadLock):      {cache.GetRate("USD"):F2} грн");
    }
}
