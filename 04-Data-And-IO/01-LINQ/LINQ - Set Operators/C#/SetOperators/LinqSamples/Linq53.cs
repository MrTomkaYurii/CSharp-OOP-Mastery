using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Except для символьних проєкцій (Різниця літер)
    // ==============================================================================================
    // Категорія: Set Operators (Оператори над множинами)
    // Призначення: Знаходження символів (перших літер товарів), які жодного разу не зустрічаються
    //              як перші літери назв клієнтів.
    //
    // Принцип роботи:
    //   - Від множини символів товарів віднімається множина символів клієнтів (Products \ Customers).
    //   - Результат містить лише ті літери, які унікальні для каталогу товарів.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesSetOperators
    {
        /// <summary>
        /// Приклад 53: Пошук перших літер назв товарів, з яких не починається жодна компанія-клієнт (Query + Except).
        /// </summary>
        public void Linq53()
        {
            List<Products.Product> products = Products.GetProductList();
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Query Syntax + Except()

            var productFirstChars =
                from prod in products
                select prod.ProductName[0];

            var customerFirstChars =
                from cust in customers
                select cust.CompanyName[0];

            var productOnlyFirstChars = productFirstChars.Except(customerFirstChars);

            Console.WriteLine("Перші літери назв товарів, яких немає серед назв клієнтів:");
            foreach (var ch in productOnlyFirstChars)
            {
                Console.WriteLine("  " + ch);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 53A: Пошук різниці літер за допомогою Method Syntax (Select + Except).
        /// </summary>
        public void Linq53A()
        {
            List<Products.Product> products = Products.GetProductList();
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Method Syntax (Select + Except)

            var productOnlyFirstChars = products
                .Select(prod => prod.ProductName[0])
                .Except(customers.Select(cust => cust.CompanyName[0]));

            Console.WriteLine();
            Console.WriteLine("*************************************");
            Console.WriteLine("Method Syntax — Перші літери лише для товарів:");
            foreach (var ch in productOnlyFirstChars)
            {
                Console.WriteLine("  " + ch);
            }

            #endregion
        }
    }
}
