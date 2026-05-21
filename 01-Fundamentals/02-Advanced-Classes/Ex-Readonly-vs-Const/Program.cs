// ============================================================================
// Тема: const vs static readonly vs readonly (поле екземпляра)
// ----------------------------------------------------------------------------
// Усі три способи задають величину, яку не можна змінити після ініціалізації,
// АЛЕ поводяться по-різному:
//
//              | Коли задається значення            | Одне на клас? | Інлайниться?
//  ------------|-----------------------------------|---------------|------------
//  const       | під час КОМПІЛЯЦІЇ (в оголошенні)  | так (неявно)  | ТАК
//  static      | під час виконання: в оголошенні    | так           | ні
//    readonly  | або в СТАТИЧНОМУ конструкторі      |               |
//  readonly    | в оголошенні або в конструкторі    | ні (у кожного | ні
//              | екземпляра — свій раз              |  об'єкта своє)|
//
// «Інлайниться» означає, що компілятор підставляє саме число в місце виклику.
// Тому якщо публічний const у бібліотеці змінити, залежні збірки треба
// ПЕРЕкомпілювати — інакше вони й далі використовують старе значення.
// static readonly такої проблеми не має.
// ============================================================================

using System;

namespace Ex_Readonly_vs_Const
{
    public class Circle
    {
        // 1) const — істинна константа, відома компілятору. Тільки примітиви,
        //    string, enum або null. Неявно static: звертання Circle.Pi.
        public const double Pi = 3.14159265358979;

        // 2) static readonly — «константа часу виконання». Значення можна
        //    обчислити (виклик методу, читання конфігурації тощо).
        public static readonly DateTime FirstCreatedAt;

        // 3) readonly-поле екземпляра — у КОЖНОГО об'єкта власне незмінне значення,
        //    яке фіксується в конструкторі й далі не міняється.
        public readonly double Radius;

        // Статичний конструктор ініціалізує static readonly один раз на клас
        static Circle()
        {
            FirstCreatedAt = DateTime.Now;   // так з const зробити НЕ можна
        }

        public Circle(double radius)
        {
            // readonly-полю можна присвоїти лише тут (у конструкторі) або в оголошенні
            Radius = radius > 0 ? radius : throw new ArgumentOutOfRangeException(nameof(radius));
        }

        // Площа використовує const Pi — тут воно буде інлайнене як число
        public double Area => Pi * Radius * Radius;

        public void TryToMutate()
        {
            // Radius = 10;        // ПОМИЛКА CS0191: readonly-поле не можна змінювати поза конструктором
            // Pi = 3.14;          // ПОМИЛКА CS0131: лівий бік має бути змінюваним
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine($"Circle.Pi (const)            = {Circle.Pi}");
            Console.WriteLine($"Circle.FirstCreatedAt (s/ro) = {Circle.FirstCreatedAt:HH:mm:ss.fff}");

            var small = new Circle(2);
            var big = new Circle(10);

            // readonly-поле Radius у кожного об'єкта СВОЄ
            Console.WriteLine($"small.Radius = {small.Radius}, small.Area = {small.Area:F2}");
            Console.WriteLine($"big.Radius   = {big.Radius}, big.Area   = {big.Area:F2}");

            // Локальна константа — теж поширений випадок const
            const int MaxRetries = 3;
            Console.WriteLine($"MaxRetries (локальна const)  = {MaxRetries}");
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати / підводні камені:
//  * readonly для ПОСИЛАЛЬНОГО типу (readonly List<int> items): не можна
//    замінити сам список, але Add/Remove працюють — незмінне лише посилання.
//  * readonly struct — весь тип незмінний (див. Ex-Operator-Overloading).
//  * Показати «версійну» проблему const на двох проєктах (бібліотека + консоль).
//  * ref readonly та in-параметри — просунута тема.
// ----------------------------------------------------------------------------
