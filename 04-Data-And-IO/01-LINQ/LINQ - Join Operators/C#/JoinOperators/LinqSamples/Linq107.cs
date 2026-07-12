using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: З'єднання за складеними ключами (Composite Key Join)
    // ==============================================================================================
    // Категорія: Join Operators (Оператори з'єднання даних)
    // Призначення: З'єднує послідовності за кількома полями одночасно (наприклад, City та Country).
    //
    // Принцип роботи:
    //   - Для багатополярного зв'язку в C#/LINQ створюються анонімні типи:
    //     new { sup.City, sup.Country } equals new { cust.City, cust.Country }.
    //   - Компілятор C# автоматично генерує перевизначені методи Equals() та GetHashCode()
    //     для анонімних типів із однаковими іменами та типами властивостей.
    //   - Завдяки цьому два різні об'єкти анонімного типу вважаються рівними, якщо збігаються всі їхні властивості!
    //   - Важливо: імена та порядок властивостей в обох анонімних типах ключа повинні точно збігатися.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesJoinOperators
    {
        /// <summary>
        /// Приклад 107: Ліве зовнішнє з'єднання постачальників із клієнтами за складеним ключем (City та Country) (Query Syntax).
        /// </summary>
        public void Linq107()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Make Sure to try yourself before looking at the code

            // З'єднання за складеним ключем (City, Country) з використанням анонімних типів
            var supplierCusts =
                from sup in suppliers
                join cust in customers on new { sup.City, sup.Country } equals new { cust.City, cust.Country } into cs
                from c in cs.DefaultIfEmpty()
                orderby sup.SupplierName
                select new
                {
                    sup.Country,
                    sup.City,
                    sup.SupplierName,
                    CompanyName = c == null ? "(No customers)" : c.CompanyName
                };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");

            foreach (var item in supplierCusts.Take(10))
            {
                Console.WriteLine("{0} ({1}, {2}): {3}", item.SupplierName, item.City, item.Country, item.CompanyName);
            }
        }

        /// <summary>
        /// Приклад 107A: Ліве з'єднання за складеним ключем за допомогою методів розширення (Method Syntax).
        /// </summary>
        public void Linq107A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Make Sure to try yourself before looking at the code

            var supplierCusts = suppliers
                .GroupJoin(customers, 
                    sup => new { sup.City, sup.Country }, 
                    cust => new { cust.City, cust.Country },
                    (sup, cs) => new
                    {
                        sup.Country,
                        sup.City,
                        sup.SupplierName,
                        cs
                    })
                .SelectMany(x => x.cs.DefaultIfEmpty(), (sub, cs) => new
                {
                    sub.Country,
                    sub.City,
                    sub.SupplierName,
                    CompanyName = cs == null ? "(No customers)" : cs.CompanyName
                })
                .OrderBy(sup => sup.SupplierName);
             
            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 75)));
            Console.WriteLine("Linq with Lambda.");
            foreach (var item in supplierCusts.Take(10))
            {
                Console.WriteLine("{0} ({1}, {2}): {3}", item.SupplierName, item.City, item.Country, item.CompanyName);
            }
        }
    }
}
