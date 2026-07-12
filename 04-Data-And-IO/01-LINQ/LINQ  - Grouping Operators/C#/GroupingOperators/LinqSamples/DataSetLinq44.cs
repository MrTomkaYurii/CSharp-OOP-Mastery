using System;
using System.Data;
using System.Linq;

namespace GroupingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор GroupBy з користувацьким компаратором еквівалентності (IEqualityComparer)
    // ==============================================================================================
    // Категорія: Grouping Operators (Оператори групування)
    // Призначення: Групування елементів за нестандартним критерієм рівності
    //              (наприклад, анаграми — слова, що складаються з однакових літер у різному порядку: "from" і "form").
    //
    // Принцип роботи:
    //   - Метод GroupBy приймає екземпляр IEqualityComparer<TKey>.
    //   - Компаратор перевизначає методи Equals(x, y) та GetHashCode(obj).
    //   - Ключі, які мають однаковий хеш-код і повертають Equals == true, потрапляють в одну групу.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesGroupingOperators
    {
        /// <summary>
        /// Приклад 44: Групування слів-анаграм за допомогою GroupBy та кастомного AnagramEqualityComparer.
        /// </summary>
        public void DataSetLinq44()
        {
            var anagrams = testDS.Tables["Anagrams"].AsEnumerable();

            #region LINQ Method Syntax (GroupBy з IEqualityComparer)

            var orderGroups = anagrams.GroupBy(
                w => w.Field<string>("anagram").Trim(),
                new AnagramEqualityComparer());

            Console.WriteLine("Method Syntax — Групування слів-анаграм:");
            foreach (var g in orderGroups)
            {
                Console.WriteLine("Ключ групи: {0}", g.Key);
                foreach (var w in g)
                {
                    Console.WriteLine("\t" + w.Field<string>("anagram"));
                }
            }

            #endregion
        }
    }
}
