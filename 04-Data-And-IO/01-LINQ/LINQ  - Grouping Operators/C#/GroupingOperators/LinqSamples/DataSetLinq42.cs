using System;
using System.Data;
using System.Linq;

namespace GroupingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор GroupBy за текстовою властивістю об'єкта (Категорія товару)
    // ==============================================================================================
    // Категорія: Grouping Operators (Оператори групування)
    // Призначення: Групування доменних сутностей за категорійною ознакою (Category)
    //              для подальшої агрегації або ієрархічного виведення.
    //
    // Принцип роботи:
    //   - group p by p.Category into g створює групу g з ключем g.Key (назва категорії).
    //   - Група містить перелік усіх товарів, що належать до цієї категорії.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesGroupingOperators
    {
        /// <summary>
        /// Приклад 42: Групування списку товарів за їхньою категорією (Query Syntax).
        /// </summary>
        public void DataSetLinq42()
        {
            var products = testDS.Tables["Products"].AsEnumerable();

            #region LINQ Query Syntax (group by Category)

            var productGroups =
                from p in products
                group p by p.Field<string>("Category") into g
                select new { Category = g.Key, Products = g };

            Console.WriteLine("Query Syntax — Товари, згруповані за категоріями:");
            foreach (var g in productGroups.Take(3))
            {
                Console.WriteLine("Категорія: {0}", g.Category);
                foreach (var w in g.Products.Take(3))
                {
                    Console.WriteLine("\t" + w.Field<string>("ProductName"));
                }
            }

            #endregion
        }

        /// <summary>
        /// Приклад 42A: Групування товарів за категорією за допомогою Method Syntax (GroupBy).
        /// </summary>
        public void DataSetLinq42A()
        {
            var products = testDS.Tables["Products"].AsEnumerable();

            #region LINQ Method Syntax (GroupBy)

            var productGroups = products
                .GroupBy(p => p.Field<string>("Category"))
                .Select(g => new { Category = g.Key, Products = g });

            Console.WriteLine();
            Console.WriteLine("*****************************");
            Console.WriteLine("Method Syntax — Товари, згруповані за категоріями:");
            foreach (var g in productGroups.Take(3))
            {
                Console.WriteLine("Категорія: {0}", g.Category);
                foreach (var w in g.Products.Take(3))
                {
                    Console.WriteLine("\t" + w.Field<string>("ProductName"));
                }
            }

            #endregion
        }
    }
}
