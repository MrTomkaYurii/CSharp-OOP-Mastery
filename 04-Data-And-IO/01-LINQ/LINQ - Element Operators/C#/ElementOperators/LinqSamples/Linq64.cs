using System;
using System.Linq;

namespace ElementOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор ElementAt (Отримання елемента за порядковим індексом)
    // ==============================================================================================
    // Категорія: Element Operators (Оператори елементів)
    // Призначення: Повертає елемент за вказаним індексом у послідовності (нумерація з нуля, 0-based).
    //
    // Принцип роботи:
    //   - Якщо колекція реалізує IList<T>, доступ виконується миттєво за O(1).
    //   - Для довільного IEnumerable<T> виконується лінійний прохід до потрібного індексу за O(N).
    //   - Якщо індекс є від'ємним або більшим/рівним кількості елементів, викидає ArgumentOutOfRangeException.
    //   - Для безпечного доступу існує парний метод ElementAtOrDefault(index).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesElementOperators
    {
        /// <summary>
        /// Приклад 64: Отримання другого числа, більшого за 5 (індекс 1), з масиву чисел за допомогою ElementAt (Query Syntax).
        /// </summary>
        public void Linq64()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region Make Sure to try yourself before looking at the code

            // Отримуємо друге число більше за 5 (індекс 1, бо нумерація починається з 0)
            int fourthLowNum = (
                from num in numbers
                where num > 5
                select num)
                .ElementAt(1);

            Console.WriteLine("Second number > 5: {0}", fourthLowNum);

            #endregion
        }

        /// <summary>
        /// Приклад 64A: Отримання другого елемента за індексом 1 через Where(...).ElementAt(1) (Method Syntax).
        /// </summary>
        public void Linq64A()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            // Лямбда-синтаксис: фільтрація та вибірка за позицією
            int fourthLowNum = numbers.Where(num => num > 5).ElementAt(1);

            Console.WriteLine();
            Console.WriteLine("**********************************");
            Console.WriteLine("Linq with Lambda");
            Console.WriteLine("Second number > 5: {0}", fourthLowNum);

            #endregion
        }
    }
}
