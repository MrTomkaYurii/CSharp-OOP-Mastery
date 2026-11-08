using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestrictionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Where — Фільтрація сутностей із пов'язаними даними
    // ==============================================================================================
    // Категорія: Restriction Operators (Оператори фільтрації)
    // Призначення: Фільтрація батьківських сутностей за атрибутом (наприклад, Region == "WA")
    //              з подальшим доступом до дочірніх колекцій (Orders).
    //
    // Принцип роботи:
    //   - Запит відбирає колекцію батьківських об'єктів Customer.
    //   - Для кожного відібраного клієнта можна звернутися до його властивості Orders (масив або список).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesRestrictionOperators
    {
        /// <summary>
        /// Приклад 4: Пошук клієнтів зі штату Вашингтон (Region == "WA") та виведення їхніх замовлень
        /// за допомогою Query Syntax.
        /// </summary>
        public void Linq4()
        {
            // Вхідні дані: список клієнтів
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Query Syntax

            // Відбираємо клієнтів із регіону "WA"
            var waCustomers =
                from cust in customers
                where cust.Region == "WA"
                select cust;

            Console.WriteLine("Query Syntax — Клієнти з Вашингтона та їхні замовлення:");
            Console.WriteLine();
            foreach (var customer in waCustomers)
            {
                Console.WriteLine("Клієнт {0}: {1}", customer.CustomerID, customer.CompanyName);
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine("  Замовлення {0}: {1:d}", order.OrderID, order.OrderDate);
                }
            }

            #endregion
        }

        /// <summary>
        /// Приклад 4A: Пошук клієнтів зі штату Вашингтон та виведення їхніх замовлень за допомогою Method Syntax.
        /// </summary>
        public void Linq4A()
        {
            // Вхідні дані: список клієнтів
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Method Syntax (Lambda)

            // Фільтрація клієнтів за допомогою методу Where()
            var waCustomers = customers.Where(cust => cust.Region == "WA");

            Console.WriteLine("*******************************************");
            Console.WriteLine("Lambda-вираз — Клієнти з Вашингтона та їхні замовлення:");
            Console.WriteLine();
            foreach (var customer in waCustomers)
            {
                Console.WriteLine("Клієнт {0}: {1}", customer.CustomerID, customer.CompanyName);
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine("  Замовлення {0}: {1:d}", order.OrderID, order.OrderDate);
                }
            }

            #endregion
        }
    }
}
