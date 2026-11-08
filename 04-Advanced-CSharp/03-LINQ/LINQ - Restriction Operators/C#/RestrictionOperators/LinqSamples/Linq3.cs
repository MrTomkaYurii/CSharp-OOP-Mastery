using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestrictionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Where — Складені логічні умови (Logical AND/OR)
    // ==============================================================================================
    // Категорія: Restriction Operators (Оператори фільтрації)
    // Призначення: Фільтрація елементів за кількома критеріями одночасно за допомогою логічних
    //              операторів (&&, ||, !).
    //
    // Принцип роботи:
    //   - Предикат обчислює вираз із декількома умовами.
    //   - У випадку оператора '&&' (логічне І) використовується скорочена оцінка (short-circuit evaluation):
    //     якщо перша умова false, друга навіть не обчислюється.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesRestrictionOperators
    {
        /// <summary>
        /// Приклад 3: Пошук товарів, які є в наявності (UnitsInStock > 0) ТА коштують більше 3.00,
        /// за допомогою Query Syntax.
        /// </summary>
        public void Linq3()
        {
            // Вхідні дані: каталог товарів
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Query Syntax

            // Фільтрація за двома критеріями одночасно за допомогою оператора '&&'
            var expensiveInStockProducts =
                from prod in products
                where prod.UnitsInStock > 0 && prod.UnitPrice > 3.00M
                select prod;

            Console.WriteLine("Query Syntax — Товари в наявності дорожчі за 3.00:");
            Console.WriteLine();
            foreach (var product in expensiveInStockProducts)
            {
                Console.WriteLine("{0} є в наявності і коштує {1:C}.", product.ProductName, product.UnitPrice);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 3A: Пошук товарів у наявності вартістю понад 3.00 за допомогою Method Syntax.
        /// </summary>
        public void Linq3A()
        {
            // Вхідні дані: каталог товарів
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Method Syntax (Lambda)

            // Фільтрація списку товарів методом Where() з комбінованою лямбда-умовою
            var expensiveInStockProducts = products.Where(
                prod => prod.UnitsInStock > 0 &&
                        prod.UnitPrice > 3.00M
            );

            Console.WriteLine("************************************************************************");
            Console.WriteLine("Lambda-вираз — Товари в наявності дорожчі за 3.00:");
            Console.WriteLine();
            foreach (var product in expensiveInStockProducts)
            {
                Console.WriteLine("{0} є в наявності і коштує {1:C}.", product.ProductName, product.UnitPrice);
            }

            #endregion
        }
    }
}
