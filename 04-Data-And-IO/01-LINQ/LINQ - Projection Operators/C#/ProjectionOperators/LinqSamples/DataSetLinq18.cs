using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Рання фільтрація (Early Filtering) у складених запитах
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції та оптимізація запитів)
    // Призначення: Підвищення ефективності запиту шляхом фільтрації головної сутності (Customer)
    //              ДО виконання декартового добутку / з'єднання із дочірньою колекцією (Orders).
    //
    // Принцип роботи:
    //   - Якщо відфільтрувати клієнтів (Region == "WA") перед другим реченням 'from',
    //     система не витрачатиме ресурси на перебір замовлень для клієнтів з інших регіонів.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 18: Оптимізована вибірка замовлень: клієнти фільтруються за регіоном "WA" до вибірки їхніх замовлень.
        /// </summary>
        public void DataSetLinq18()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();
            DateTime cutoffDate = new DateTime(1997, 1, 1);

            #region LINQ Query Syntax (Рання фільтрація)

            var myOrders =
                from c in customers
                where c.Field<string>("Region") == "WA"
                from o in orders
                where c.Field<string>("CustomerID") == o.Field<string>("CustomerID")
                   && (DateTime)o["OrderDate"] >= cutoffDate
                select new { CustomerID = c.Field<string>("CustomerID"), OrderID = o.Field<int>("OrderID") };

            Console.WriteLine("Query Syntax (Рання фільтрація клієнтів):");
            ObjectDumper.Write(myOrders);

            #endregion
        }

        /// <summary>
        /// Приклад 18A: Реалізація ранньої фільтрації за допомогою Method Syntax (Where -> Join -> Where).
        /// </summary>
        public void DataSetLinq18A()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();
            DateTime cutoffDate = new DateTime(1997, 1, 1);

            #region LINQ Method Syntax (Lambda)

            var myOrders = customers
                .Where(c => c.Field<string>("Region") == "WA")
                .Join(orders,
                    c => c.Field<string>("CustomerID"),
                    o => o.Field<string>("CustomerID"),
                    (c, o) => new
                    {
                        CustomerID = c.Field<string>("CustomerID"),
                        OrderID = o.Field<int>("OrderID"),
                        OrderDate = (DateTime)o["OrderDate"]
                    })
                .Where(o => o.OrderDate > cutoffDate);

            Console.WriteLine();
            Console.WriteLine("**************************************");
            Console.WriteLine("Lambda Syntax (Where + Join + Where):");
            ObjectDumper.Write(myOrders);

            #endregion
        }
    }
}
