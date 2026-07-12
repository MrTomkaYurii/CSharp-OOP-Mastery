using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Ліве зовнішнє з'єднання зі сторони клієнтів (Customer Left Outer Join)
    // ==============================================================================================
    // Категорія: Join Operators (Оператори з'єднання даних)
    // Призначення: Для кожного клієнта повертає всіх постачальників із тієї ж країни, або маркер відсутності постачальників.
    //
    // Принцип роботи:
    //   - Зворотна конфігурація лівого з'єднання: базовою лівою таблицею виступає Customers.
    //   - Якщо для певної країни (наприклад, Мексика) немає зареєстрованого постачальника,
    //     метод DefaultIfEmpty() підставляє s = null.
    //   - Тернарний оператор (s == null ? "(No suppliers)" : s.SupplierName) коректно обробляє відсутність зв'язку.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesJoinOperators
    {
        /// <summary>
        /// Приклад 106: Ліве зовнішнє з'єднання клієнтів із постачальниками за країною (Query Syntax).
        /// </summary>
        public void Linq106()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Make Sure to try yourself before looking at the code

            // Клієнти з'єднуються з постачальниками; якщо постачальників у країні немає -> DefaultIfEmpty
            var custSuppliers =
                from cust in customers
                join sup in suppliers on cust.Country equals sup.Country into ss
                from s in ss.DefaultIfEmpty()
                orderby cust.CompanyName
                select new
                {
                    cust.Country,
                    cust.CompanyName,
                    SupplierName = s == null ? "(No suppliers)" : s.SupplierName
                };

            #endregion

            Console.WriteLine();
            Console.WriteLine("Original 101 Linq example.");
            foreach (var item in custSuppliers.Take(10))
            {
                Console.WriteLine("{0} ({1}): {2}", item.CompanyName, item.Country, item.SupplierName);
            }
        }

        /// <summary>
        /// Приклад 106A: Ліве з'єднання клієнтів із постачальниками за допомогою ланцюжка GroupJoin(...).SelectMany(...) (Method Syntax).
        /// </summary>
        public void Linq106A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            var custSuppliers = customers
                .GroupJoin(suppliers, 
                    cust => cust.Country, 
                    sup => sup.Country, 
                    (cust, ss) => new
                    {
                        cust.Country,
                        cust.CompanyName,
                        ss
                    })
                .SelectMany(x => x.ss.DefaultIfEmpty(),
                    (cust, ss) => new
                    {
                        cust.Country,
                        cust.CompanyName,
                        SupplierName = ss == null ? "(No suppliers)" : ss.SupplierName
                    })
                .OrderBy(cust => cust.CompanyName);

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 75)));
            Console.WriteLine("Linq with Lambda.");
            foreach (var item in custSuppliers.Take(10))
            {
                Console.WriteLine("{0} ({1}): {2}", item.CompanyName, item.Country, item.SupplierName);
            }
        }
    }
}
