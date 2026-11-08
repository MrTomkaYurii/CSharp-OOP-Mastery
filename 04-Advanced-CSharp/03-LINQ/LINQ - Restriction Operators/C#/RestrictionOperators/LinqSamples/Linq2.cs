using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestrictionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Where — Фільтрація об'єктів за властивостями
    // ==============================================================================================
    // Категорія: Restriction Operators (Оператори фільтрації)
    // Призначення: Фільтрує колекцію складних об'єктів на основі значення однієї з їхніх властивостей.
    //
    // Принцип роботи:
    //   - Предикат звертається до конкретного поля або властивості об'єкта (наприклад, prod.UnitsInStock == 0).
    //   - Об'єкти, для яких предикат повертає true, передаються далі в конвеєр обробки.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesRestrictionOperators
    {
        /// <summary>
        /// Приклад 2: Пошук товарів, яких немає на складі (UnitsInStock == 0), за допомогою Query Syntax.
        /// </summary>
        public void Linq2()
        {
            // Вхідні дані: список сутностей Product
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Query Syntax

            // Відбираємо товари із залишком 0 на складі
            var soldOutProducts =
                from prod in products
                where prod.UnitsInStock == 0
                select prod;

            Console.WriteLine("Query Syntax — Товари, яких немає в наявності:");
            Console.WriteLine();
            foreach (var product in soldOutProducts)
            {
                Console.WriteLine("{0} — немає в наявності!", product.ProductName);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 2A: Пошук товарів, яких немає на складі (UnitsInStock == 0), за допомогою Method Syntax.
        /// </summary>
        public void Linq2A()
        {
            // Вхідні дані: список сутностей Product
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Method Syntax (Lambda)

            // Фільтрація списку товарів за допомогою Where()
            var soldOutProducts = products.Where(prod => prod.UnitsInStock == 0);

            Console.WriteLine("******************************************");
            Console.WriteLine("Lambda-вираз — Товари, яких немає в наявності:");
            Console.WriteLine();
            foreach (var product in soldOutProducts)
            {
                Console.WriteLine("{0} — немає в наявності!", product.ProductName);
            }

            #endregion
        }
    }
}
