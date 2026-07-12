using System;
using System.Linq;

namespace RestrictionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Where з індексом (Indexed Where)
    // ==============================================================================================
    // Категорія: Restriction Operators (Оператори фільтрації)
    // Призначення: Фільтрує елементи послідовності з урахуванням їхньої порядкової позиції (індексу).
    //
    // Принцип роботи:
    //   - Використовує перевантаження: Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate).
    //   - Другий аргумент предиката (int index) представляє 0-базований індекс поточного елемента.
    //
    // Важлива особливість:
    //   - Індексований Where існує ТІЛЬКИ в синтаксисі методів (Method / Fluent Syntax).
    //   - У декларативному синтаксисі запитів (Query Syntax) прямий аналог відсутній.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesRestrictionOperators
    {
        /// <summary>
        /// Приклад 5: Індексована фільтрація. Повертає назви цифр, довжина яких менша за їхнє числове значення (індекс).
        /// Зверніть увагу: цей приклад неможливо виразити через Query Syntax.
        /// </summary>
        public void Linq5()
        {
            // Вхідні дані: масив текстових назв цифр від 0 до 9
            // Індекс у масиві відповідає числовому значенню слова
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Method Syntax (Indexed Where)

            // Фільтрація: довжина слова (digit.Length) менша за його індекс у масиві (index)
            // Наприклад: "five" (довжина 4) < індекс 5 -> підходить!
            var shortDigits = digits.Where((digit, index) => digit.Length < index);

            Console.WriteLine("Lambda-вираз (індексований Where) — Слова, коротші за свій індекс:");
            Console.WriteLine();
            foreach (var d in shortDigits)
            {
                Console.WriteLine("Слово '{0}' коротше за його числове значення.", d);
            }

            #endregion
        }
    }
}
