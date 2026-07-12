using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Sum у згрупованих послідовностях
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори) / Grouping
    // Призначення: Обчислює сумарне значення числової властивості для елементів кожної групи.
    //
    // Принцип роботи:
    //   - Застосовується до об'єкта групи prodGroup, що представляє всі елементи в категорії.
    //   - Виклик prodGroup.Sum(p => p.UnitsInStock) обчислює сумарний залишок товарів на складі для цієї категорії.
    //   - Еквівалентний SQL-конструкції: SELECT Category, SUM(UnitsInStock) FROM Products GROUP BY Category.
    //
    // Тип виконання:
    //   - Відкладене виконання запиту з негайним агрегуванням сум всередині кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 80: Обчислення загальної кількості одиниць товарів на складі для кожної категорії через Sum (Query Syntax).
        /// </summary>
        public void Linq80()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Групуємо товари за категорією та обчислюємо суму UnitsInStock у кожній групі
            var categories =
                from prod in products
                group prod by prod.Category into prodGroup
                select new { Category = prodGroup.Key, TotalUnitsInStock = prodGroup.Sum(p => p.UnitsInStock) };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            ObjectDumper.Write(categories);
        }

        /// <summary>
        /// Приклад 80A: Обчислення сум запасів товарів за категоріями за допомогою GroupBy(...).Select(...) (Method Syntax).
        /// </summary>
        public void Linq80A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var categories = products
                .GroupBy(prod => prod.Category)
                .Select(prodGroup => new { Category = prodGroup.Key, TotalUnitsInStock = prodGroup.Sum(p => p.UnitsInStock) });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(categories);
        }
    }
}
