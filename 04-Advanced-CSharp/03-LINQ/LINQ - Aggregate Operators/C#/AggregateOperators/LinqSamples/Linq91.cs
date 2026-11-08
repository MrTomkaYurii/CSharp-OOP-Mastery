using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Average у згрупованих послідовностях
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори) / Grouping
    // Призначення: Обчислює середнє арифметичне значення числової властивості в межах кожної групи.
    //
    // Принцип роботи:
    //   - Застосовується до кожної підгрупи prodGroup.
    //   - Обчислює prodGroup.Average(p => p.UnitPrice) — середню вартість товарів у категорії.
    //   - Еквівалентний SQL-запиту: SELECT Category, AVG(UnitPrice) FROM Products GROUP BY Category.
    //
    // Тип виконання:
    //   - Відкладене виконання загального запиту з негайним обчисленням Average для кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 91: Групування товарів за категорією та обчислення середньої ціни товарів у кожній категорії через Average (Query Syntax).
        /// </summary>
        public void Linq91()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Групуємо за категорією та обчислюємо середню ціну товару всередині кожної категорії
            var categories =
                from prod in products
                group prod by prod.Category into prodGroup
                select new { Category = prodGroup.Key, AveragePrice = prodGroup.Average(p => p.UnitPrice) };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            ObjectDumper.Write(categories);
        }

        /// <summary>
        /// Приклад 91A: Обчислення середньої ціни товарів у категоріях за допомогою методів розширення (Method Syntax).
        /// </summary>
        public void Linq91A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var categories = products
                .GroupBy(prod => prod.Category)
                .Select(prodGroup => new {
                    Category = prodGroup.Key,
                    AveragePrice = prodGroup.Average(p => p.UnitPrice)
                });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(categories);
        }
    }
}
