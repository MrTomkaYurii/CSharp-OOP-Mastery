using System;
using System.Data;
using System.Linq;

namespace GroupingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор GroupBy з виділеним селектором елементів (Element Selector)
    // ==============================================================================================
    // Категорія: Grouping Operators (Оператори групування)
    // Призначення: Одночасне групування елементів та трансформація значень, що зберігаються всередині груп.
    //
    // Принцип роботи:
    //   - Перевантаження GroupBy(keySelector, elementSelector, comparer):
    //     * keySelector визначає ключ для формування групи.
    //     * elementSelector трансформує кожен вхідний елемент перед додаванням до групи (наприклад, ToUpper()).
    //     * comparer визначає рівність ключів.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesGroupingOperators
    {
        /// <summary>
        /// Приклад 45: Групування анаграм із трансформацією слів у верхній регістр за допомогою elementSelector.
        /// </summary>
        public void DataSetLinq45()
        {
            var anagrams = testDS.Tables["Anagrams"].AsEnumerable();

            #region LINQ Method Syntax (GroupBy з elementSelector)

            // Групуємо за очищеним словом, перетворюємо елементи у UPPERCASE
            var orderGroups = anagrams.GroupBy(
                w => w.Field<string>("anagram").Trim(),
                a => a.Field<string>("anagram").ToUpper(),
                new AnagramEqualityComparer()
            );

            Console.WriteLine("Method Syntax — Групування анаграм із трансформацією у верхній регістр:");
            foreach (var g in orderGroups)
            {
                Console.WriteLine("Ключ групи: {0}", g.Key);
                foreach (var w in g)
                {
                    Console.WriteLine("\t" + w);
                }
            }

            #endregion
        }
    }
}
