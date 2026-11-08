using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Intersect для символьних даних (Спільні елементи)
    // ==============================================================================================
    // Категорія: Set Operators (Оператори над множинами)
    // Призначення: Пошук спільних початкових літер, які зустрічаються як серед назв товарів,
    //              так і серед назв компаній-клієнтів.
    //
    // Принцип роботи:
    //   - Обчислюються дві послідовності перших літер.
    //   - Intersect() повертає множину літер, що належать одночасно обом колекціям.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesSetOperators
    {
        /// <summary>
        /// Приклад 51: Знаходження спільних початкових літер назв товарів та компаній-клієнтів (Query + Intersect).
        /// </summary>
        public void Linq51()
        {
            List<Products.Product> products = Products.GetProductList();
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Query Syntax + Intersect()

            var productFirstChars =
                from prod in products
                select prod.ProductName[0];

            var customerFirstChars =
                from cust in customers
                select cust.CompanyName[0];

            var commonFirstChars = productFirstChars.Intersect(customerFirstChars);

            Console.WriteLine("Спільні перші літери назв товарів і клієнтів:");
            foreach (var ch in commonFirstChars.Take(10))
            {
                Console.WriteLine("  " + ch);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 51A: Знаходження спільних літер за допомогою Method Syntax (Select + Intersect).
        /// </summary>
        public void Linq51A()
        {
            List<Products.Product> products = Products.GetProductList();
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Method Syntax (Select + Intersect)

            var commonFirstChars = products
                .Select(prod => prod.ProductName[0])
                .Intersect(customers.Select(cust => cust.CompanyName[0]));

            Console.WriteLine();
            Console.WriteLine("***************************");
            Console.WriteLine("Method Syntax — Спільні перші літери назв:");
            foreach (var ch in commonFirstChars.Take(10))
            {
                Console.WriteLine("  " + ch);
            }

            #endregion
        }
    }
}
