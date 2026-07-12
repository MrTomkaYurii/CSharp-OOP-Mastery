using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Комбінування операторів Where та Select
    // ==============================================================================================
    // Категорія: Projection & Restriction Operators (Проєкція з фільтрацією)
    // Призначення: Поєднання відбору потрібних елементів (Where) із подальшою їхньою трансформацією (Select).
    //
    // Принцип роботи:
    //   - Спочатку виконується фільтрація елементів за умовою предиката.
    //   - Тільки ті елементи, які пройшли фільтр, передаються у функцію селектора для трансформації.
    //   - Завдяки потоковому виконанню (streaming) елементи обробляються конвеєрно один за одним.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 13: Поєднання select і where для вибірки текстової форми цифр, менших за 5 (Query Syntax).
        /// </summary>
        public void DataSetLinq13()
        {
            var numbers = testDS.Tables["Numbers"].AsEnumerable();
            var digits = testDS.Tables["Digits"];

            #region LINQ Query Syntax

            // Фільтрація чисел < 5 та їхня проєкція у відповідне слово з таблиці Digits
            var lowNums =
                from n in numbers
                where n.Field<int>("number") < 5
                select digits.Rows[n.Field<int>("number")].Field<string>("digit");

            Console.WriteLine("Query Syntax — Числа < 5 у текстовому вигляді:");
            foreach (var num in lowNums)
            {
                Console.WriteLine(num);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 13A: Аналогічна операція за допомогою спрощених масивів та порівняння Query і Method Syntax.
        /// </summary>
        public void DataSetLinq13A()
        {
            var numbers = Enumerable.Range(0, 9);
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Query vs Method Syntax

            // Варіант 1: Query Syntax
            var lowNums =
                from num in numbers
                where num < 5
                select digits[num];

            // Варіант 2: Method Syntax (ланцюжок Where -> Select)
            var lowNumsWithLambda = numbers
                .Where(num => num < 5)
                .Select(index => digits[index]);

            Console.WriteLine();
            Console.WriteLine("*************************************");
            Console.WriteLine("Query Syntax — Числа < 5:");
            foreach (var num in lowNums)
            {
                Console.WriteLine(num);
            }

            Console.WriteLine();
            Console.WriteLine("*************************************");
            Console.WriteLine("Method Syntax (Lambda) — Числа < 5:");
            foreach (var num in lowNumsWithLambda)
            {
                Console.WriteLine(num);
            }

            #endregion
        }
    }
}
