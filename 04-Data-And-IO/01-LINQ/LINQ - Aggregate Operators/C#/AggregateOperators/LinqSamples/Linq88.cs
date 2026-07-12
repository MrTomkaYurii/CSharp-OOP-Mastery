using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Пошук елементів із максимальним значенням у групі (let + Max + Where)
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори) / Advanced Grouping
    // Призначення: Знаходить усі сутності (об'єкти), які мають максимальне значення в кожній групі.
    //
    // Принцип роботи:
    //   - Обчислює максимальну ціну в групі через Max() та фіксує її в змінній через let:
    //     let maxPrice = prodGroup.Max(p => p.UnitPrice).
    //   - Фільтрує товари групи за цією максимальною ціною: Where(p => p.UnitPrice == maxPrice).
    //   - Якщо в категорії кілька товарів ділять максимальну ціну між собою, усі вони будуть включені.
    //
    // Тип виконання:
    //   - Відкладене виконання запиту з негайним агрегуванням Max для кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 88: Знаходження найдорожчих товарів у кожній категорії за допомогою let та Max (Query Syntax).
        /// </summary>
        public void Linq88()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Групуємо, обчислюємо максимальну ціну через let та відбираємо товари з цією ціною
            var categories =
                from prod in products
                group prod by prod.Category into prodGroup
                let maxPrice = prodGroup.Max(p => p.UnitPrice)
                select new { Category = prodGroup.Key, MostExpensiveProducts = prodGroup.Where(p => p.UnitPrice == maxPrice) };

            #endregion

            Console.WriteLine("Original 101 Linq example.");
            ObjectDumper.Write(categories.Take(5), 1);
        }

        /// <summary>
        /// Приклад 88A: Знаходження найдорожчих товарів у кожній категорії за допомогою двох Select (Method Syntax).
        /// </summary>
        public void Linq88A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var categories = products
                .GroupBy(prod => prod.Category)
                .Select(prodGroup => new { prodGroup, maxPrice = prodGroup.Max(p => p.UnitPrice) })
                .Select(pG => new {
                    Category = pG.prodGroup.Key,
                    MostExpensiveProducts = pG.prodGroup.Where(p => p.UnitPrice == pG.maxPrice)
                });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(categories.Take(5), 1);
        }
    }
}
