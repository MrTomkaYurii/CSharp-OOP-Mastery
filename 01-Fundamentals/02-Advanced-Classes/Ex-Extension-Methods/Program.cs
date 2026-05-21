// ============================================================================
// Тема: Методи розширення (extension methods)
// ----------------------------------------------------------------------------
// Метод розширення дозволяє «додати» метод до вже наявного типу (навіть чужого,
// напр. string чи IEnumerable<T>), НЕ змінюючи його вихідний код і не успадковуючи.
//
// Умови (усі обов'язкові):
//   * метод оголошено в СТАТИЧНОМУ класі (див. Ex-Static-Class);
//   * сам метод static;
//   * перший параметр має модифікатор this і задає тип, який розширюється.
//
// Насправді це лише «синтаксичний цукор»: виклик  text.WordCount()
// компілятор перетворює на  StringExtensions.WordCount(text).
// Саме так працює весь LINQ: Where/Select/OrderBy — методи розширення для
// IEnumerable<T> у класі System.Linq.Enumerable.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ex_Extension_Methods;   // щоб було видно клас StringExtensions / EnumerableExtensions

namespace Ex_Extension_Methods
{
    public static class StringExtensions
    {
        // "this string value" => метод з'являється на КОЖНОМУ рядку
        public static bool IsNullOrBlank(this string? value)
            => string.IsNullOrWhiteSpace(value);

        // Обрізати рядок до maxLength символів, додавши "…"
        public static string Truncate(this string value, int maxLength)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (maxLength < 0) throw new ArgumentOutOfRangeException(nameof(maxLength));

            return value.Length <= maxLength
                ? value
                : value[..maxLength].TrimEnd() + "…";
        }

        // Порахувати слова
        public static int WordCount(this string value)
            => value.IsNullOrBlank()
                ? 0
                : value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;

        // Метод розширення МОЖЕ бути узагальненим
        public static string ToTitleCase(this string value)
        {
            if (value.IsNullOrBlank()) return value;
            var ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(value.ToLower(CultureInfo.CurrentCulture));
        }
    }

    // Розширення для інтерфейсу — працюватиме для масиву, List<T>, результату LINQ...
    public static class EnumerableExtensions
    {
        // Розбити послідовність на «пачки» по size елементів
        public static IEnumerable<IReadOnlyList<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            var bucket = new List<T>(size);
            foreach (var item in source)
            {
                bucket.Add(item);
                if (bucket.Count == size)
                {
                    yield return bucket;
                    bucket = new List<T>(size);
                }
            }
            if (bucket.Count > 0)
                yield return bucket;
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            string title = "розширювальні методи в c#";
            string longText = "Методи розширення — це синтаксичний цукор над статичними викликами";

            // Виклик через крапку — так, наче методи «рідні» для string
            Console.WriteLine($"IsNullOrBlank : {"   ".IsNullOrBlank()}");
            Console.WriteLine($"ToTitleCase   : {title.ToTitleCase()}");
            Console.WriteLine($"Truncate(20)  : {longText.Truncate(20)}");
            Console.WriteLine($"WordCount     : {longText.WordCount()}");

            // Те саме, але явним статичним викликом — повністю рівнозначно
            Console.WriteLine($"\nЯвний виклик  : {StringExtensions.WordCount(longText)}");

            // Розширення для IEnumerable<T> + поєднання з LINQ
            Console.WriteLine("\nBatch(1..10, 3):");
            foreach (var chunk in Enumerable.Range(1, 10).Batch(3))
                Console.WriteLine($"  [{string.Join(", ", chunk)}]");

            // null-рядок: метод розширення МОЖНА викликати на null,
            // бо це звичайний статичний виклик (NullReferenceException не буде).
            string? nothing = null;
            Console.WriteLine($"\nnull.IsNullOrBlank() = {nothing.IsNullOrBlank()}");
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * Метод розширення для власного інтерфейсу (напр. IShape.Describe()).
//  * Показати «затінення»: якщо в типі є свій метод із такою ж сигнатурою —
//    виграє метод екземпляра, а не розширення.
//  * Розширення для enum (Enum.GetDescription() через атрибут).
//  * Ланцюжок розширень у стилі fluent: builder.WithName(...).WithAge(...).
//  * Порівняти з «справжнім» доданням методу через partial class (той самий проєкт).
// ----------------------------------------------------------------------------
