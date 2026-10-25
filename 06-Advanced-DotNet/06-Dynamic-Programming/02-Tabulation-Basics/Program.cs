using System.Diagnostics;

namespace TabulationBasics;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 02. Основи алгоритмічної табуляції (Bottom-Up Approach) ===\n");

        int n = 80;
        Console.WriteLine($"Обчислення великого значення Fib({n}) без ризику переповнення стека:\n");

        // 1. Табуляція з масивом: O(n) час, O(n) пам'ять
        Console.WriteLine("1. Табуляція з використанням повної DP-таблиці (масиву)...");
        var sw = Stopwatch.StartNew();
        long tableResult = FibonacciTable(n);
        sw.Stop();
        Console.WriteLine($"   Результат:     Fib({n}) = {tableResult}");
        Console.WriteLine($"   Час виконання: {sw.Elapsed.TotalMicroseconds:F2} μs");
        Console.WriteLine($"   Використання пам'яті: масив на {n + 1} елементів (O(n))\n");

        // 2. Просторова оптимізація: O(n) час, O(1) пам'ять
        Console.WriteLine("2. Табуляція з оптимізацією пам'яті (дві змінні)...");
        sw.Restart();
        long spaceOptimizedResult = FibonacciSpaceOptimized(n);
        sw.Stop();
        Console.WriteLine($"   Результат:     Fib({n}) = {spaceOptimizedResult}");
        Console.WriteLine($"   Час виконання: {sw.Elapsed.TotalMicroseconds:F2} μs");
        Console.WriteLine("   Використання пам'яті: 2 локальні змінні (O(1))\n");

        // 3. Порівняння Top-Down vs Bottom-Up
        Console.WriteLine("--- Порівняння підходів динамічного програмування ---");
        Console.WriteLine("| Критерій             | Мемоізація (Top-Down)       | Табуляція (Bottom-Up)      |");
        Console.WriteLine("|----------------------|-----------------------------|----------------------------|");
        Console.WriteLine("| Порядок обчислення   | Від кінця до базових станів | Від базових станів до кінця|");
        Console.WriteLine("| Реалізація           | Рекурсія + Словник/Кеш      | Ітеративний цикл (for)     |");
        Console.WriteLine("| Витрати на стек      | Ризик StackOverflowException| 0 (без викликів стека)     |");
        Console.WriteLine("| Оптимізація пам'яті  | Складно (потрібен кеш)      | Легко (зведення до O(1))   |");
    }

    /// <summary>
    /// Класична табуляція (Bottom-Up).
    /// Заповнює таблицю результатів послідовно від найменших підзадач до n.
    /// Часова складність: O(n). Просторова складність: O(n).
    /// </summary>
    private static long FibonacciTable(int n)
    {
        if (n <= 1) return n;

        long[] dp = new long[n + 1];
        dp[0] = 0;
        dp[1] = 1;

        for (int i = 2; i <= n; i++)
        {
            dp[i] = dp[i - 1] + dp[i - 2];
        }

        return dp[n];
    }

    /// <summary>
    /// Табуляція з просторовою оптимізацією (Space-Optimized Bottom-Up).
    /// Оскільки для обчислення fib(n) потрібні лише два попередні числа,
    /// немає потреби зберігати весь масив — достатньо двох змінних стану.
    /// Часова складність: O(n). Просторова складність: O(1).
    /// </summary>
    private static long FibonacciSpaceOptimized(int n)
    {
        if (n <= 1) return n;

        long prev2 = 0; // fib(i - 2)
        long prev1 = 1; // fib(i - 1)
        long current = 0;

        for (int i = 2; i <= n; i++)
        {
            current = prev1 + prev2;
            prev2 = prev1;
            prev1 = current;
        }

        return current;
    }
}
