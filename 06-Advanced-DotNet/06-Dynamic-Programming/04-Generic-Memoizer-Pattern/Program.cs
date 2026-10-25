using System.Collections.Concurrent;
using System.Diagnostics;

namespace GenericMemoizerPattern;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 04. Патерн Generic Memoizer у мові C# ===\n");

        // 1. Мемоізація важкої бізнес-функції (імітація розрахунку вартості доставки)
        Console.WriteLine("--- 1. Мемоізація бізнес-обчислення (двовимірний вхід) ---");
        Func<string, int, decimal> calculateDelivery = (zone, weight) =>
        {
            // Імітація важкої операції (запит до геосервісу, тарифікація)
            Thread.Sleep(200);
            decimal baseRate = zone switch
            {
                "EU-West" => 25.0m,
                "EU-East" => 18.0m,
                "US-East" => 45.0m,
                _ => 30.0m
            };
            return baseRate + (weight * 3.5m);
        };

        var memoizedDelivery = calculateDelivery.Memoize();

        TestDeliveryCall(memoizedDelivery, "EU-West", 10, "Перший виклик (холодний)");
        TestDeliveryCall(memoizedDelivery, "EU-West", 10, "Другий виклик з тими ж аргументами (гарячий кеш)");
        TestDeliveryCall(memoizedDelivery, "US-East", 5,  "Новий виклик (інші аргументи)");
        TestDeliveryCall(memoizedDelivery, "EU-West", 10, "Повторний виклик першого запиту (гарячий кеш)\n");

        // 2. Рекурсивна мемоізація (Fixed-point / Recursive Memoizer)
        Console.WriteLine("--- 2. Універсальна рекурсивна мемоізація (Фібоначчі) ---");
        // Оголошуємо функцію вищого порядку, яка приймає посилання на саму себе
        Func<Func<int, long>, Func<int, long>> fibonacciGenerator = recurse => n =>
        {
            if (n <= 1) return n;
            return recurse(n - 1) + recurse(n - 2);
        };

        var fastFib = fibonacciGenerator.MemoizeRecursive();

        var sw = Stopwatch.StartNew();
        long fibResult = fastFib(50);
        sw.Stop();

        Console.WriteLine($"Результат fastFib(50) = {fibResult}");
        Console.WriteLine($"Час виконання з рекурсивним кешуванням: {sw.Elapsed.TotalMicroseconds:F2} μs\n");
    }

    private static void TestDeliveryCall(Func<string, int, decimal> func, string zone, int weight, string scenario)
    {
        var sw = Stopwatch.StartNew();
        decimal cost = func(zone, weight);
        sw.Stop();
        Console.WriteLine($"[{scenario}]");
        Console.WriteLine($"  Вхід: Зона={zone}, Вага={weight} кг -> Вартість: ${cost}");
        Console.WriteLine($"  Час:  {sw.ElapsedMilliseconds} ms ({sw.Elapsed.TotalMicroseconds:F0} μs)");
    }
}

/// <summary>
/// Бібліотека методів розширення для функціональної мемоізації.
/// </summary>
public static class MemoizerExtensions
{
    /// <summary>
    /// Мемоізує одномісну чисту функцію за допомогою ConcurrentDictionary.
    /// </summary>
    public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func) where T : notnull
    {
        var cache = new ConcurrentDictionary<T, TResult>();
        return arg => cache.GetOrAdd(arg, func);
    }

    /// <summary>
    /// Мемоізує двомісну чисту функцію за допомогою кортежу як ключа.
    /// </summary>
    public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        where T1 : notnull
        where T2 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2), TResult>();
        return (arg1, arg2) => cache.GetOrAdd((arg1, arg2), key => func(key.Item1, key.Item2));
    }

    /// <summary>
    /// Рекурсивний мемоізатор для функцій, які викликають самі себе.
    /// Дозволяє автоматично кешувати проміжні рекурсивні виклики.
    /// </summary>
    public static Func<T, TResult> MemoizeRecursive<T, TResult>(
        this Func<Func<T, TResult>, Func<T, TResult>> generator) where T : notnull
    {
        var cache = new ConcurrentDictionary<T, TResult>();
        Func<T, TResult> self = null!;
        self = arg => cache.GetOrAdd(arg, generator(x => self(x)));
        return self;
    }
}
