using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Count на вкладених колекціях
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Підраховує кількість елементів у вкладеній дочірній колекції для кожного батьківського об'єкта.
    //
    // Принцип роботи:
    //   - Застосовується всередині проекції Select для кожного окремого клієнта: cust.Orders.Count().
    //   - Якщо колекція Orders реалізує ICollection<T>, виклик .Count() оптимізується до O(1) через властивість .Count.
    //   - Дозволяє формувати підсумкові звіти без попереднього розгортання зв'язків.
    //
    // Тип виконання:
    //   - Негайне виконання для кожної вкладеної колекції під час матеріалізації або перебору проекції.
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 76: Отримання списку клієнтів та кількості їхніх замовлень за допомогою Count (Query Syntax).
        /// </summary>
        public void Linq76()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region Make Sure to try yourself before looking at the code

            // Для кожного клієнта рахуємо кількість замовлень у вкладеній колекції Orders
            var orderCounts = 
                from cust in customers
                select new { cust.CustomerID, OrderCount = cust.Orders.Count() };

            #endregion

            Console.WriteLine("Original 101 Linq() examples.");
            ObjectDumper.Write(orderCounts.Take(10));
        }

        /// <summary>
        /// Приклад 76A: Підрахунок вкладених замовлень за допомогою Select(cust => ...) (Method Syntax).
        /// </summary>
        public void Linq76A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            var orderCounts = customers.Select(cust => new { cust.CustomerID, OrderCount = cust.Orders.Count() });

            #endregion

            Console.WriteLine();
            Console.WriteLine("************************************");
            Console.WriteLine("Linq with Lambda.");
            ObjectDumper.Write(orderCounts.Take(10));
        }
    }
}
