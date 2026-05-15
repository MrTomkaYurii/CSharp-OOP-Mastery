// ============================================================================
//  INTRO 01 — Консоль і типи   (швидкий стрибок з C++ на C#)
// ----------------------------------------------------------------------------
//  Головні відмінності від C++, які видно вже тут:
//   * Немає заголовкових файлів (.h) і forward-декларацій. Порядок методів
//     у файлі не має значення — компілятор бачить увесь клас одразу.
//   * Точка входу — метод Main усередині класу. Весь код живе в класах.
//   * Замість <iostream> — статичний клас Console (простір імен System).
//   * ImplicitUsings у .csproj автоматично підключає System, System.Linq,
//     System.Collections.Generic тощо — тому "using System;" писати не треба.
//   * Рядок (string) порівнюється за ВМІСТОМ через ==, а не за адресою.
//   * bool — окремий тип. if (1) НЕ компілюється.
// ============================================================================

namespace Intro;   // файлова область імен (file-scoped namespace) — сучасний синтаксис

internal static class Program
{
    private static void Main()
    {
        Section("1. Вивід у консоль");
        OutputDemo();

        Section("2. Типи та змінні");
        TypesDemo();

        Section("3. Рядки (string)");
        StringsDemo();

        Section("4. Перетворення типів");
        ConversionDemo();

        Section("5. Читання з консолі");
        InputDemo();
    }

    // ------------------------------------------------------------------------
    private static void OutputDemo()
    {
        // WriteLine — з переносом рядка, Write — без.
        Console.Write("Привіт, ");
        Console.WriteLine("світе!");

        int a = 7, b = 3;

        // Інтерполяція рядків: $"...{вираз}..."  (замінює cout << a << " " << b)
        Console.WriteLine($"a = {a}, b = {b}, a / b = {a / b}, a % b = {a % b}");
        //  Ділення цілих, як у C++: 7 / 3 == 2. Для дробового — хоча б один операнд double:
        Console.WriteLine($"7.0 / 3 = {7.0 / 3}");

        // Форматування прямо у дужках: {значення : формат}
        Console.WriteLine($"Число: {1234567.891:N2}");   // 1,234,567.89
        Console.WriteLine($"Відсоток: {0.1875:P1}");      // 18.8 %
        Console.WriteLine($"Hex: {255:X}, дата: {DateTime.Now:yyyy-MM-dd}");

        //Офіційна документація Microsoft(C# / .NET) українською мовою:
        //    Інтерполяція рядків($)
        //    https://learn.microsoft.com/uk-ua/dotnet/csharp/language-reference/tokens/interpolated
        //    Стандартні рядки числових форматів
        //    https://learn.microsoft.com/uk-ua/dotnet/standard/base-types/standard-numeric-format-strings
        //    Стандартні рядки формату дати та часу
        //    https://learn.microsoft.com/uk-ua/dotnet/standard/base-types/standard-date-and-time-format-strings
        //    Користувацькі рядки формату дати та часу(Custom)
        //    https://learn.microsoft.com/uk-ua/dotnet/standard/base-types/custom-date-and-time-format-strings

        // Вирівнювання: {значення, ширина}  (від'ємна ширина — вліво)
        Console.WriteLine($"|{"ліворуч",-10}|{"праворуч",10}|");

        // Композитне форматування (старий стиль, як printf): {0}, {1}
        Console.WriteLine("{0} + {1} = {2}", a, b, a + b);

        // Екранування vs дослівний (verbatim) рядок vs "сирий" рядок:
        Console.WriteLine("Шлях:\tC:\\Temp\\new");                 // \t \\ працюють
        Console.WriteLine(@"Шлях:  C:\Temp\new  (у @-рядку \ звичайний)");
        Console.WriteLine("""
            "Сирий" рядок (raw string): лапки " і \ пишемо як є,
            відступ рахується від закриваючих трьох лапок.
            """);
    }

    // ------------------------------------------------------------------------
    private static void TypesDemo()
    {
        // var — це не "динамічний тип", а вивід типу компілятором (аналог auto в C++).
        var i = 42;          // int
        var d = 3.14;        // double
        var m = 9.99m;       // decimal (гроші!) — суфікс m
        var text = "hello";  // string
        var flag = true;     // bool
        Console.WriteLine($"{i.GetType().Name}, {d.GetType().Name}, {m.GetType().Name}, {text.GetType().Name}, {flag.GetType().Name}");

        // Значущі типи (struct): int, double, bool, char, DateTime, власні struct —
        // копіюються при присвоєнні. Посилальні типи (class): string, масиви, List<T> —
        // копіюється посилання. (Детально — у прикладах SOLIS про параметри методів.)

        const double Pi = 3.14159;   // const обчислюється на етапі компіляції
        Console.WriteLine($"Коло радіуса 2: {2 * Pi * 2:F3}");

        // Значення за замовчуванням: числа — 0, bool — false, посилання — null.
        Console.WriteLine($"default(int) = {default(int)}, default(bool) = {default(bool)}");

        // Переповнення: за замовчуванням C# мовчки "загортає" (як у C++ для unsigned).
        // checked робить його помилкою під час виконання:
        int max = int.MaxValue;
        Console.WriteLine($"unchecked: {max + 1}");   // -2147483648
        try
        {
            int overflow = checked(max + 1);
            Console.WriteLine(overflow);
        }
        catch (OverflowException)
        {
            Console.WriteLine("checked: спіймали OverflowException");
        }

        // bool строгий: наступний рядок НЕ скомпілюється —
        //   if (i) { }            // помилка: int не є bool
        if (i != 0) Console.WriteLine("i != 0 — так писати обов'язково");
    }

