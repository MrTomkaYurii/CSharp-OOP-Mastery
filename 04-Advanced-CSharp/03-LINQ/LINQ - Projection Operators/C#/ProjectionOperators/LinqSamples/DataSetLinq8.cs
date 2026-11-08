using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Select — Трансформація на основі зовнішніх даних
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції)
    // Призначення: Перетворення значень одного типу в інший шляхом індексування зовнішнього масиву
    //              або словника (наприклад, числа в текстові назви).
    //
    // Принцип роботи:
    //   - Значення поточного елемента (число від 0 до 9) використовується як індекс у зовнішньому масиві рядків.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 8: Перетворення послідовності чисел на рядкове текстове представлення ("zero", "one"...)
        /// за допомогою Method Syntax.
        /// </summary>
        public void DataSetLinq8()
        {
            var numbers = testDS.Tables["Numbers"].AsEnumerable();
            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Method Syntax (Lambda)

            // Проєкція: беремо число та отримуємо відповідне словесне значення з масиву
            var textNums = numbers.Select(p => strings[p.Field<int>("number")]);

            Console.WriteLine("Текстові назви чисел:");
            foreach (var s in textNums)
            {
                Console.WriteLine(s);
            }

            #endregion
        }
    }
}
