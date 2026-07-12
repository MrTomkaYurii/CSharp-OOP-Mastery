using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Max у згрупованих послідовностях
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори) / Grouping
    // Призначення: Знаходить найвище значення певної властивості (наприклад, найвищу ціну) у кожній групі.
    //
    // Принцип роботи:
    //   - Обчислює максимум для кожної підколекції IGrouping окремо: prodGroup.Max(p => p.UnitPrice).
    //   - Еквівалентний SQL-запиту: SELECT Category, MAX(UnitPrice) FROM Products GROUP BY Category.
    //
    // Тип виконання:
    //   - Відкладене виконання запиту з негайним обчисленням Max для кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 87: Групування товарів за категорією та пошук найвищої ціни в кожній категорії через Max (Query Syntax).
        /// </summary>
        public void Linq87()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Групуємо за категорією та знаходимо максимальну ціну в межах кожної категорії
            var categories =
                from prod in products
                group prod by prod.Category into prodGroup
                select new { Category = prodGroup.Key, MostExpensivePrice = prodGroup.Max(p => p.UnitPrice) };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            ObjectDumper.Write(categories);
        }

        /// <summary>
        /// Приклад 87A: Обчислення найвищої ціни товару в кожній категорії за допомогою методів розширення (Method Syntax).
        /// </summary>
        public void Linq87A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var categories = products
                .GroupBy(prod => prod.Category)
                .Select(prodGroup => new {
                    Category = prodGroup.Key,
                    MostExpensivePrice = prodGroup.Max(p => p.UnitPrice)
                });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(categories);
        }
    }
}
