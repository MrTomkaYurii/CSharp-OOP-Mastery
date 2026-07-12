using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Distinct після проєкції (Унікальні значення властивостей)
    // ==============================================================================================
    // Категорія: Set Operators (Оператори над множинами)
    // Призначення: Отримання списку унікальних категорій, статусів або міст із великої бази об'єктів.
    //
    // Принцип роботи:
    //   - Спочатку оператор Select проєктує сутність у значення конкретної властивості (Category).
    //   - Потім метод Distinct() відфільтровує всі повтори, залишаючи кожну категорію лише один раз.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesSetOperators
    {
        /// <summary>
        /// Приклад 47: Пошук унікальних назв категорій товарів за допомогою Query Syntax + Distinct.
        /// </summary>
        public void Linq47()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Query Syntax + Distinct()

            var categoryNames = (
                from prod in products
                select prod.Category)
                .Distinct();

            Console.WriteLine("Query Syntax — Унікальні назви категорій:");
            foreach (var n in categoryNames)
            {
                Console.WriteLine("  " + n);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 47A: Пошук унікальних назв категорій за допомогою Method Syntax (Select + Distinct).
        /// </summary>
        public void Linq47A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region LINQ Method Syntax (Select + Distinct)

            var categoryNames = products.Select(prod => prod.Category).Distinct();

            Console.WriteLine();
            Console.WriteLine("*********************************");
            Console.WriteLine("Method Syntax — Унікальні назви категорій:");
            foreach (var n in categoryNames)
            {
                Console.WriteLine("  " + n);
            }

            #endregion
        }
    }
}
