using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор ThenByDescending (Вторинне сортування за спаданням)
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Вторинне сортування за спаданням значень вторинного ключа всередині вже
    //              відсортованих первинних груп.
    //
    // Принцип роботи:
    //   - Query Syntax: orderby key1 [ascending], key2 descending.
    //   - Method Syntax: source.OrderBy(key1).ThenByDescending(key2).
    //   - Дозволяє поєднувати прямий порядок для першого ключа (наприклад, категорія від A до Z)
    //     та зворотний для другого (наприклад, ціна від найдорожчої до найдешевшої).
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 37: Сортування товарів спочатку за категорією (зростання), а потім за ціною
        /// від найдорожчого до найдешевшого (Query Syntax).
        /// </summary>
        public void Linq37()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Query Syntax (orderby Category, UnitPrice descending)

            var sortedProducts =
                from prod in products
                orderby prod.Category, prod.UnitPrice descending
                select prod;

            Console.WriteLine("Query Syntax — Товари: за категорією (А-Я), потім за ціною (спадання):");
            ObjectDumper.Write(sortedProducts.Take(10));

            #endregion
        }

        /// <summary>
        /// Приклад 37A: Багаторівневе сортування за категорією та ціною за допомогою Method Syntax (OrderBy + ThenByDescending).
        /// </summary>
        public void Linq37A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Method Syntax (OrderBy + ThenByDescending)

            var sortedProducts = products
                .OrderBy(prod => prod.Category)
                .ThenByDescending(prod => prod.UnitPrice)
                .Take(10);

            Console.WriteLine();
            Console.WriteLine("************************************");
            Console.WriteLine("Method Syntax — Товари: OrderBy(Category).ThenByDescending(UnitPrice):");
            ObjectDumper.Write(sortedProducts);

            #endregion
        }
    }
}
