using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Select з індексом (Indexed Select)
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції)
    // Призначення: Проєктує кожен елемент послідовності, надаючи доступ до його порядкового індексу (0, 1, 2...).
    //
    // Принцип роботи:
    //   - Перевантаження: Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector).
    //   - Параметр int представляє 0-базований індекс елемента в послідовності.
    //
    // Важлива особливість:
    //   - Індексований Select існує ТІЛЬКИ в Method Syntax (Lambda).
    //   - У декларативному Query Syntax аналог відсутній.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 12: Перевірка, чи збігається числове значення елемента з його порядковим індексом у масиві.
        /// Зверніть увагу: приклад реалізується лише через Method Syntax (Lambda).
        /// </summary>
        public void DataSetLinq12()
        {
            var numbers = testDS.Tables["Numbers"].AsEnumerable();

            #region LINQ Method Syntax (Indexed Select)

            // Проєкція: перевіряємо умову num == index для кожного елемента
            var numsInPlace = numbers.Select((num, index) => new
            {
                Num = num.Field<int>("number"),
                InPlace = (num.Field<int>("number") == index)
            });

            Console.WriteLine("Число: На своєму місці (значення == індекс)?");
            foreach (var n in numsInPlace)
            {
                Console.WriteLine("{0}: {1}", n.Num, n.InPlace);
            }

            #endregion
        }
    }
}
