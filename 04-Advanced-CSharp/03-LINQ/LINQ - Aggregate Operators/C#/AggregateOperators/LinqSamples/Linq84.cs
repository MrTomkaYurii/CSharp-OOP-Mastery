using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Пошук елементів із мінімальним значенням у групі (let + Min + Where)
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори) / Advanced Grouping
    // Призначення: Знаходить не просто числове мінімальне значення, а самі екземпляри об'єктів,
    //              що володіють цим мінімальним значенням у кожній групі.
    //
    // Принцип роботи:
    //   - Обчислює мінімальну ціну групи за допомогою Min() та зберігає її у проміжну змінну через let:
    //     let minPrice = prodGroup.Min(p => p.UnitPrice).
    //   - Потім фільтрує елементи групи за допомогою Where(p => p.UnitPrice == minPrice).
    //   - Якщо кілька товарів мають однакову мінімальну ціну, вони всі потраплять у результат.
    //   - У .NET 6+ для вибору одного об'єкта додано зручний оператор MinBy, але даний патерн
    //     дозволяє отримати ВСІ товари з мінімальною ціною (при нічиїх/дублікатах цін).
    //
    // Тип виконання:
    //   - Відкладене виконання загального запиту з негайним обчисленням Min для кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 84: Знаходження товарів із найнижчою ціною в кожній категорії за допомогою let та Min (Query Syntax).
        /// </summary>
        public void Linq84()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Групуємо, обчислюємо мінімальну ціну через let та вибираємо товари з цією ціною
            var categories =
                from prod in products
                group prod by prod.Category into prodGroup
                let minPrice = prodGroup.Min(p => p.UnitPrice)
                select new { Category = prodGroup.Key, CheapestProducts = prodGroup.Where(p => p.UnitPrice == minPrice) };

            #endregion

            Console.WriteLine("Original 101 Linq example.");
            ObjectDumper.Write(categories.Take(5), 1);
        }

        /// <summary>
        /// Приклад 84A: Знаходження найдешевших товарів у категоріях за допомогою двохетапного Select (Method Syntax).
        /// </summary>
        public void Linq84A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            var categories = products
                .GroupBy(prod => prod.Category)
                .Select(prodGroup => new { prodGroup, minPrice = prodGroup.Min(p => p.UnitPrice) })
                .Select(pG => new {
                    Category = pG.prodGroup.Key,
                    CheapestProducts = pG.prodGroup.Where(p => p.UnitPrice == pG.minPrice)
                });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(categories.Take(5), 1);
        }
    }
}
