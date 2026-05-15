// ============================================================================
//  INTRO 03 — Методи (функції)   (з C++ на C#)
// ----------------------------------------------------------------------------
//  Ключове:
//   * Вільних функцій немає — усі методи належать класу. "Функції рівня файлу"
//     тут це private static методи.
//   * Немає оголошень наперед і заголовків: метод можна викликати до місця,
//     де він описаний у файлі.
//   * Перевантаження — за кількістю/типами параметрів (як у C++).
//   * Замість вказівників для "вихідних" параметрів — модифікатори out / ref / in.
//   * Кілька значень зручно повертати кортежем (tuple).
//   * Функцію можна покласти у змінну: делегати Func / Action + лямбди.
// ============================================================================

namespace Intro;

internal static class Program
{
    private static void Main()
    {
        Section("1. Оголошення, повернення, перевантаження");
        Console.WriteLine($"Square(6) = {Square(6)}");
        Console.WriteLine($"Square(2.5) = {Square(2.5)}");          // інше перевантаження
        Console.WriteLine($"Max(3, 9) = {Max(3, 9)}");
        Console.WriteLine($"Max(3, 9, 5) = {Max(3, 9, 5)}");

        Section("2. Необов'язкові та іменовані параметри");
        Console.WriteLine(Greet("Аня"));
        Console.WriteLine(Greet("Богдан", greeting: "Вітаю"));
        Console.WriteLine(Greet(name: "Оля", loud: true));

        Section("3. out / ref / in");
        RefOutInDemo();

        Section("4. params — змінна кількість аргументів");
        Console.WriteLine($"Sum() = {Sum()}");
        Console.WriteLine($"Sum(1, 2, 3) = {Sum(1, 2, 3)}");
        Console.WriteLine($"Sum(масив) = {Sum(new[] { 10, 20, 30 })}");

        Section("5. Кілька значень: кортеж + деконструкція");
        var stats = MinMax(new[] { 7, 2, 9, 4, 5 });
        Console.WriteLine($"min = {stats.min}, max = {stats.max}");
        (int lo, int hi) = MinMax(new[] { 3, 1, 8 });   // одразу розкладаємо
        Console.WriteLine($"lo = {lo}, hi = {hi}");

        Section("6. Рекурсія та локальні функції");
        Console.WriteLine($"5! = {Factorial(5)}");
        Console.WriteLine($"Fib(10) = {Fibonacci(10)}");

        Section("7. Метод як значення: делегати та лямбди");
        DelegatesDemo();
    }

    // ---- 1. Перевантаження -------------------------------------------------
    // Вирази-тіла (=>) для однорядкових методів.
    private static int Square(int x) => x * x;
    private static double Square(double x) => x * x;

    private static int Max(int a, int b) => a > b ? a : b;
    private static int Max(int a, int b, int c) => Max(Max(a, b), c);

    // ---- 2. Необов'язкові / іменовані ------------------------------------
    // Параметри зі значенням за замовчуванням мають іти останніми.
    private static string Greet(string name, string greeting = "Привіт", bool loud = false)
    {
        string line = $"{greeting}, {name}!";
        return loud ? line.ToUpper() : line;
    }

    // ---- 3. out / ref / in ---------------------------------------------
    private static void RefOutInDemo()
    {
        // out — метод ЗОБОВ'ЯЗАНИЙ присвоїти значення; вхідне значення не потрібне.
        if (TryDivide(10, 4, out double q))
            Console.WriteLine($"10 / 4 = {q}");

        // ref — метод працює з тією самою змінною викликача (читає і змінює).
        int value = 10;
        TripleInPlace(ref value);
        Console.WriteLine($"після TripleInPlace: {value}");

        // in — передача за посиланням лише для читання (оптимізація для великих struct).
        var big = new Vector3(1, 2, 2);
        Console.WriteLine($"довжина вектора = {Length(in big):F3}");
    }

    private static bool TryDivide(double a, double b, out double result)
    {
        if (b == 0) { result = 0; return false; }
        result = a / b;
        return true;
    }

    private static void TripleInPlace(ref int n) => n *= 3;

    private static double Length(in Vector3 v) => Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

    private readonly struct Vector3(double x, double y, double z)
    {
        public double X { get; } = x;
        public double Y { get; } = y;
        public double Z { get; } = z;
    }

    // ---- 4. params -------------------------------------------------------
    private static int Sum(params int[] numbers)
    {
        int total = 0;
        foreach (int n in numbers) total += n;
        return total;
    }

    // ---- 5. Кортежі -----------------------------------------------------
    private static (int min, int max) MinMax(int[] data)
    {
        int min = data[0], max = data[0];
        foreach (int x in data)
        {
            if (x < min) min = x;
            if (x > max) max = x;
        }
        return (min, max);
    }

    // ---- 6. Рекурсія / локальні функції -------------------------------
    private static long Factorial(int n) => n <= 1 ? 1 : n * Factorial(n - 1);

    private static int Fibonacci(int n)
    {
        // Локальна функція — метод усередині методу. Бачить змінні зовнішнього,
        // не видима зовні. Зручно для допоміжної рекурсії.
        return Fib(n);

        static int Fib(int k) => k < 2 ? k : Fib(k - 1) + Fib(k - 2);
    }

    // ---- 7. Делегати та лямбди --------------------------------------
    private static void DelegatesDemo()
    {
        // Func<аргументи..., результат> — функція, що повертає значення.
        Func<int, int, int> add = (a, b) => a + b;
        Func<int, bool> isEven = n => n % 2 == 0;

        // Action<...> — нічого не повертає (void).
        Action<string> log = msg => Console.WriteLine($"  log: {msg}");

        Console.WriteLine($"add(2, 3) = {add(2, 3)}");
        log("лямбда у змінній");

        // Метод, що приймає іншу функцію як параметр (функція вищого порядку):
        int[] nums = { 1, 2, 3, 4, 5, 6 };
        Console.WriteLine($"кількість парних = {CountWhere(nums, isEven)}");
        Console.WriteLine($"кількість > 3    = {CountWhere(nums, n => n > 3)}");
    }

    private static int CountWhere(int[] data, Func<int, bool> predicate)
    {
        int count = 0;
        foreach (int x in data)
            if (predicate(x)) count++;
        return count;
    }

    // ---------------------------------------------------------------------
    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ".PadRight(70, '='));
    }
}
