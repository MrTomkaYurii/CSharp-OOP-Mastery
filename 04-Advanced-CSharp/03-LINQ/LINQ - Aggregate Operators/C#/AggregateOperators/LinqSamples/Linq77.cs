using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Count на згрупованих послідовностях
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори) / Grouping
    // Призначення: Підраховує кількість елементів у кожній утвореній групі після групування (GroupBy).
    //
    // Принцип роботи:
    //   - Об'єкт групи реалізує інтерфейс IGrouping<TKey, TElement>, який є спадкоємцем IEnumerable<TElement>.
    //   - Виклик prodGroup.Count() обчислює розмір групи.
    //   - Еквівалентний SQL-конструкції: SELECT Category, COUNT(*) FROM Products GROUP BY Category.
    //
    // Тип виконання:
    //   - Відкладене виконання запиту з негайним агрегуванням кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 77: Групування товарів за категоріями та підрахунок кількості товарів у кожній категорії (Query Syntax).
        /// </summary>
        public void Linq77()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Групуємо за категорією та обчислюємо кількість елементів у кожній групі
            var categoryCounts =
                from prod in products
                group prod by prod.Category into prodGroup
                select new { Category = prodGroup.Key, ProductCount = prodGroup.Count() };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            ObjectDumper.Write(categoryCounts);
        }

        /// <summary>
        /// Приклад 77A: Групування та агрегування кількості через ланцюжок GroupBy(...).Select(...) (Method Syntax).
        /// </summary>
        public void Linq77A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var categoryCounts = products
                .GroupBy(prod => prod.Category)  
                .Select(prodGroup => new {
                    Category = prodGroup.Key,
                    ProductCount = prodGroup.Count()
                });
            
            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(categoryCounts);
        }
    }
}
