using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор SelectMany / Складене речення 'from' (Декартів добуток)
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції та сплющення)
    // Призначення: Об'єднання декількох послідовностей (декартів добуток або сплющення колекцій 1-до-багатьох).
    //
    // Принцип роботи:
    //   - У Query Syntax декілька операторів 'from' підряд (compound from clause) транслюються
    //     компілятором у виклик методу SelectMany().
    //   - Для кожного елемента першої послідовності перебираються всі елементи другої послідовності.
    //   - За допомогою речення 'where' можна відсікти непотрібні пари.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 14: Використання складеного 'from' для генерації всіх пар чисел (a, b)
        /// з двох масивів, для яких виконується умова a < b (Query Syntax).
        /// </summary>
        public void DataSetLinq14()
        {
            var numbersA = testDS.Tables["NumbersA"].AsEnumerable();
            var numbersB = testDS.Tables["NumbersB"].AsEnumerable();

            #region LINQ Query Syntax (Compound from)

            // Перебираємо елементи a з першої таблиці та b з другої; залишаємо тільки пари, де a < b
            var pairs =
                from a in numbersA
                from b in numbersB
                where a.Field<int>("number") < b.Field<int>("number")
                select new { a = a.Field<int>("number"), b = b.Field<int>("number") };

            Console.WriteLine("Пари, де a < b (з DataTable):");
            foreach (var pair in pairs)
            {
                Console.WriteLine("{0} менше за {1}", pair.a, pair.b);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 14A: Аналогічна генерація пар за допомогою спрощених цілочислових масивів.
        /// </summary>
        public void DataSetLinq14A()
        {
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            #region LINQ Query Syntax (Спрощені масиви)

            var pairs =
                from a in numbersA
                from b in numbersB
                where a < b
                select new { a, b };

            Console.WriteLine();
            Console.WriteLine("***********************************");
            Console.WriteLine("Query Syntax зі спрощеними масивами — Пари де a < b:");
            foreach (var pair in pairs)
            {
                Console.WriteLine("{0} менше за {1}", pair.a, pair.b);
            }

            #endregion
        }
    }
}
