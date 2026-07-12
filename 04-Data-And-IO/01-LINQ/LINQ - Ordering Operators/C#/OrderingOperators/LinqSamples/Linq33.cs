using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Сортування складних об'єктів за спаданням числової властивості
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Сортування списку сутностей (Product) за значенням кількісної властивості
    //              (UnitsInStock — кількість одиниць на складі) від максимуму до мінімуму.
    //
    // Принцип роботи:
    //   - Селектор ключа prod => prod.UnitsInStock повертає ціле число.
    //   - Елементи з більшим залишком розташовуються першими.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 33: Сортування товарів за спаданням кількості на складі (UnitsInStock) з обмеженням Take(10) (Query Syntax).
        /// </summary>
        public void Linq33()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Query Syntax (orderby UnitsInStock descending)

            var sortedProducts = (
                from prod in products
                orderby prod.UnitsInStock descending
                select prod)
                .Take(10);

            Console.WriteLine("Query Syntax — Товари за залишком на складі (спадання, топ-10):");
            ObjectDumper.Write(sortedProducts);

            #endregion
        }

        /// <summary>
        /// Приклад 33A: Сортування товарів за спаданням залишку за допомогою Method Syntax (OrderByDescending).
        /// </summary>
        public void Linq33A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Method Syntax (OrderByDescending + Take)

            var sortedProducts = products.OrderByDescending(prod => prod.UnitsInStock).Take(10);

            Console.WriteLine();
            Console.WriteLine("********************************");
            Console.WriteLine("Method Syntax — Товари за залишком на складі (спадання, топ-10):");
            ObjectDumper.Write(sortedProducts);

            #endregion
        }
    }
}
