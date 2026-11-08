using System;
using System.Data;
using System.Linq;

namespace GroupingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор GroupBy за символьним ключем (Перша літера слова)
    // ==============================================================================================
    // Категорія: Grouping Operators (Оператори групування)
    // Призначення: Розбиття текстової колекції на словникові групи за початковою літерою (char).
    //
    // Принцип роботи:
    //   - Селектор ключа: word[0] повертає символ char, який слугує ключем групи (g.Key).
    //   - Усі слова, що починаються з однакової літери, потрапляють у відповідну групу g.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesGroupingOperators
    {
        /// <summary>
        /// Приклад 41: Групування слів за їхньою першою літерою за допомогою Query Syntax.
        /// </summary>
        public void DataSetLinq41()
        {
            var words4 = testDS.Tables["Words4"].AsEnumerable();

            #region LINQ Query Syntax (group by word[0])

            var wordGroups =
                from w in words4
                group w by w.Field<string>("word")[0] into g
                select new { FirstLetter = g.Key, Words = g };

            Console.WriteLine("Query Syntax — Слова, згруповані за першою літерою:");
            foreach (var g in wordGroups)
            {
                Console.WriteLine("Слова, що починаються на літеру '{0}':", g.FirstLetter);
                foreach (var w in g.Words)
                {
                    Console.WriteLine("  " + w.Field<string>("word"));
                }
            }

            #endregion
        }

        /// <summary>
        /// Приклад 41A: Групування слів за першою літерою за допомогою Method Syntax (GroupBy).
        /// </summary>
        public void DataSetLinq41A()
        {
            var words4 = testDS.Tables["Words4"].AsEnumerable();

            #region LINQ Method Syntax (GroupBy)

            var wordGroups = words4
                .GroupBy(w => w.Field<string>("word")[0])
                .Select(g => new { FirstLetter = g.Key, Words = g });

            Console.WriteLine();
            Console.WriteLine("********************************");
            Console.WriteLine("Method Syntax — Слова, згруповані за першою літерою:");
            foreach (var g in wordGroups)
            {
                Console.WriteLine("Слова, що починаються на літеру '{0}':", g.FirstLetter);
                foreach (var w in g.Words)
                {
                    Console.WriteLine("  " + w.Field<string>("word"));
                }
            }

            #endregion
        }
    }
}
