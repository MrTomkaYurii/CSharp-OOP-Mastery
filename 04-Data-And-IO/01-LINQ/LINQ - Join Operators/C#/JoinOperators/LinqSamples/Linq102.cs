using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Внутрішнє з'єднання (Inner Equijoin)
    // ==============================================================================================
    // Категорія: Join Operators (Оператори з'єднання даних)
    // Призначення: Поєднує дві послідовності на основі рівності ключів (елементів зв'язку).
    //
    // Принцип роботи:
    //   - Реалізує алгоритм Hash Join: за замовчуванням будує хеш-таблицю для правої (внутрішньої) послідовності
    //     та потім за один прохід перевіряє ліву (зовнішню) послідовність.
    //   - Складність алгоритму близька до лінійної: O(N + M), що кардинально швидше за вкладені цикли O(N * M).
    //   - До результату потрапляють ТІЛЬКИ ті пари елементів, для яких знайдено збіг ключа в обох колекціях.
    //   - У Query Syntax використовується конструкція: join ... in ... on outerKey equals innerKey.
    //   - У Method Syntax використовується: outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming після побудови хеш-таблиці).
    // ==============================================================================================

    public partial class LinqSamplesJoinOperators
    {
        /// <summary>
        /// Приклад 102: Просте внутрішнє з'єднання постачальників та клієнтів за країною (sup.Country == cust.Country) (Query Syntax).
        /// </summary>
        public void Linq102()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Make Sure to try yourself before looking at the code

            // Внутрішнє з'єднання постачальників і клієнтів з однаковою країною
            var custSupJoin =
                from sup in suppliers
                join cust in customers on sup.Country equals cust.Country
                select new { Country = sup.Country, SupplierName = sup.SupplierName, CustomerName = cust.CompanyName };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            foreach (var item in custSupJoin.Take(10))
            {
                Console.WriteLine("Country = {0}, Supplier = {1}, Customer = {2}", item.Country, item.SupplierName, item.CustomerName);
            }
        }

        /// <summary>
        /// Приклад 102A: Внутрішнє з'єднання за допомогою методу розширення Join(...) (Method Syntax).
        /// </summary>
        public void Linq102A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();
            List<Suppliers.Supplier> suppliers = Suppliers.GetSupplierList();

            #region Make Sure to try yourself before looking at the code

            // Функціональний виклик Join з передачею селекторів ключів і проектора результату
            var custSupJoin = suppliers
                .Join(customers, 
                    sup => sup.Country, 
                    cust => cust.Country,
                    (sup, cust) => new { sup.Country, sup.SupplierName, CustomerName = cust.CompanyName });

            #endregion

            Console.WriteLine();
            Console.WriteLine(string.Concat(Enumerable.Repeat('*', 100)));
            Console.WriteLine("Linq with Lambda");
            foreach (var item in custSupJoin.Take(10))
            {
                Console.WriteLine("Country = {0}, Supplier = {1}, Customer = {2}", item.Country, item.SupplierName, item.CustomerName);
            }
        }
    }
}
