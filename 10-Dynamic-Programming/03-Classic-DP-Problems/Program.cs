namespace ClassicDpProblems;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 03. Класичні задачі алгоритмічного динамічного програмування ===\n");

        // 1. Задача про розмін монет (Coin Change Problem)
        RunCoinChangeDemo();

        // 2. Задача про рюкзак (0/1 Knapsack Problem)
        RunKnapsackDemo();

        // 3. Відстань Левенштейна (Edit Distance)
        RunLevenshteinDemo();
    }

    private static void RunCoinChangeDemo()
    {
        Console.WriteLine("--- 1. Задача про розмін монет (Coin Change Problem) ---");
        int[] coins = { 1, 2, 5, 10, 25 };
        int amount = 63;

        var (minCoins, usedCoins) = CoinChangeSolver.Solve(coins, amount);

        Console.WriteLine($"Набір доступних монет: [{string.Join(", ", coins)}]");
        Console.WriteLine($"Цільова сума: {amount}");
        Console.WriteLine($"Мінімальна кількість монет: {minCoins}");
        Console.WriteLine($"Використані монети: [{string.Join(", ", usedCoins)}]\n");
    }

    private static void RunKnapsackDemo()
    {
        Console.WriteLine("--- 2. Задача про рюкзак (0/1 Knapsack Problem) ---");
        var items = new KnapsackItem[]
        {
            new("Ноутбук", Weight: 3, Value: 1200),
            new("Фотокамера", Weight: 2, Value: 700),
            new("Смартфон", Weight: 1, Value: 800),
            new("Книга", Weight: 1, Value: 150),
            new("Повербанк", Weight: 1, Value: 300)
        };
        int capacity = 5;

        var (maxValue, packedItems) = KnapsackSolver.Solve(items, capacity);

        Console.WriteLine($"Вантажопідйомність рюкзака: {capacity} кг");
        Console.WriteLine("Доступні предмети:");
        foreach (var item in items)
        {
            Console.WriteLine($"  - {item.Name}: вага {item.Weight} кг, вартість ${item.Value}");
        }

        Console.WriteLine($"\nМаксимальна цінність у рюкзаку: ${maxValue}");
        Console.WriteLine("Обрані предмети:");
        foreach (var item in packedItems)
        {
            Console.WriteLine($"  * {item.Name} (${item.Value}, {item.Weight} кг)");
        }
        Console.WriteLine();
    }

    private static void RunLevenshteinDemo()
    {
        Console.WriteLine("--- 3. Відстань Левенштейна (Levenshtein Distance) ---");
        string word1 = "algorithm";
        string word2 = "altruistic";

        int distance = LevenshteinSolver.Calculate(word1, word2);

        Console.WriteLine($"Рядок 1: \"{word1}\"");
        Console.WriteLine($"Рядок 2: \"{word2}\"");
        Console.WriteLine($"Мінімальна кількість операцій (вставка, видалення, заміна): {distance}\n");
    }
}

public record KnapsackItem(string Name, int Weight, int Value);

/// <summary>
/// Розв'язувач задачі розміну монет з відновленням відповіді.
/// </summary>
public static class CoinChangeSolver
{
    public static (int MinCoins, List<int> UsedCoins) Solve(int[] coins, int amount)
    {
        int[] dp = new int[amount + 1];
        int[] parent = new int[amount + 1];

        Array.Fill(dp, amount + 1);
        Array.Fill(parent, -1);
        dp[0] = 0;

        for (int i = 1; i <= amount; i++)
        {
            foreach (int coin in coins)
            {
                if (coin <= i && dp[i - coin] + 1 < dp[i])
                {
                    dp[i] = dp[i - coin] + 1;
                    parent[i] = coin; // Запам'ятовуємо, яка монета привела до оптимуму
                }
            }
        }

        if (dp[amount] > amount) return (-1, new List<int>());

        // Відновлення шляху (яких саме монет взяли)
        var used = new List<int>();
        int current = amount;
        while (current > 0)
        {
            int coin = parent[current];
            used.Add(coin);
            current -= coin;
        }

        return (dp[amount], used);
    }
}

/// <summary>
/// Розв'язувач задачі про рюкзак 0/1.
/// </summary>
public static class KnapsackSolver
{
    public static (int MaxValue, List<KnapsackItem> SelectedItems) Solve(KnapsackItem[] items, int capacity)
    {
        int n = items.Length;
        int[,] dp = new int[n + 1, capacity + 1];

        // Заповнення таблиці підзадач
        for (int i = 1; i <= n; i++)
        {
            var item = items[i - 1];
            for (int w = 0; w <= capacity; w++)
            {
                if (item.Weight <= w)
                {
                    dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - item.Weight] + item.Value);
                }
                else
                {
                    dp[i, w] = dp[i - 1, w];
                }
            }
        }

        // Відновлення набору обраних предметів
        var selected = new List<KnapsackItem>();
        int currW = capacity;
        for (int i = n; i > 0 && currW > 0; i--)
        {
            if (dp[i, currW] != dp[i - 1, currW])
            {
                var item = items[i - 1];
                selected.Add(item);
                currW -= item.Weight;
            }
        }

        return (dp[n, capacity], selected);
    }
}

/// <summary>
/// Обчислення редакторської відстані Левенштейна.
/// </summary>
public static class LevenshteinSolver
{
    public static int Calculate(string s1, string s2)
    {
        int m = s1.Length;
        int n = s2.Length;
        int[,] dp = new int[m + 1, n + 1];

        for (int i = 0; i <= m; i++) dp[i, 0] = i;
        for (int j = 0; j <= n; j++) dp[0, j] = j;

        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                if (s1[i - 1] == s2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1]; // Символи збігаються, операція не потрібна
                }
                else
                {
                    dp[i, j] = 1 + Math.Min(
                        dp[i - 1, j],           // Видалення
                        Math.Min(dp[i, j - 1],  // Вставка
                        dp[i - 1, j - 1])       // Заміна
                    );
                }
            }
        }

        return dp[m, n];
    }
}
