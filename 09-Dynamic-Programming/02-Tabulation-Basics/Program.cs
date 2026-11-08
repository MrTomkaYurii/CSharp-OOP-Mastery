using System.Text;

namespace TabulationBasics;

/// <summary>
/// 02. Табуляція (Bottom-Up DP = Заповнення таблички знизу-вгору у звичайному циклі).
/// 
/// Чим Табуляція відрізняється від Мемоізації?
/// - У Мемоізації (Top-Down) ми починали з великої задачі N і рекурсивно спускалися вниз до 0.
/// - У Табуляції (Bottom-Up) ми ВЗАГАЛІ НЕ ВИКОРИСТОВУЄМО РЕКУРСІЮ!
///   Ми створюємо масив (табличку) dp[] і заповнюємо його в циклі for від найпростіших клітинок
///   dp[0], dp[1], dp[2]... аж до dp[N].
/// 
/// Переваги Табуляції:
/// 1) Жодного ризику переповнення стека (StackOverflowException), бо немає рекурсії.
/// 2) Часто масив dp[] можна замінити всього на 2 звичайні змінні, зменшивши витрати пам'яті до O(1)!
/// </summary>
internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("================================================================================");
        Console.WriteLine("  02. ОСНОВИ ТАБУЛЯЦІЇ (BOTTOM-UP DP = ЗАПОВНЕННЯ ТАБЛИЧКИ В ЦИКЛІ БЕЗ РЕКУРСІЇ)");
        Console.WriteLine("================================================================================\n");

        RunStaircaseTabulationDemo();
        Console.WriteLine(new string('-', 80) + "\n");
        RunRobotCoinCollectionDemo();
        Console.WriteLine(new string('-', 80) + "\n");
        RunSpaceOptimizationDemo();
    }

    /// <summary>
    /// ДЕМО 1: Покрокове заповнення масиву dp[] у циклі для задачі про сходинки.
    /// </summary>
    private static void RunStaircaseTabulationDemo()
    {
        Console.WriteLine("--- ПРИКЛАД 1: Підйом по сходинках через табличку (масив dp[]) ---");
        int n = 6;
        Console.WriteLine($"Мета: Знайти кількість способів піднятися на {n}-ту сходинку (кроки по 1 або 2).\n");

        long[] dp = new long[n + 1];

        // Базові стани (найменші підзадачі, відповідь на які ми знаємо одразу)
        dp[0] = 1; // Стоїмо на підлозі перед сходами (1 стартовий стан)
        dp[1] = 1; // На 1-шу сходинку є лише 1 спосіб: зробити крок [1]

        Console.WriteLine("Крок 0 (База): dp[0] = 1 (старт), dp[1] = 1 (перша сходинка)");
        PrintDpArray(dp, highlightedIndex: 1);

        // Ітеративно заповнюємо табличку зліва направо від 2 до N
        for (int i = 2; i <= n; i++)
        {
            dp[i] = dp[i - 1] + dp[i - 2];
            Console.WriteLine($"\nКрок {i - 1}: Рахуємо dp[{i}] = dp[{i - 1}] ({dp[i - 1]}) + dp[{i - 2}] ({dp[i - 2]}) = {dp[i]}");
            PrintDpArray(dp, highlightedIndex: i);
        }

        Console.WriteLine($"\n=> Готово! Відповідь лежить в останній клітинці dp[{n}] = {dp[n]} способів.\n");
    }

    /// <summary>
    /// ДЕМО 2: Задача "Робот-збирач монет" (або "Пограбування будинків" / House Robber).
    /// Умова: Уздовж коридору лежать купки монет. Робот іде вперед і може забирати монети,
    /// але сигналізація спрацює, якщо взяти монети з ДВОХ СУСІДНІХ клітинок підряд!
    /// Питання: Яку максимальну суму монет може зібрати робот?
    /// 
    /// Логіка переходу для клітинки i:
    /// У нас є 2 вибори:
    ///   Варіант А (НЕ беремо монети на клітинці i): тоді наш максимум такий самий, як на попередній клітинці -> dp[i - 1].
    ///   Варіант Б (БЕРЕМО монети на клітинці i): тоді попередню клітинку (i - 1) брати не можна,
    ///              отже додаємо монети клітинки i до найкращого результату з клітинки (i - 2) -> dp[i - 2] + coins[i].
    /// Обираємо кращий із двох варіантів:
    ///   dp[i] = Math.Max(dp[i - 1], dp[i - 2] + coins[i])
    /// </summary>
    private static void RunRobotCoinCollectionDemo()
    {
        Console.WriteLine("--- ПРИКЛАД 2: Робот-збирач монет (не можна брати дві сусідні клітинки) ---");
        int[] coins = { 3, 10, 4, 2, 12, 6 };
        Console.WriteLine($"Монети в клітинках: [{string.Join(", ", coins)}]");
        Console.WriteLine("Правило: Якщо беремо монети з клітинки i, то клітинку (i-1) треба пропустити.");
        Console.WriteLine("Формула: dp[i] = Max( пропустити: dp[i-1],  взяти: dp[i-2] + coins[i] )\n");

        int n = coins.Length;
        int[] dp = new int[n + 1];

        dp[0] = 0;        // 0 клітинок -> 0 монет
        dp[1] = coins[0]; // З першої клітинки вигідно взяти її монети (3)

        Console.WriteLine($"Клітинка 1 (монет: {coins[0],2}): беремо одразу -> dp[1] = {dp[1]}");

        for (int i = 2; i <= n; i++)
        {
            int currentCoin = coins[i - 1];
            int skipOption = dp[i - 1];
            int takeOption = dp[i - 2] + currentCoin;

            dp[i] = Math.Max(skipOption, takeOption);

            string decision = takeOption >= skipOption
                ? $"БЕРЕМО ({dp[i - 2]} + {currentCoin} = {takeOption} >= {skipOption})"
                : $"ПРОПУСКАЄМО (залишаємо {skipOption} > {takeOption})";

            Console.WriteLine($"Клітинка {i} (монет: {currentCoin,2}): Max(пропустити={skipOption}, взяти={dp[i - 2]}+{currentCoin}={takeOption}) -> dp[{i}] = {dp[i],2} [{decision}]");
        }

        Console.WriteLine($"\n=> Максимальна зібрана сума монет: {dp[n]} (взято купки: 10 + 12 = 22).\n");
    }

    /// <summary>
    /// ДЕМО 3: Як позбутися масиву dp[] і залишити лише 2 змінні (O(1) пам'яті).
    /// </summary>
    private static void RunSpaceOptimizationDemo()
    {
        Console.WriteLine("--- ПРИКЛАД 3: Оптимізація пам'яті з O(N) до O(1) (Лише 2 змінні!) ---");
        Console.WriteLine("Подивіться уважно на формулу: dp[i] = dp[i - 1] + dp[i - 2].");
        Console.WriteLine("Щоб порахувати нову клітинку, нам потрібні ЛИШЕ ДВІ попередні клітинки!");
        Console.WriteLine("Усі старіші клітинки (dp[0], dp[1]...) нам більше ніколи не знадобляться.\n");

        int n = 50;
        long resultWithArray = FibonacciTable(n);
        long resultWithTwoVars = FibonacciSpaceOptimized(n);

        Console.WriteLine($"Обчислення Fib({n}):");
        Console.WriteLine($"  1) З масивом long[{n + 1}] (пам'ять O(N)):      {resultWithArray:N0}");
        Console.WriteLine($"  2) З двома змінними prev1, prev2 (пам'ять O(1)): {resultWithTwoVars:N0}\n");

        Console.WriteLine("--- Підсумкове порівняння: Мемоізація vs Табуляція ---");
        Console.WriteLine("| Критерій             | Мемоізація (Top-Down)       | Табуляція (Bottom-Up)      |");
        Console.WriteLine("|----------------------|-----------------------------|----------------------------|");
        Console.WriteLine("| Як мислимо           | Згори-вниз (від N до 0)     | Знизу-вгору (від 0 до N)   |");
        Console.WriteLine("| Інструмент у коді    | Рекурсія + Словник/Кеш      | Звичайний цикл for + масив |");
        Console.WriteLine("| Стек викликів        | Ризик StackOverflowException| 0 (безпечно для будь-яких N)|");
        Console.WriteLine("| Оптимізація пам'яті  | Завжди O(N) на словник      | Легко стискається до O(1)  |");
    }

    private static void PrintDpArray(long[] dp, int highlightedIndex)
    {
        Console.Write("   Індекси: ");
        for (int i = 0; i < dp.Length; i++)
        {
            Console.Write($"  [{i}] ");
        }
        Console.WriteLine();

        Console.Write("   Масив dp:");
        for (int i = 0; i < dp.Length; i++)
        {
            if (i == highlightedIndex)
                Console.Write($" *{dp[i],2}* ");
            else if (i < highlightedIndex)
                Console.Write($"  {dp[i],2}  ");
            else
                Console.Write("   .  ");
        }
        Console.WriteLine();
    }

    public static long FibonacciTable(int n)
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

    public static long FibonacciSpaceOptimized(int n)
    {
        if (n <= 1) return n;

        long prev2 = 0; // Позаминуле значення: dp[i - 2]
        long prev1 = 1; // Минуле значення:     dp[i - 1]
        long current = 0;

        for (int i = 2; i <= n; i++)
        {
            current = prev1 + prev2;
            prev2 = prev1;   // Зсуваємо вікно вперед
            prev1 = current;
        }

        return current;
    }
}
