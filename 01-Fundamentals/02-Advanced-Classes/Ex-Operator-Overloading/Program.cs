// ============================================================================
// Тема: Перевантаження операторів (operator overloading)
// ----------------------------------------------------------------------------
// C# дозволяє задати власну поведінку стандартних операторів (+, -, *, ==, ...)
// для свого типу. Це робить код із «математичними» об'єктами (вектори, гроші,
// комплексні числа, дроби) коротким і природним: v1 + v2 замість v1.Add(v2).
//
// Правила, які демонструє приклад:
//   * оператор оголошується як  public static  метод з ключовим словом operator;
//   * хоча б один параметр має бути типу самого класу/структури;
//   * оператори == і != перевантажуються ТІЛЬКИ парою; крім того, за угодою
//     разом із ними перевизначають Equals і GetHashCode;
//   * оператори не можна оголосити в звичайному методі — лише всередині типу;
//   * implicit/explicit задають правила приведення типів.
// ============================================================================

using System;

namespace Ex_Operator_Overloading
{
    /// <summary>
    /// Двовимірний вектор. readonly struct — незмінний значимий тип:
    /// кожна операція повертає НОВИЙ вектор, а не змінює наявний.
    /// </summary>
    public readonly struct Vector2D
    {
        public double X { get; }
        public double Y { get; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        // --- Бінарні арифметичні оператори ------------------------------------

        // Додавання векторів: (a.X + b.X, a.Y + b.Y)
        public static Vector2D operator +(Vector2D a, Vector2D b)
            => new Vector2D(a.X + b.X, a.Y + b.Y);

        // Віднімання векторів
        public static Vector2D operator -(Vector2D a, Vector2D b)
            => new Vector2D(a.X - b.X, a.Y - b.Y);

        // Множення вектора на число (скаляр). Перевантажуємо ОБИДВА порядки
        // операндів, щоб працювало і v * 2, і 2 * v.
        public static Vector2D operator *(Vector2D v, double k)
            => new Vector2D(v.X * k, v.Y * k);

        public static Vector2D operator *(double k, Vector2D v)
            => v * k;   // перевикористовуємо вже написаний оператор

        // --- Унарний оператор ------------------------------------------------

        // Унарний мінус: -v => протилежний напрямок
        public static Vector2D operator -(Vector2D v)
            => new Vector2D(-v.X, -v.Y);

        // --- Оператори порівняння (тільки парою!) ----------------------------

        public static bool operator ==(Vector2D a, Vector2D b)
            => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector2D a, Vector2D b)
            => !(a == b);   // виражаємо через уже визначений ==

        // За угодою .NET: якщо перевантажили ==, перевизначаємо і ці два методи,
        // інакше словники/множини/LINQ поводитимуться несподівано.
        public override bool Equals(object? obj)
            => obj is Vector2D other && this == other;

        public override int GetHashCode()
            => HashCode.Combine(X, Y);

        // --- Приведення типів ----------------------------------------------

        // НЕЯВНЕ приведення: число -> вектор уздовж осі X. Неявне роблять лише
        // тоді, коли перетворення завжди безпечне й без втрати даних.
        public static implicit operator Vector2D(double x)
            => new Vector2D(x, 0);

        // ЯВНЕ приведення: вектор -> його довжина (double). Явне — бо це вже
        // «втрата інформації» (напрямок губиться), тож потрібне (double)v.
        public static explicit operator double(Vector2D v)
            => Math.Sqrt(v.X * v.X + v.Y * v.Y);

        // Гарний текстовий вигляд для виводу в консоль
        public override string ToString() => $"({X}; {Y})";
    }

    internal static class Program
    {
        private static void Main()
        {
            var a = new Vector2D(2, 3);
            var b = new Vector2D(1, -1);

            Console.WriteLine($"a          = {a}");
            Console.WriteLine($"b          = {b}");
            Console.WriteLine($"a + b      = {a + b}");        // (3; 2)
            Console.WriteLine($"a - b      = {a - b}");        // (1; 4)
            Console.WriteLine($"a * 2      = {a * 2}");        // (4; 6)
            Console.WriteLine($"3 * b      = {3 * b}");        // (3; -3)
            Console.WriteLine($"-a         = {-a}");           // (-2; -3)

            var aCopy = new Vector2D(2, 3);                    // окремий об'єкт з тими самими числами
            Console.WriteLine($"a == aCopy = {a == aCopy}");   // True  — порівняння за значенням (наш operator ==)
            Console.WriteLine($"a == b     = {a == b}");       // False

            // Неявне приведення double -> Vector2D спрацьовує саме по собі
            Vector2D fromNumber = 5;
            Console.WriteLine($"(Vector2D)5 = {fromNumber}");  // (5; 0)

            // Явне приведення Vector2D -> double: потрібні дужки (double)
            double length = (double)a;
            Console.WriteLine($"|a|        = {length:F3}");    // 3.606
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * Оператори ++/--, +=/-= (складені присвоєння генеруються автоматично з +/-).
//  * Оператори true/false і & | для «трипозиційної логіки».
//  * Реалізацію IEquatable<Vector2D> для швидшого порівняння без боксингу.
//  * Тип Fraction (звичайний дріб) із скороченням у конструкторі — гарна вправа.
//  * Клас Money(decimal amount, string currency): заборонити + для різних валют.
// ----------------------------------------------------------------------------
