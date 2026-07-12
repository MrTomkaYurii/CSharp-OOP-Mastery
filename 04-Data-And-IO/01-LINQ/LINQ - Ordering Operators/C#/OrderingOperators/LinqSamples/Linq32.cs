using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Сортування числових типів із плаваючою комою за спаданням
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Впорядкування послідовності дійсних чисел (double) від найбільшого до найменшого.
    //
    // Принцип роботи:
    //   - Query Syntax: orderby d descending.
    //   - Method Syntax: OrderByDescending(d => d).
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 32: Сортування масиву чисел типу double за спаданням (Query Syntax).
        /// </summary>
        public void Linq32()
        {
            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            #region LINQ Query Syntax

            var sortedDoubles =
                from d in doubles
                orderby d descending
                select d;

            Console.WriteLine("Query Syntax — Дійсні числа від найбільшого до найменшого:");
            foreach (var d in sortedDoubles)
            {
                Console.WriteLine(d);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 32A: Сортування дійсних чисел за спаданням за допомогою Method Syntax (OrderByDescending).
        /// </summary>
        public void Linq32A()
        {
            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            #region LINQ Method Syntax (Lambda)

            var sortedDoubles = doubles.OrderByDescending(d => d);

            Console.WriteLine();
            Console.WriteLine("***********************************************");
            Console.WriteLine("Lambda-вираз — Дійсні числа від найбільшого до найменшого:");
            foreach (var d in sortedDoubles)
            {
                Console.WriteLine(d);
            }

            #endregion
        }
    }
}