    // ------------------------------------------------------------------------
    private static void StringsDemo()
    {
        // string — посилальний тип, АЛЕ незмінний (immutable): будь-яка "зміна"
        // створює новий рядок. Тому цикл із += по рядку — повільний (є StringBuilder).
        string s = "abc";
        string upper = s.ToUpper();          // новий рядок
        Console.WriteLine($"s = {s}, upper = {upper} (s не змінився)");

        // == порівнює ВМІСТ (головна пастка для тих, хто прийшов з C++, де це порівняння вказівників).
        string x = "Hello";
        string y = "Hel" + "lo";
        Console.WriteLine($"x == y -> {x == y}");                  // True
        Console.WriteLine($"ReferenceEquals -> {ReferenceEquals(x, y)}");   // може бути будь-як

        Console.WriteLine($"Довжина: {s.Length}, перший символ: {s[0]} (тип char)");
        Console.WriteLine($"Містить 'b'? {s.Contains('b')};  Верхній регістр 'a' -> {char.ToUpper('a')}");
        Console.WriteLine($"'  hi  '.Trim() = '{"  hi  ".Trim()}'");
        Console.WriteLine($"Розбити 'a,b,c' -> [{string.Join(" | ", "a,b,c".Split(','))}]");
        Console.WriteLine($"Повтор: {new string('=', 10)}");

        // Багаторядкова збірка рядка — через StringBuilder (System.Text):
        var sb = new System.Text.StringBuilder();
        for (int k = 1; k <= 5; k++)
            sb.Append(k).Append(' ');
        Console.WriteLine($"StringBuilder: {sb.ToString().Trim()}");
    }

    // ------------------------------------------------------------------------
    private static void ConversionDemo()
    {
        // 1) Явне приведення числових типів (може втратити дані) — як у C++:
        double pi = 3.99;
        int truncated = (int)pi;             // 3 (обрізає, не округлює)
        Console.WriteLine($"(int)3.99 = {truncated};  Math.Round -> {Math.Round(pi)}");

        // 2) Рядок -> число. Три способи:
        int a = int.Parse("42");                     // кине FormatException на сміття
        int b = Convert.ToInt32("42");               // + вміє null -> 0
        bool ok = int.TryParse("123x", out int c);   // БЕЗ винятку: повертає true/false
        Console.WriteLine($"Parse={a}, Convert={b}, TryParse ok={ok}, c={c}");

        // TryParse — стандартний безпечний шаблон для введення користувача:
        if (int.TryParse("777", out int n))
            Console.WriteLine($"Розпарсили: {n}");

        // 3) Будь-що -> рядок: .ToString() (у інтерполяції викликається автоматично).
        Console.WriteLine($"(255).ToString(\"X\") = {255.ToString("X")}");
    }

    // ------------------------------------------------------------------------
    private static void InputDemo()
    {
        // Console.ReadLine() повертає string?  (може бути null у кінці потоку).
        // Щоб демо не зависало під час автозапуску — читаємо лише в інтерактивному режимі.
        if (Console.IsInputRedirected)
        {
            Console.WriteLine("(введення пропущено: консоль не інтерактивна)");
            return;
        }

        Console.Write("Введіть ваше ім'я: ");
        string? name = Console.ReadLine();
        name = string.IsNullOrWhiteSpace(name) ? "незнайомець" : name.Trim();

        Console.Write("Введіть рік народження: ");
        string? raw = Console.ReadLine();

        if (int.TryParse(raw, out int year) && year > 0 && year <= DateTime.Now.Year)
            Console.WriteLine($"Привіт, {name}! Тобі приблизно {DateTime.Now.Year - year} років.");
        else
            Console.WriteLine($"Привіт, {name}! Рік розпізнати не вдалося.");
    }

    // ------------------------------------------------------------------------
    //  Допоміжний метод. Оголошений НИЖЧЕ за Main, але викликається з нього —
    //  у C# це нормально (жодних forward-декларацій, як у C++).
    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ".PadRight(70, '='));
    }
}
