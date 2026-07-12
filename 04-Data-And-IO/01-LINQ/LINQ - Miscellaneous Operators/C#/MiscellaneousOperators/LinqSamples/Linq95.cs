using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscellaneousOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Concat для проектованих послідовностей рядків
    // ==============================================================================================
    // Категорія: Miscellaneous Operators (Різні допоміжні оператори)
    // Призначення: Поєднує результати двох різних запитів вибірки імен в єдиний потік рядків.
    //
    // Принцип роботи:
    //   - Об'єднує проєкцію імен клієнтів (CompanyName) та назв товарів (ProductName).
    //   - Результатом є послідовність IEnumerable<string>, де спершу йдуть усі компанії, а потім усі назви товарів.
    //   - Підтримує відкладене виконання: читання з джерел відбувається лише під час ітерації foreach.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesMiscellaneousOperators
    {
        /// <summary>
        /// Приклад 95: Об'єднання назв компаній клієнтів та назв товарів в єдиний список за допомогою Concat (Query Syntax).
        /// </summary>
        public void Linq95()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var customerNames =
                from cust in customers
                select cust.CompanyName;
            var productNames =
                from prod in products
                select prod.ProductName;

            // Зливаємо дві послідовності рядків в одну
            var allNames = customerNames.Concat(productNames);

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("Customer and product names:");
            foreach (var n in allNames)
            {
                Console.WriteLine(n);
            }
        }

        /// <summary>
        /// Приклад 95A: Об'єднання назв клієнтів і товарів за допомогою викликів методів розширення (Method Syntax).
        /// </summary>
        public void Linq95A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var allNames = customers
                .Select(cust => cust.CompanyName)
                .Concat(products.Select(prod => prod.ProductName));

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 50)));
            Console.WriteLine("Linq with Lambda.");
            Console.WriteLine("Customer and product names:");
            foreach (var n in allNames)
            {
                Console.WriteLine(n);
            }
        }
    }
}
