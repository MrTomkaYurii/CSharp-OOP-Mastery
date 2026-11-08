using System.Diagnostics;

namespace MemoizationBasics;

internal class Program
{
    private static long _naiveCallCount = 0;
    private static long _memoizedCallCount = 0;

    static void Main()
    {
        Console.WriteLine("=== 01. Основи алгоритмічної мемоізації (Top-Down Approach) ===\n");

        int n = 40;
        Console.WriteLine($"Обчислення {n}-го числа Фібоначчі:\n");

        // 1. Наївна рекурсія: O(2^n) час
        Console.WriteLine("1. Запуск наївної рекурсії...");
        var sw = Stopwatch.StartNew();
        long naiveResult = NaiveFibonacci(n);
        sw.Stop();
        long naiveTimeMs = sw.ElapsedMilliseconds;
        Console.WriteLine($"   Результат:      Fib({n}) = {naiveResult}");
        Console.WriteLine($"   Час виконання:  {naiveTimeMs} ms");
        Console.WriteLine($"   Кількість викликів: {_naiveCallCount:N0}\n");

        // 2. Мемоізована рекурсія: O(n) час, O(n) пам'ять
        Console.WriteLine("2. Запуск рекурсії з мемоізацією (кешем)...");
        var memo = new Dictionary<int, long>();
        sw.Restart();
        long memoResult = MemoizedFibonacci(n, memo);
        sw.Stop();
        long memoTimeMs = sw.ElapsedMilliseconds;
        Console.WriteLine($"   Результат:      Fib({n}) = {memoResult}");
        Console.WriteLine($"   Час виконання:  {memoTimeMs} ms");
        Console.WriteLine($"   Кількість викликів: {_memoizedCallCount:N0}\n");

        // 3. Порівняння результатів
        Console.WriteLine("--- Підсумок ефективності мемоізації ---");
        Console.WriteLine($"Прискорення за викликами: у {(_naiveCallCount / (double)_memoizedCallCount):N0} разів менше операцій!");
        Console.WriteLine("Висновок: збереження результатів перекривних підзадач усуває експоненційне зростання дерева викликів.");
    }

    /// <summary>
    /// Наївна рекурсія. Обчислює кожне піддерево заново, породжуючи O(2^n) викликів.
    /// </summary>
    private static long NaiveFibonacci(int n)
    {
        _naiveCallCount++;
        if (n <= 1) return n;
        return NaiveFibonacci(n - 1) + NaiveFibonacci(n - 2);
    }

    /// <summary>
    /// Мемоізована рекурсія (Top-Down).
    /// Перед обчисленням перевіряє наявність результату в словнику-кеші.
    /// </summary>
    private static long MemoizedFibonacci(int n, Dictionary<int, long> memo)
    {
        _memoizedCallCount++;
        if (n <= 1) return n;

        // Якщо підзадача вже розв'язана — повертаємо готове значення з кешу
        if (memo.TryGetValue(n, out long cachedValue))
        {
            return cachedValue;
        }

        // Інакше розв'язуємо підзадачу та зберігаємо результат у кеші
        long result = MemoizedFibonacci(n - 1, memo) + MemoizedFibonacci(n - 2, memo);
        memo[n] = result;
        return result;
    }
}
