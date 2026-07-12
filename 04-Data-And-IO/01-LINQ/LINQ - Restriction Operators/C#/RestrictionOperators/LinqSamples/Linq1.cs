using System;
using System.ComponentModel;
using System.Linq;

namespace RestrictionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Where (Фільтрація / Обмеження)
    // ==============================================================================================
    // Категорія: Restriction Operators (Оператори фільтрації)
    // Призначення: Відбирає елементи з послідовності, які задовольняють задану умову (предикат).
    //
    // Принцип роботи:
    //   - Приймає делегат-предикат Func<TSource, bool>.
    //   - Перевіряє кожен елемент колекції. Якщо предикат повертає true, елемент включається
    //     у вихідну послідовність, інакше — пропускається.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming). Елементи оцінюються по одному під час
    //     ітерації (наприклад, у циклі foreach або виклику ToList()).
    //
    // Синтаксис:
    //   - Query Syntax (синтаксис запиту):
    //       from item in source
    //       where item < 5
    //       select item;
    //   - Method Syntax (синтаксис методів / лямбда):
    //       source.Where(item => item < 5);
    // ==============================================================================================

    public partial class LinqSamplesRestrictionOperators
    {
        /// <summary>
        /// Приклад 1: Вибірка всіх чисел, менших за 5, за допомогою Query Syntax.
        /// Демонструє використання речення 'where' для базової фільтрації масиву.
        /// </summary>
        public void Linq1()
        {
            // Вхідні дані: масив цілих чисел
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region LINQ Query Syntax

            // Формуємо LINQ-запит для фільтрації: залишаємо лише числа, менші за 5
            var lowNums =
                from num in numbers
                where num < 5
                select num;

            Console.WriteLine("Результат з використанням Query Syntax (числа < 5):");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 1A: Вибірка всіх чисел, менших за 5, за допомогою Method Syntax (Fluent API / Lambda).
        /// Демонструє використання методу розширення Where().
        /// </summary>
        public void Linq1A()
        {
            // Вхідні дані: масив цілих чисел
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region LINQ Method Syntax (Lambda)

            // Фільтрація за допомогою методу розширення Where та лямбда-виразу
            var lowNums = numbers.Where(num => num < 5);

            Console.WriteLine();
            Console.WriteLine("Результат з використанням Lambda-виразу (числа < 5):");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }

            #endregion
        }
    }
}
