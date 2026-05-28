using System;

/*
 * ── Методи-розширення (Solis, с. 214) ──────────────────────────────
 *
 * Метод-розширення — це статичний метод статичного класу, перший
 * параметр якого позначено ключовим словом `this`. Компілятор дозволяє
 * викликати його так, ніби це звичайний метод екземпляра:
 *
 *     md.Average()   замість   ExtendMyData.Average(md)
 *
 * Це той самий підхід, що й на с. 212, але із синтаксичним "цукром".
 * Клас MyData тут `sealed` — метод-розширення все одно працює, бо не
 * потребує успадкування.
 *
 * Очікуваний вивід:
 *   Sum:     12
 *   Average: 4
 */

namespace ExtensionMethods
{
    // ── Клас, який ми розширюємо ────────────────────────────────────
    sealed class MyData
    {
        private double D1, D2, D3;

        public MyData(double d1, double d2, double d3)
        {
            D1 = d1;
            D2 = d2;
            D3 = d3;
        }

        public double Sum()
        {
            return D1 + D2 + D3;
        }
    }

    // ── Статичний клас із методом-розширенням ───────────────────────
    static class ExtendMyData
    {
        // `this MyData md` робить Average методом-розширенням для MyData.
        public static double Average(this MyData md)
        {
            return md.Sum() / 3;
        }
    }

    class Program
    {
        static void Main()
        {
            MyData md = new MyData(3, 4, 5);

            Console.WriteLine($"Sum:     {md.Sum()}");           // звичайний метод
            Console.WriteLine("Average: {0}", md.Average());     // виклик методу-розширення
        }
    }
}
