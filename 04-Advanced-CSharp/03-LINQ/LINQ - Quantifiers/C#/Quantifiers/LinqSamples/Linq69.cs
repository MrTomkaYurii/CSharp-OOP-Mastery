using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantifiers
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Any всередині згрупованих послідовностей
    // ==============================================================================================
    // Категорія: Quantifiers (Оператори-квантори) / Grouping
    // Призначення: Фільтрує згруповані дані (групи IGrouping), залишаючи лише ті групи,
    //              в яких хоча б один елемент відповідає предикату.
    //
    // Принцип роботи:
    //   - Застосовується до кожної підколекції (групи), утвореної оператором group by.
    //   - Дозволяє реалізувати логіку на кшталт SQL HAVING: відфільтрувати цілі групи за станом їхніх членів.
    //   - Завдяки short-circuiting перевірка групи зупиняється на першому збігу (наприклад, перший товар не на складі).
    //
    // Тип виконання:
    //   - Відкладене виконання загального запиту з негайною оцінкою Any для кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesQuantifiers
    {
        /// <summary>
        /// Приклад 69: Групування товарів за категоріями та фільтрація груп за допомогою Any (тільки категорії, де є товари не на складі) (Query Syntax).
        /// </summary>
        public void Linq69()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Orginal Linq with Query syntax

            // Групуємо за категорією та залишаємо лише групи, де є товар із UnitsInStock == 0
            var productGroups =
                from prod in products
                group prod by prod.Category into prodGroup
                where prodGroup.Any(p => p.UnitsInStock == 0)
                select new { Category = prodGroup.Key, Products = prodGroup };

            ObjectDumper.Write(productGroups.Take(1), 1); 

            #endregion
        }

        /// <summary>
        /// Приклад 69A: Реалізація групування з фільтрацією груп через Any за допомогою ланцюжка методів (Method Syntax).
        /// </summary>
        public void Linq69A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            // Функціональний конвеєр: GroupBy -> Where (Any) -> Select
            var productGroups = products
                .GroupBy(prod => prod.Category)
                .Where(prodGroup => prodGroup.Any(p => p.UnitsInStock == 0))
                .Select(prodGroup => new { Category = prodGroup.Key, Products = prodGroup });

            Console.WriteLine();
            Console.WriteLine("***********************************");
            Console.Write("Linq with Lambda.");
            ObjectDumper.Write(productGroups.Take(1), 1);

            #endregion
        }
    }
}
