using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор OrderByDescending (Сортування за спаданням / Descending)
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Впорядковує елементи послідовності за спаданням значень ключа.
    //
    // Принцип роботи:
    //   - У Query Syntax використовується ключове слово 'descending' після виразу сортування.
    //   - У Method Syntax викликається метод OrderByDescending(keySelector).
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 30: Сортування списку товарів за назвою (ProductName) у зворотному алфавітному порядку (Query Syntax).
        /// </summary>
        public void Linq30()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Query Syntax (orderby ... descending)

            var sortedProducts =
                from prod in products
                orderby prod.ProductName descending
                select prod;

            Console.WriteLine("Query Syntax — Товари, відсортовані за назвою (спадання):");
            ObjectDumper.Write(sortedProducts.Take(10));

            #endregion
        }

        /// <summary>
        /// Приклад 30A: Сортування товарів за назвою у зворотному порядку за допомогою Method Syntax (OrderByDescending).
        /// </summary>
        public void Linq30A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Method Syntax (OrderByDescending)

            var sortedProducts = products.OrderByDescending(prod => prod.ProductName);

            Console.WriteLine();
            Console.WriteLine("****************************************");
            Console.WriteLine("Method Syntax (OrderByDescending) — Товари у зворотному порядку:");
            ObjectDumper.Write(sortedProducts.Take(10));

            #endregion
        }
    }
}
