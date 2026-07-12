using System;
using System.Linq;

namespace ConversionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор ToArray (Примусова матеріалізація в масив)
    // ==============================================================================================
    // Категорія: Conversion Operators (Оператори перетворення типів / Матеріалізації)
    // Призначення: Негайно виконує LINQ-запит і записує всі його результати у фіксований масив T[].
    //
    // Принцип роботи:
    //   - Завершує конвеєр відкладеного виконання (Deferred Execution) і переводить його в негайне (Immediate Execution).
    //   - Виділяє пам'ять під масив заданого розміру та копіює туди елементи.
    //   - Дозволяє отримати O(1) доступ до елементів за індексом або передавати масив у методи,
    //     що вимагають саме T[] замість IEnumerable<T>.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesConversionOperators
    {
        /// <summary>
        /// Приклад 54: Негайне виконання запиту сортування чисел з їх збереженням у масив за допомогою ToArray (Query Syntax).
        /// </summary>
        public void Linq54()
        {
            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            #region LINQ Query Syntax + ToArray()

            var sortedDoubles =
                from d in doubles
                orderby d descending
                select d;

            // Примусове негайне виконання запиту та запис результатів у масив
            var doublesArray = sortedDoubles.ToArray();

            Console.WriteLine("Query Syntax + ToArray — Кожен другий елемент масиву:");
            for (int d = 0; d < doublesArray.Length; d += 2)
            {
                Console.WriteLine("Індекс {0}: {1}", d, doublesArray[d]);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 54A: Матеріалізація результатів у масив за допомогою Method Syntax (OrderByDescending + ToArray).
        /// </summary>
        public void Linq54A()
        {
            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            #region LINQ Method Syntax + ToArray()

            var doublesArray = doubles.OrderByDescending(d => d).ToArray();

            Console.WriteLine();
            Console.WriteLine("****************************************");
            Console.WriteLine("Method Syntax + ToArray — Кожен другий елемент масиву:");
            for (int d = 0; d < doublesArray.Length; d += 2)
            {
                Console.WriteLine("Індекс {0}: {1}", d, doublesArray[d]);
            }

            #endregion
        }
    }
}
