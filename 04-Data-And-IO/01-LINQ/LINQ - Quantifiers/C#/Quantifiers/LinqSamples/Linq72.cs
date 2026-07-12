using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantifiers
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор All у згрупованих послідовностях
    // ==============================================================================================
    // Категорія: Quantifiers (Оператори-квантори) / Grouping
    // Призначення: Фільтрує згруповані дані, відбираючи лише ті групи, де ВСІ елементи
    //              без винятку задовольняють задану умову.
    //
    // Принцип роботи:
    //   - Застосовується до кожної групи IGrouping.
    //   - Якщо хоча б один елемент у групі не відповідає умові, вся група відсікається.
    //   - Завдяки short-circuiting перевірка групи припиняється одразу при знаходженні першого порушення умови.
    //
    // Тип виконання:
    //   - Відкладене виконання загального запиту з негайною оцінкою All для кожної групи.
    // ==============================================================================================

    public partial class LinqSamplesQuantifiers
    {
        /// <summary>
        /// Приклад 72: Групування товарів за категорією та фільтрація через All (тільки категорії, де ВСІ товари є в наявності) (Query Syntax).
        /// </summary>
        public void Linq72()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Відбираємо тільки категорії, в яких кожен товар має UnitsInStock > 0
            var productGroups =
                from prod in products
                group prod by prod.Category into prodGroup
                where prodGroup.All(p => p.UnitsInStock > 0)
                select new { Category = prodGroup.Key, Products = prodGroup };

            #endregion

            ObjectDumper.Write(productGroups.Take(1), 1);
        }

        /// <summary>
        /// Приклад 72A: Групування та перевірка повної наявності через All за допомогою методів розширення (Method Syntax).
        /// </summary>
        public void Linq72A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            // Конвеєр методів: GroupBy -> Where(All) -> Select
            var productGroups = products
                .GroupBy(prod => prod.Category)
                .Where(prodGroup => prodGroup.All(p => p.UnitsInStock > 0))
                .Select(prodGroup => new { Category = prodGroup.Key, Products = prodGroup });

            #endregion

            Console.WriteLine();
            Console.WriteLine("*********************************");
            Console.WriteLine("With Lambda.");
            Console.WriteLine();
            ObjectDumper.Write(productGroups.Take(1), 1);
        }
    }
}
