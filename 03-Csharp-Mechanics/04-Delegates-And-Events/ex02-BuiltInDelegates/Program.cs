using System;
using System.Collections.Generic;

// ============================================================================
// КАСТОМНИЙ ПРИКЛАД 2: Вбудовані стандартні делегати (Action, Func, Predicate)
// ============================================================================
// НАВІЩО ЦЕЙ ПРИКЛАД:
// Щоразу оголошувати свій тип через 'delegate void ...' незручно.
// У бібліотеці .NET уже є готові системні узагальнені делегати:
//
// 1. Action<...>        — дія, повертає VOID.
// 2. Func<..., TResult> — обчислення, повертає РЕЗУЛЬТАТ (останній тип у <>).
// 3. Predicate<T>       — перевірка умови, завжди повертає BOOL.
// ============================================================================

namespace ex02_BuiltInDelegates;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Приклад 2: Вбудовані делегати Action, Func, Predicate ===\n");

        // ====================================================================
        // [КРОК 1] ДЕЛЕГАТ Action (Завжди повертає void)
        // ====================================================================
        // Використовується для дій: друк на консоль, запис у файл, виклик процедур.
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- [КРОК 1] ДЕЛЕГАТ Action (void) ---");
        Console.ResetColor();

        // 1А. Action без параметрів (сигнатура: void Action())
        Action showGreeting = () => Console.WriteLine("Привіт! Я Action без параметрів.");
        showGreeting();

        // 1Б. Action з двома параметрами (сигнатура: void Action<string, int>)
        Action<string, int> printStudentInfo = (name, score) =>
        {
            Console.WriteLine($"Студент: {name}, Бали: {score}/100");
        };
        printStudentInfo("Іван", 95);
        printStudentInfo("Ольга", 88);
        Console.WriteLine();

        // ====================================================================
        // [КРОК 2] ДЕЛЕГАТ Func (Завжди повертає значення)
        // ====================================================================
        // УВАГА: Останній вказаний тип у <> — це обов'язково тип значення, що повертається!
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- [КРОК 2] ДЕЛЕГАТ Func (з поверненням результату) ---");
        Console.ResetColor();

        // 2А. Func<int, int, int> => приймає (int a, int b), повертає int
        Func<int, int, int> add = (a, b) => a + b;
        Func<int, int, int> multiply = (a, b) => a * b;

        Console.WriteLine($"5 + 10 = {add(5, 10)}");
        Console.WriteLine($"5 * 10 = {multiply(5, 10)}");

        // 2Б. Func<string, int> => приймає string, повертає int (довжину)
        Func<string, int> stringLength = text => text.Length;
        Console.WriteLine($"Довжина слова 'Делегат': {stringLength("Делегат")} символів");
        Console.WriteLine();

        // ====================================================================
        // [КРОК 3] ДЕЛЕГАТ Predicate (Спеціалізована перевірка умови)
        // ====================================================================
        // Сигнатура: bool Predicate<T>(T item). Це еквівалент Func<T, bool>.
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- [КРОК 3] ДЕЛЕГАТ Predicate (повертає bool) ---");
        Console.ResetColor();

        Predicate<int> isAdult = age => age >= 18;
        Console.WriteLine($"Чи є вік 16 повнолітнім? {isAdult(16)}");
        Console.WriteLine($"Чи є вік 20 повнолітнім? {isAdult(20)}");
        Console.WriteLine();

        // ====================================================================
        // [КРОК 4] РЕАЛЬНЕ ЗАСТОСУВАННЯ В СТАНДАРТНИХ МЕТОДАХ .NET (List<T>)
        // ====================================================================
        // Чому це важливо: стандартні методи C# вже побудовані на Action і Predicate!
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- [КРОК 4] Використання у стандартній колекції List<T> ---");
        Console.ResetColor();

        List<string> cities = new() { "Київ", "Львів", "Одеса", "Харків", "Дніпро", "Ужгород" };

        Console.WriteLine("Міста з назвою довшою за 5 букв (List.FindAll приймає Predicate):");
        // Метод FindAll приймає Predicate<string>:
        List<string> longCities = cities.FindAll(city => city.Length > 5);

        // Метод ForEach приймає Action<string>:
        longCities.ForEach(city => Console.WriteLine($" * {city}"));

        // --------------------------------------------------------------------
        // ПІДСУМОК-ШПАРГАЛКА ДЛЯ СТУДЕНТІВ:
        // - Потрібно виконати дію без повернення? -> Action<T>
        // - Потрібно щось обчислити і повернути результат? -> Func<T, TResult>
        // - Потрібно перевірити умову (True/False)? -> Predicate<T>
        // --------------------------------------------------------------------
    }
}
