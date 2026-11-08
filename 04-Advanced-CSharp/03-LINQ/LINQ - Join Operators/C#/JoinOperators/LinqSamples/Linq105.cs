using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Ліве зовнішнє з'єднання (Left Outer Join)
    // ==============================================================================================
    // Категорія: Join Operators (Оператори з'єднання даних)
    // Призначення: Повертає ВСІ елементи лівої послідовності щонайменше один раз, навіть якщо
    //              для них немає відповідників у правій послідовності.
    //
    // Принцип роботи:
    //   - У LINQ немає окремого ключового слова `left join`.
    //   - Ліве з'єднання реалізується ідіоматичною комбінацією трьох операцій:
    //       1. join ... into cs (групове з'єднання правої колекції)
    //       2. from c in cs.DefaultIfEmpty() (розгортання групи з підстановкою null/дефолту, якщо група порожня)
    //       3. Перевірка c == null у проєкції результату.
    //   - Якщо збігів немає, c дорівнює null (для посилальних типів), що дозволяє вивести дефолтний текст або значення.
    //   - У Method Syntax патерн реалізується як:
    //     left.GroupJoin(right, ...).SelectMany(x => x.subCollection.DefaultIfEmpty(), ...).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesJoinOperators
    {
        /// <summary>
        /// Приклад 105: Ліве зовнішнє з'єднання постачальників із клієнтами за країною (DefaultIfEmpty) (Query Syntax).
        /// </summary>
        public void Linq105()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Make Sure to try yourself before looking at the code

            // Ліве з'єднання: якщо в країні постачальника немає клієнтів, DefaultIfEmpty поверне одне значення null
            var supplierCusts =
                from sup in suppliers
                join cust in customers on sup.Country equals cust.Country into cs
                from c in cs.DefaultIfEmpty()
                orderby sup.SupplierName
                select new
                {
                    sup.Country,
                    CompanyName = c == null ? "(No customers)" : c.CompanyName,
                    sup.SupplierName
                };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            foreach (var item in supplierCusts.Take(15))
            {
                Console.WriteLine("{0} ({1}): {2}", item.SupplierName, item.Country, item.CompanyName);
            }
        }

        /// <summary>
        /// Приклад 105A: Ліве зовнішнє з'єднання за допомогою GroupJoin(...).SelectMany(x => x.c.DefaultIfEmpty(), ...) (Method Syntax).
        /// </summary>
        public void Linq105A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            var supplierCusts = suppliers
                .GroupJoin(customers, 
                    sup => sup.Country, 
                    cust => cust.Country,
                    (sup, c) => new
                    {
                        sup.Country,
                        sup.SupplierName,
                        c
                    })
                .SelectMany(x => x.c.DefaultIfEmpty(), (sup, c) => new
                {
                    sup.Country,
                    sup.SupplierName,
                    CompanyName = c == null ? "(No customers)" : c.CompanyName
                })
                .OrderBy(sup => sup.SupplierName);

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 75)));
            Console.WriteLine("Linq with Lambda.");
            foreach (var item in supplierCusts.Take(15))
            {
                Console.WriteLine("{0} ({1}): {2}", item.SupplierName, item.Country, item.CompanyName);
            }
        }
    }
}
