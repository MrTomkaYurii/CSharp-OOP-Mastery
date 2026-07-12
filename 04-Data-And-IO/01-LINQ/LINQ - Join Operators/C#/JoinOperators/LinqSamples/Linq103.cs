using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Групове з'єднання (Group Join)
    // ==============================================================================================
    // Категорія: Join Operators (Оператори з'єднання даних)
    // Призначення: Поєднує дві послідовності та створює ієрархічний результат «один-до-багатьох» (1:N).
    //
    // Принцип роботи:
    //   - Для кожного елемента лівої (зовнішньої) колекції формується пара:
    //     сам лівий елемент і вкладена послідовність (IEnumerable) усіх відповідних елементів із правої колекції.
    //   - Якщо для лівого елемента немає відповідників у правій колекції, вкладена послідовність буде просто порожньою (Count == 0),
    //     але сам лівий елемент НЕ випадає з результату!
    //   - У Query Syntax реалізується за допомогою: join ... in ... on ... equals ... into groupName.
    //   - У Method Syntax реалізується методом: outer.GroupJoin(inner, outerKey, innerKey, resultSelector).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution).
    // ==============================================================================================

    public partial class LinqSamplesJoinOperators
    {
        /// <summary>
        /// Приклад 103: Ієрархічне з'єднання постачальників із клієнтами за країною через group join (into cs) (Query Syntax).
        /// </summary>
        public void Linq103()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Make Sure to try yourself before looking at the code

            // Для кожного постачальника групуємо всіх клієнтів із його країни
            var custSupQuery =
                from sup in suppliers
                join cust in customers on sup.Country equals cust.Country into cs
                select new { Key = sup.Country, Items = cs };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            foreach (var item in custSupQuery)
            {
                Console.WriteLine(item.Key + ":");
                foreach (var element in item.Items)
                {
                    Console.WriteLine("   " + element.CompanyName);
                }
            }
        }

        /// <summary>
        /// Приклад 103A: Групове з'єднання за допомогою методу розширення GroupJoin(...) (Method Syntax).
        /// </summary>
        public void Linq103A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            var custSupQuery = suppliers
                .GroupJoin(customers, 
                    sup => sup.Country, 
                    cust => cust.Country, 
                    (sup, cs) => new { Key = sup.Country, Items = cs });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 75)));
            Console.WriteLine("Linq with Lambda.");
            foreach (var item in custSupQuery)
            {
                Console.WriteLine(item.Key + ":");
                foreach (var element in item.Items)
                {
                    Console.WriteLine("   " + element.CompanyName);
                }
            }
        }
    }
}
