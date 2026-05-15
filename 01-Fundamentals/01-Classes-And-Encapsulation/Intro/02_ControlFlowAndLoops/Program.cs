// ============================================================================
//  INTRO 02 — Розгалуження та цикли   (з C++ на C#)
// ----------------------------------------------------------------------------
//  Що звично, що ні:
//   * if / while / for / do-while / break / continue — синтаксично як у C++.
//   * Умова МАЄ бути bool: if (x)  де x — int, не компілюється.
//   * switch НЕ "провалюється" крізь case — кожна гілка має завершитись
//     (break / return / throw / goto case).
//   * Новинки без аналога в класичному C++: switch-вираз, зіставлення зі
//     зразком (pattern matching), foreach, оператори діапазону  ^  та  ..
// ============================================================================

namespace Intro;

internal static class Program
{
    private static void Main()
    {
        Section("1. if / else / потрійний оператор");
        BranchingDemo();

        Section("2. switch — інструкція");
        SwitchStatementDemo();

        Section("3. switch — вираз і зіставлення зі зразком");
        SwitchExpressionDemo();

        Section("4. Цикли: for / while / do-while");
        LoopsDemo();

        Section("5. foreach");
        ForeachDemo();

        Section("6. Індекси ^ та діапазони ..");
        RangesDemo();
    }

    // ------------------------------------------------------------------------
    private static void BranchingDemo()
    {
        int score = 73;

        if (score >= 90) Console.WriteLine("відмінно");
        else if (score >= 60) Console.WriteLine("задовільно");
        else Console.WriteLine("не склав");

        // Потрійний оператор — вираз (повертає значення):
        string result = score >= 60 ? "pass" : "fail";
        Console.WriteLine($"result = {result}");

        // ?? — значення за замовчуванням для null; ?. — безпечний доступ (не кине NullReference).
        string? input = null;
        Console.WriteLine($"input ?? \"—\" -> {input ?? "—"}");
        Console.WriteLine($"input?.Length -> {input?.Length.ToString() ?? "null"}");
    }

    // ------------------------------------------------------------------------
    private static void SwitchStatementDemo()
    {
        for (int day = 1; day <= 8; day++)
        {
            switch (day)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    Console.WriteLine($"день {day}: робочий");
                    break;              // "провалювання" заборонене — break обов'язковий
                case 6:
                case 7:
                    Console.WriteLine($"день {day}: вихідний");
                    break;
                default:
                    Console.WriteLine($"день {day}: такого немає");
                    break;
            }
        }

        // switch працює і по рядках (у C++ так не можна):
        string cmd = "start";
        switch (cmd)
        {
            case "start": Console.WriteLine("запуск..."); break;
            case "stop": Console.WriteLine("зупинка..."); break;
            default: Console.WriteLine("невідома команда"); break;
        }
    }

    // ------------------------------------------------------------------------
    private static void SwitchExpressionDemo()
    {
        // switch-ВИРАЗ: коротко повертає значення. => замість case/break, _ замість default.
        for (int n = 0; n <= 5; n++)
        {
            string word = n switch
            {
                0 => "нуль",
                1 => "один",
                2 or 3 => "трохи",
                >= 4 => "багато",         // реляційний зразок
                _ => "?"
            };
            Console.WriteLine($"{n} -> {word}");
        }

        // Зіставлення зі зразком за типом + when-обмеження:
        object[] values = { 42, "текст", 3.14, true, null! };
        foreach (object? v in values)
        {
            string desc = v switch
            {
                int x when x > 10 => $"велике ціле {x}",
                int x => $"ціле {x}",
                string s => $"рядок довжини {s.Length}",
                double => "дробове",
                null => "нічого (null)",
                _ => $"інше: {v!.GetType().Name}"
            };
            Console.WriteLine(desc);
        }

        // Кортежний зразок — таблиця рішень у два рядки:
        Console.WriteLine(RockPaperScissors("камінь", "ножиці"));
        Console.WriteLine(RockPaperScissors("папір", "папір"));
    }

    private static string RockPaperScissors(string a, string b) => (a, b) switch
    {
        _ when a == b => "нічия",
        ("камінь", "ножиці") or ("ножиці", "папір") or ("папір", "камінь") => "перемагає перший",
        _ => "перемагає другий"
    };

    // ------------------------------------------------------------------------
    private static void LoopsDemo()
    {
        // Класичний for
        for (int i = 0; i < 5; i++)
            Console.Write(i + " ");
        Console.WriteLine();

        // for з кількома змінними
        for (int lo = 0, hi = 10; lo < hi; lo++, hi--)
            Console.Write($"({lo},{hi}) ");
        Console.WriteLine();

        // while з continue/break
        int sum = 0, k = 0;
        while (true)
        {
            k++;
            if (k % 2 == 0) continue;   // пропускаємо парні
            if (k > 9) break;
            sum += k;
        }
        Console.WriteLine($"сума непарних 1..9 = {sum}");

        // do-while — тіло виконується щонайменше раз
        int countdown = 3;
        do { Console.Write($"{countdown} "); } while (--countdown > 0);
        Console.WriteLine("поїхали!");
    }

    // ------------------------------------------------------------------------
    private static void ForeachDemo()
    {
        // foreach = range-based for. Працює з масивами, списками, рядками, словниками...
        int[] numbers = { 5, 8, 13, 21 };
        int total = 0;
        foreach (int x in numbers) total += x;
        Console.WriteLine($"сума масиву = {total}");

        foreach (char c in "C#")
            Console.Write($"[{c}]");
        Console.WriteLine();

        var ages = new Dictionary<string, int> { ["Аня"] = 20, ["Богдан"] = 22 };
        foreach ((string name, int age) in ages)          // деконструкція пари
            Console.WriteLine($"{name}: {age}");

        // Індекс у foreach: додаємо .Select((значення, індекс))
        foreach (var (value, index) in numbers.Select((v, i) => (v, i)))
            Console.WriteLine($"numbers[{index}] = {value}");
    }

    // ------------------------------------------------------------------------
    private static void RangesDemo()
    {
        int[] a = { 10, 20, 30, 40, 50, 60 };

        Console.WriteLine($"a.Length = {a.Length}");        // масив знає свій розмір
        Console.WriteLine($"a[^1] = {a[^1]} (останній)");   // індекс з кінця
        Console.WriteLine($"a[^2] = {a[^2]} (передостанній)");

        int[] middle = a[1..4];      // елементи з 1 по 3 (кінець не включається)
        int[] firstTwo = a[..2];     // з початку
        int[] lastTwo = a[^2..];     // до кінця
        Console.WriteLine($"a[1..4] = [{string.Join(", ", middle)}]");
        Console.WriteLine($"a[..2]  = [{string.Join(", ", firstTwo)}]");
        Console.WriteLine($"a[^2..] = [{string.Join(", ", lastTwo)}]");

        Console.WriteLine($"'Hello, World'[7..] = {"Hello, World"[7..]}");

        // Enumerable.Range — зручний генератор послідовності для foreach:
        foreach (int i in Enumerable.Range(1, 5))
            Console.Write($"{i * i} ");
        Console.WriteLine();
    }

    // ------------------------------------------------------------------------
    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ".PadRight(70, '='));
    }
}
