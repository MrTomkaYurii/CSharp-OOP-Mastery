using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Union для символьних даних (Об'єднання проєкцій)
    // ==============================================================================================
    // Категорія: Set Operators (Оператори над множинами)
    // Призначення: Об'єднання результатів двох незалежних проєкцій в єдиний алфавітний набір унікальних символів.
    //
    // Принцип роботи:
    //   - З двох різних колекцій (товари та клієнти) витягуються перші літери назв (char).
    //   - Метод Union() повертає повну множину всіх унікальних початкових літер без дублювання.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesSetOperators
    {
        /// <summary>
        /// Приклад 49: Отримання унікальних початкових літер назв товарів та компаній-клієнтів (Query + Union).
        /// </summary>
        public void Linq49()
        {
            List<Products.Product> products = Products.GetProductList();
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Query Syntax + Union()

            var productFirstChars =
                from prod in products
                select prod.ProductName[0];

            var customerFirstChars =
                from cust in customers
                select cust.CompanyName[0];

            var uniqueFirstChars = productFirstChars.Union(customerFirstChars);

            Console.WriteLine("Унікальні перші літери назв товарів та клієнтів:");
            foreach (var ch in uniqueFirstChars.Take(10))
            {
                Console.WriteLine("  " + ch);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 49A: Аналогічне об'єднання за допомогою чистого Method Syntax (Select + Union).
        /// </summary>
        public void Linq49A()
        {
            List<Products.Product> products = Products.GetProductList();
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Method Syntax (Select + Union)

            var uniqueFirstChars = products
                .Select(prod => prod.ProductName[0])
                .Union(customers.Select(cust => cust.CompanyName[0]));

            Console.WriteLine();
            Console.WriteLine("********************************");
            Console.WriteLine("Method Syntax — Унікальні перші літери назв:");
            foreach (var ch in uniqueFirstChars.Take(10))
            {
                Console.WriteLine("  " + ch);
            }

            #endregion
        }
    }
}
