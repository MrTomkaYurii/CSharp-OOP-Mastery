using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Min у згрупованих послідовностях
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори) / Grouping
    // Призначення: Знаходить мінімальне значення властивості (наприклад, ціни) серед елементів кожної групи.
    //
    // Принцип роботи:
    //   - Застосовується до кожної підгрупи prodGroup.
    //   - Обчислює мінімальну ціну (prodGroup.Min(p => p.UnitPrice)) всередині категорії товарів.
    //   - Еквівалентний SQL-запиту: SELECT Category, MIN(UnitPrice) FROM Products GROUP BY Category.
    //
    // Тип виконання:
    //   - Відкладене виконання запиту з негайним обчисленням мінімуму для кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 83: Групування товарів за категорією та пошук найнижчої ціни в кожній категорії через Min (Query Syntax).
        /// </summary>
        public void Linq83()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Групуємо за категорією та обчислюємо мінімальну ціну для кожної категорії
            var categories =
                from prod in products
                group prod by prod.Category into prodGroup
                select new { Category = prodGroup.Key, CheapestPrice = prodGroup.Min(p => p.UnitPrice) };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            ObjectDumper.Write(categories);
        }

        /// <summary>
        /// Приклад 83A: Обчислення мінімальної ціни товару в кожній категорії за допомогою методів розширення (Method Syntax).
        /// </summary>
        public void Linq83A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var categories = products
                .GroupBy(prod => prod.Category) 
                .Select(prodGroup => new {
                    Category = prodGroup.Key,
                    CheapestPrice = prodGroup.Min(p => p.UnitPrice)
                });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(categories);
        }
    }
}
