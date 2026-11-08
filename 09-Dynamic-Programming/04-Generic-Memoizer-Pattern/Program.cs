using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace GenericMemoizerPattern;

/// <summary>
/// 04. Патерн Generic Memoizer у мові C# для студентів.
/// 
/// Мета прикладу:
/// Показати, як перетворити будь-яку звичайну функцію Func<T, TResult> на функцію з кешем
/// за допомогою одного простого методу розширення .Memoize(), без повторення коду словника!
/// </summary>
internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine("  04. ПАТЕРН GENERIC MEMOIZER (АВТОМАТИЧНИЙ КЕШ ДЛЯ ФУНКЦІЙ)");
        Console.WriteLine("================================================================================\n");

        // 1. Мемоізація важкої бізнес-функції розрахунку тарифу доставки
        Console.WriteLine("--- 1. Мемоізація бізнес-функції (два аргументи: зона і вага) ---");
        
        Func<string, int, decimal> calculateDelivery = (zone, weight) =>
        {
            // Імітація важкої операції (запит до геосервісу, тарифікація)
            Thread.Sleep(150);
            decimal baseRate = zone switch
            {
                "EU-West" => 25.0m,
                "EU-East" => 18.0m,
                "US-East" => 45.0m,
                _ => 30.0m
            };
            return baseRate + (weight * 3.5m);
        };

        // Завдяки методу розширення .Memoize() ми огортаємо функцію автоматичним кешем:
        var memoizedDelivery = calculateDelivery.Memoize();

        TestDeliveryCall(memoizedDelivery, "EU-West", 10, "Перший виклик (холодний — обчислення)");
        TestDeliveryCall(memoizedDelivery, "EU-West", 10, "Другий виклик з тими ж аргументами (гарячий — миттєво з кешу)");
        TestDeliveryCall(memoizedDelivery, "US-East", 5,  "Новий виклик з іншими аргументами (обчислення)");
        TestDeliveryCall(memoizedDelivery, "EU-West", 10, "Повторний виклик першого запиту (гарячий — з кешу)\n");

        // 2. Зрозуміла рекурсивна мемоізація (числа Фібоначчі)
        Console.WriteLine("--- 2. Рекурсивна мемоізація (Фібоначчі без зайвого ускладнення) ---");
        Console.WriteLine("Замість складних лямбд-генераторів, рекурсивний кеш створюється через");
        Console.WriteLine("замикання локальної функції з ConcurrentDictionary:\n");

        var fastFib = MemoizerExtensions.CreateRecursiveMemoizedFibonacci();

        var sw = Stopwatch.StartNew();
        long fibResult = fastFib(50);
        sw.Stop();

        Console.WriteLine($"Результат fastFib(50) = {fibResult}");
        Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMicroseconds:F1} мкс (миттєво завдяки кешу!)\n");
    }

    private static void TestDeliveryCall(Func<string, int, decimal> func, string zone, int weight, string scenario)
    {
        var sw = Stopwatch.StartNew();
        decimal cost = func(zone, weight);
        sw.Stop();
        Console.WriteLine($"[{scenario}]");
        Console.WriteLine($"  Вхід: Зона={zone}, Вага={weight} кг -> Вартість: ${cost}");
        Console.WriteLine($"  Час:  {sw.ElapsedMilliseconds} мс ({sw.Elapsed.TotalMicroseconds:F0} мкс)");
    }
}

/// <summary>
/// Прості та наочні методи розширення для мемоізації функцій у C#.
/// </summary>
public static class MemoizerExtensions
{
    /// <summary>
    /// Мемоізує функцію з одним аргументом за допомогою ConcurrentDictionary.
    /// </summary>
    public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func) where T : notnull
    {
        var cache = new ConcurrentDictionary<T, TResult>();
        return arg => cache.GetOrAdd(arg, func);
    }

    /// <summary>
    /// Мемоізує функцію з двома аргументами, використовуючи кортеж (T1, T2) як ключ словника.
    /// </summary>
    public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        where T1 : notnull
        where T2 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2), TResult>();
        return (arg1, arg2) => cache.GetOrAdd((arg1, arg2), key => func(key.Item1, key.Item2));
    }

    /// <summary>
    /// Наочний приклад рекурсивної мемоізації для студентів:
    /// Створює функцію розрахунку Фібоначчі, в якій локальний кеш автоматично зберігає
    /// результати всіх проміжних підзадач.
    /// </summary>
    public static Func<int, long> CreateRecursiveMemoizedFibonacci()
    {
        var cache = new ConcurrentDictionary<int, long>();

        long Fib(int n)
        {
            if (n <= 1) return n;
            return cache.GetOrAdd(n, num => Fib(num - 1) + Fib(num - 2));
        }

        return Fib;
    }
}
