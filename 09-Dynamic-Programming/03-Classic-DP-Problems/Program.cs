using System.Text;

namespace ClassicDpProblems;

/// <summary>
/// 03. Три найвідоміші класичні задачі Динамічного Програмування на життєвих прикладах:
/// 
/// 1) Кавомашина та решта монетами (Coin Change) — одновимірна табличка dp[сума].
/// 2) Туристичний рюкзак (0/1 Knapsack) — двовимірна табличка dp[предмет, вага].
/// 3) Виправлення одруківок у слові (Відстань Левенштейна) — двовимірна табличка dp[буква1, буква2].
/// </summary>
internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("================================================================================");
        Console.WriteLine("  03. КЛАСИЧНІ ЗАДАЧІ ДИНАМІЧНОГО ПРОГРАМУВАННЯ (З ВІЗУАЛІЗАЦІЄЮ ТАБЛИЦЬ)");
        Console.WriteLine("================================================================================\n");

        // 1. Кавомашина та решта монетами (Coin Change Problem)
        RunCoinChangeDemo();
        Console.WriteLine(new string('=', 80) + "\n");

        // 2. Туристичний рюкзак (0/1 Knapsack Problem)
        RunKnapsackDemo();
        Console.WriteLine(new string('=', 80) + "\n");

        // 3. Виправлення одруківок у слові (Відстань Левенштейна)
        RunLevenshteinDemo();
    }

    /// <summary>
    /// ЗАДАЧА 1: Кавомашина та решта монетами (Coin Change).
    /// Як видати решту 11 грн мінімальною кількістю монет номіналом 1, 2, 5 грн?
    /// </summary>
    private static void RunCoinChangeDemo()
    {
        Console.WriteLine("--- 1. КАВОМАШИНА ТА РЕШТА МОНЕТАМИ (Coin Change Problem) ---");
        Console.WriteLine("Життєва ситуація: Покупець замовив каву і кавомашині потрібно видати решту 11 грн");
        Console.WriteLine("найменшою кількістю монет. Доступні номінали монет: [1 грн, 2 грн, 5 грн].\n");

        int[] coins = { 1, 2, 5 };
        int targetAmount = 11;

        var (minCoins, usedCoins, dpTable) = CoinChangeSolver.SolveWithTable(coins, targetAmount);

        Console.WriteLine("Як мислить алгоритм (Табуляція від 0 грн до 11 грн):");
        Console.WriteLine("Для кожної суми S від 1 до 11 грн ми пробуємо відняти кожну доступну монету (1, 2 або 5)");
        Console.WriteLine("і дивимося, для якого залишку (S - монета) у таблиці dp[] вже записана найменша кількість монет!\n");

        // Виводимо наочну табличку dp[0..11]
        Console.Write("Сума (грн):     ");
        for (int s = 0; s <= targetAmount; s++) Console.Write($"{s,4} ");
        Console.WriteLine();

        Console.Write("Мін. монет dp:  ");
        for (int s = 0; s <= targetAmount; s++) Console.Write($"{dpTable[s],4} ");
        Console.WriteLine("\n");

        Console.WriteLine("Покрокове пояснення для кількох ключових сум:");
        Console.WriteLine("  * dp[0]  = 0 монет (решта 0 грн не потребує монет)");
        Console.WriteLine("  * dp[1]  = 1 монета (беремо [1 грн] + dp[0])");
        Console.WriteLine("  * dp[2]  = 1 монета (краще взяти одну [2 грн] + dp[0], ніж дві по 1 грн)");
        Console.WriteLine("  * dp[5]  = 1 монета (беремо одну [5 грн] + dp[0])");
        Console.WriteLine("  * dp[10] = 2 монети (беремо [5 грн] + dp[5] = 1 + 1 = 2 монети)");
        Console.WriteLine("  * dp[11] = 3 монети (беремо [1 грн] + dp[10] або [5 грн] + dp[6] = 3 монети)\n");

        Console.WriteLine($"=> РЕЗУЛЬТАТ ДЛЯ {targetAmount} ГРН:");
        Console.WriteLine($"   Мінімальна кількість монет: {minCoins} шт.");
        Console.WriteLine($"   Видані монети: [{string.Join(" грн, ", usedCoins)} грн]\n");
    }

    /// <summary>
    /// ЗАДАЧА 2: Туристичний рюкзак (0/1 Knapsack Problem).
    /// Які речі взяти в похід у рюкзак місткістю 7 кг, щоб їхня цінність була максимальною?
    /// </summary>
    private static void RunKnapsackDemo()
    {
        Console.WriteLine("--- 2. ТУРИСТИЧНИЙ РЮКЗАК (0/1 Knapsack Problem) ---");
        Console.WriteLine("Життєва ситуація: Ви йдете в гірський похід. Ваш рюкзак витримує максимум 7 кг.");
        Console.WriteLine("Кожну річ можна або ВЗЯТИ (1), або ЗАЛИШИТИ вдома (0). Як набрати максимум користі?\n");

        var items = new KnapsackItem[]
        {
            new("Намет",      Weight: 4, Value: 900),
            new("Спальник",   Weight: 3, Value: 700),
            new("Пальник",    Weight: 1, Value: 300),
            new("Фотокамера", Weight: 2, Value: 500)
        };
        int capacity = 7;

        Console.WriteLine("Доступні туристичні речі:");
        foreach (var item in items)
        {
            Console.WriteLine($"  - {item.Name,-12} | Вага: {item.Weight} кг | Цінність: {item.Value} балів");
        }
        Console.WriteLine();

        var (maxValue, packedItems, dpMatrix) = KnapsackSolver.SolveWithMatrix(items, capacity);

        Console.WriteLine("Таблиця Динамічного Програмування dp[річ, місткість рюкзака від 0 до 7 кг]:");
        Console.WriteLine("(На перетині рядка і стовпця — максимальна цінність для цієї ваги)\n");

        // Шапка таблиці
        Console.Write("Річ \\ Вага рюкзака     |");
        for (int w = 0; w <= capacity; w++) Console.Write($" {w,3}кг |");
        Console.WriteLine();
        Console.WriteLine(new string('-', 24 + (capacity + 1) * 8));

        // Рядок 0 (без речей)
        Console.Write($"{"0. (Порожньо)",-22} |");
        for (int w = 0; w <= capacity; w++) Console.Write($" {dpMatrix[0, w],5} |");
        Console.WriteLine();

        // Рядки для кожної речі
        for (int i = 1; i <= items.Length; i++)
        {
            var it = items[i - 1];
            string label = $"{i}. {it.Name} ({it.Weight}кг,{it.Value})";
            Console.Write($"{label,-22} |");
            for (int w = 0; w <= capacity; w++)
            {
                Console.Write($" {dpMatrix[i, w],5} |");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        int totalWeight = packedItems.Sum(x => x.Weight);
        Console.WriteLine($"=> РЕЗУЛЬТАТ ДЛЯ РЮКЗАКА ({capacity} КГ):");
        Console.WriteLine($"   Максимальна сумарна цінність: {maxValue} балів (зайнята вага: {totalWeight} із {capacity} кг)");
        Console.WriteLine("   Список обраних у похід речей:");
        foreach (var item in packedItems)
        {
            Console.WriteLine($"     [x] {item.Name} (Вага: {item.Weight} кг, Цінність: {item.Value} балів)");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// ЗАДАЧА 3: Виправлення одруківок у слові (Відстань Левенштейна / Edit Distance).
    /// Скільки букв треба замінити/вставити/видалити, щоб перетворити "КІТ" на "КОД"?
    /// </summary>
    private static void RunLevenshteinDemo()
    {
        Console.WriteLine("--- 3. ВИПРАВЛЕННЯ ОДРУКІВОК У СЛОВІ (Відстань Левенштейна) ---");
        Console.WriteLine("Життєва ситуація: Автокорекція на смартфоні або підказка Google \"Можливо, ви мали на увазі?\".");
        Console.WriteLine("За одну дію можна: 1) Замінити букву, 2) Вставити букву, 3) Видалити букву.\n");

        string word1 = "КІТ";
        string word2 = "КОД";

        var (distance, dpMatrix) = LevenshteinSolver.CalculateWithMatrix(word1, word2);

        Console.WriteLine($"Перетворюємо слово \"{word1}\" -> на слово \"{word2}\":\n");

        // Виводимо красиву матрицю перетворень
        Console.Write("      |   \"\" |");
        foreach (char c in word2) Console.Write($"  '{c}' |");
        Console.WriteLine();
        Console.WriteLine(new string('-', 14 + word2.Length * 7));

        for (int i = 0; i <= word1.Length; i++)
        {
            string rowChar = i == 0 ? "\"\"" : $"'{word1[i - 1]}'";
            Console.Write($"  {rowChar,3} |");
            for (int j = 0; j <= word2.Length; j++)
            {
                Console.Write($"   {dpMatrix[i, j],2} |");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        Console.WriteLine("Як читати цю таблицю по діагоналі:");
        Console.WriteLine("  1) Буква 'К' == 'К' -> співпадають! 0 дій (dp[1,1] = 0).");
        Console.WriteLine("  2) Буква 'І' != 'О' -> замінюємо 'І' на 'О' (+1 дія, dp[2,2] = 1).");
        Console.WriteLine("  3) Буква 'Т' != 'Д' -> замінюємо 'Т' на 'Д' (+1 дія, dp[3,3] = 2).\n");

        Console.WriteLine($"=> РЕЗУЛЬТАТ: Відстань Левенштейна між \"{word1}\" та \"{word2}\" дорівнює {distance} операціям.\n");
    }
}

public record KnapsackItem(string Name, int Weight, int Value);

/// <summary>
/// Розв'язувач задачі про решту монетами (Coin Change) з відновленням списку монет і таблиці DP.
/// </summary>
public static class CoinChangeSolver
{
    public static (int MinCoins, List<int> UsedCoins) Solve(int[] coins, int amount)
    {
        var (minCoins, usedCoins, _) = SolveWithTable(coins, amount);
        return (minCoins, usedCoins);
    }

    public static (int MinCoins, List<int> UsedCoins, int[] DpTable) SolveWithTable(int[] coins, int amount)
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
                    parent[i] = coin; // Запам'ятовуємо, яку монету додали останньою
                }
            }
        }

        if (dp[amount] > amount) return (-1, new List<int>(), dp);

        // Відновлення списку виданих монет
        var used = new List<int>();
        int current = amount;
        while (current > 0)
        {
            int coin = parent[current];
            used.Add(coin);
            current -= coin;
        }

        return (dp[amount], used, dp);
    }
}

/// <summary>
/// Розв'язувач задачі про туристичний рюкзак 0/1 (0/1 Knapsack).
/// </summary>
public static class KnapsackSolver
{
    public static (int MaxValue, List<KnapsackItem> SelectedItems) Solve(KnapsackItem[] items, int capacity)
    {
        var (maxValue, selected, _) = SolveWithMatrix(items, capacity);
        return (maxValue, selected);
    }

    public static (int MaxValue, List<KnapsackItem> SelectedItems, int[,] DpMatrix) SolveWithMatrix(KnapsackItem[] items, int capacity)
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
                    // Обираємо максимум: або не беремо річ, або беремо її + найкраще для залишку місця (w - Weight)
                    dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - item.Weight] + item.Value);
                }
                else
                {
                    // Річ заважка для поточного ліміту w — залишаємо попередній результат
                    dp[i, w] = dp[i - 1, w];
                }
            }
        }

        // Відновлення набору обраних предметів (йдемо з правого нижнього кута таблиці)
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

        selected.Reverse();
        return (dp[n, capacity], selected, dp);
    }
}

/// <summary>
/// Обчислення редакторської відстані Левенштейна (Levenshtein Distance).
/// </summary>
public static class LevenshteinSolver
{
    public static int Calculate(string s1, string s2)
    {
        return CalculateWithMatrix(s1, s2).Distance;
    }

    public static (int Distance, int[,] DpMatrix) CalculateWithMatrix(string s1, string s2)
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
                    dp[i, j] = dp[i - 1, j - 1]; // Букви однакові — нічого змінювати не треба
                }
                else
                {
                    dp[i, j] = 1 + Math.Min(
                        dp[i - 1, j],           // Видалення букви
                        Math.Min(dp[i, j - 1],  // Вставка букви
                        dp[i - 1, j - 1])       // Заміна букви
                    );
                }
            }
        }

        return (dp[m, n], dp);
    }
}
