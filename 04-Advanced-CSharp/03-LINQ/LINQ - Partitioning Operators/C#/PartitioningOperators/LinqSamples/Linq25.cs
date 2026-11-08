using System;
using System.Linq;

namespace PartitioningOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор TakeWhile з індексом (Indexed TakeWhile)
    // ==============================================================================================
    // Категорія: Partitioning Operators (Оператори розбиття / секціонування)
    // Призначення: Повертає елементи з початку послідовності, доки елемент та його індекс задовольняють умову.
    //
    // Принцип роботи:
    //   - Перевантаження: TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate).
    //   - Предикат отримує сам елемент (n) та його 0-базований індекс (index).
    //   - Як тільки пара (n, index) повертає false, перебір негайно зупиняється.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming, Short-circuiting).
    // ==============================================================================================

    public partial class LinqSamplesPartitioningOperators
    {
        /// <summary>
        /// Приклад 25: Вибірка елементів із початку масиву, доки значення елемента не стане меншим за його індекс (n >= index).
        /// </summary>
        public void Linq25()
        {
            // Масив: { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
            // Індекс 0: 5 >= 0 (так)
            // Індекс 1: 4 >= 1 (так)
            // Індекс 2: 1 >= 2 (НІ! 1 < 2, умова n >= index хибна -> зупинка)
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region LINQ Method Syntax (Indexed TakeWhile)

            var firstSmallNumbers = numbers.TakeWhile((n, index) => n >= index);

            Console.WriteLine("Перші числа, які не менші за свій індекс у масиві:");
            foreach (var n in firstSmallNumbers)
            {
                Console.WriteLine(n);
            }

            #endregion
        }
    }
}
