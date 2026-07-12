using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Select — Обчислювані властивості в анонімних типах
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції)
    // Призначення: Формування нового об'єкта, властивості якого є результатом обчислення
    //              або логічної перевірки (наприклад, визначення парності числа).
    //
    // Принцип роботи:
    //   - Всередині виразу new { ... } можна виконувати будь-які обчислення (арифметичні операції,
    //     тернарні оператори ?:, виклики методів).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 10: Створення анонімного об'єкта з текстовою назвою цифри та булевим прапорцем Even (парне/непарне).
        /// </summary>
        public void DataSetLinq10()
        {
            var numbers = testDS.Tables["Numbers"].AsEnumerable();
            var digits = testDS.Tables["Digits"];

            #region LINQ Method Syntax (Lambda)

            var digitOddEvens = numbers.Select(n => new
            {
                Digit = digits.Rows[n.Field<int>("number")]["digit"],
                Even = (n.Field<int>("number") % 2 == 0)
            });

            Console.WriteLine("Lambda-вираз — Визначення парності чисел:");
            foreach (var d in digitOddEvens)
            {
                Console.WriteLine("Цифра {0} є {1}.", d.Digit, d.Even ? "парною" : "непарною");
            }

            #endregion
        }

        /// <summary>
        /// Приклад 10A: Визначення парності цифр за допомогою Query Syntax.
        /// </summary>
        public void DataSetLinq10A()
        {
            int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Query Syntax

            var digitOddEvens =
                from num in numbers
                select new
                {
                    Digit = digits[num],
                    Even = digits[num].Length % 2 == 0
                };

            Console.WriteLine();
            Console.WriteLine("******************************************");
            Console.WriteLine("Query Syntax — Цифра та парність довжини її назви:");
            foreach (var d in digitOddEvens)
            {
                Console.WriteLine("Цифра {0} має {1} довжину слова.", d.Digit, d.Even ? "парну" : "непарну");
            }

            #endregion
        }
    }
}
