using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Складене сортування та оператор ThenBy (Вторинне сортування)
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Багаторівневе сортування. Якщо декілька елементів мають однакові значення первинного ключа,
    //              вони додатково впорядковуються за вторинним ключем.
    //
    // Принцип роботи:
    //   - Query Syntax: після orderby вказуються кілька ключів через кому:
    //       orderby key1, key2 [descending], key3
    //   - Method Syntax: обов'язково використовується метод ThenBy() або ThenByDescending()!
    //     ПОМИЛКА: повторний виклик .OrderBy().OrderBy() ПОВНІСТЮ ЗНИЩИТЬ перше сортування!
    //     ПРАВИЛЬНО: source.OrderBy(k1).ThenBy(k2).
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 35: Сортування назв цифр спочатку за довжиною слова, а потім за алфавітом (Query Syntax).
        /// </summary>
        public void Linq35()
        {
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Query Syntax (Складений orderby)

            // Сортуємо спочатку за довжиною (digit.Length), а при однаковій довжині — за самим словом (алфавітно)
            var sortedDigits =
                from digit in digits
                orderby digit.Length, digit
                select digit;

            Console.WriteLine("Query Syntax — Цифри, відсортовані за довжиною, потім за алфавітом:");
            foreach (var d in sortedDigits)
            {
                Console.WriteLine(d);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 35A: Багаторівневе сортування за допомогою Method Syntax (OrderBy + ThenBy).
        /// </summary>
        public void Linq35A()
        {
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Method Syntax (OrderBy + ThenBy)

            var sortedDigits = digits.OrderBy(digit => digit.Length).ThenBy(digit => digit);

            Console.WriteLine();
            Console.WriteLine("**************************************");
            Console.WriteLine("Method Syntax — Сортування через OrderBy().ThenBy():");
            foreach (var d in sortedDigits)
            {
                Console.WriteLine(d);
            }

            #endregion
        }
    }
}
