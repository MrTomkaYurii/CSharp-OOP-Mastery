using System;

namespace Ex_ExtensionMethods;

/*
 * ── Методи-розширення ──────────────────────────────────────────────
 *
 * Метод-розширення дозволяє "додати" метод до вже наявного типу,
 * не змінюючи його вихідний код (навіть до string чи int).
 *
 * Правила:
 *   • метод має бути статичним у статичному класі;
 *   • перший параметр позначається словом `this` — це і є той об'єкт,
 *     для якого викликають метод.
 *
 * Виклик виглядає як звичайний метод об'єкта:
 *     "abc".Repeat(3)
 * але це те саме, що:
 *     StringExtensions.Repeat("abc", 3)
 *
 * Очікуваний вивід:
 *   ку-ку-ку-
 *   ку-ку-ку-
 *   Число 10 парне? True
 *   Число 7 парне? False
 */

static class StringExtensions
{
    // Метод-розширення для типу string: повторює рядок задану кількість разів.
    public static string Repeat(this string text, int times)
    {
        string result = "";

        for (int i = 0; i < times; i++)
        {
            result = result + text;
        }

        return result;
    }
}

static class IntExtensions
{
    // Метод-розширення для типу int: перевіряє, чи число парне.
    public static bool IsEven(this int number)
    {
        return number % 2 == 0;
    }
}

class Program
{
    static void Main()
    {
        string word = "ку-";

        // Виклик як методу об'єкта:
        Console.WriteLine(word.Repeat(3));

        // Той самий метод, викликаний напряму через статичний клас:
        Console.WriteLine(StringExtensions.Repeat(word, 3));

        int a = 10;
        int b = 7;
        Console.WriteLine($"Число {a} парне? {a.IsEven()}");
        Console.WriteLine($"Число {b} парне? {b.IsEven()}");
    }
}
