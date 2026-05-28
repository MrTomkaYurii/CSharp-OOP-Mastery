using System;

/*
 * ── Статичні класи (Solis, с. 210) ──────────────────────────────────
 *
 * Статичний клас:
 *   • містить лише статичні члени;
 *   • не можна створити через `new` і не можна успадкувати;
 *   • слугує контейнером для допоміжних функцій та констант
 *     (аналог System.Math).
 *
 * До членів звертаються через ім'я класу: MyMath.IsOdd(...).
 *
 * Очікуваний вивід:
 *   3 is odd is True.
 *   3 * 2 = 6.
 */

// ── Статичний клас-утиліта ──────────────────────────────────────────
public static class MyMath
{
    public static float PI = 3.14f;

    // Перевіряє, чи число непарне.
    public static bool IsOdd(int x)
    {
        return x % 2 == 1;
    }

    // Повертає число, помножене на 2.
    public static int Times2(int x)
    {
        return 2 * x;
    }
}

class Program
{
    static void Main()
    {
        int val = 3;

        Console.WriteLine("{0} is odd is {1}.", val, MyMath.IsOdd(val));
        Console.WriteLine($"{val} * 2 = {MyMath.Times2(val)}.");
    }
}
