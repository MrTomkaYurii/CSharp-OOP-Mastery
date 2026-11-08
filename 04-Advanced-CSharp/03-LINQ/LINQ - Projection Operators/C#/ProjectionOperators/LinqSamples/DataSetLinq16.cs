using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Складене речення 'from' (SelectMany) з фільтрацією за датою
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції та сплющення)
    // Призначення: Вибірка та сплющення пов'язаних замовлень клієнтів із фільтрацією за часовим критерієм.
    //
    // Принцип роботи:
    //   - Обмежує кількість клієнтів (Take(10)) та з'єднує їх із таблицею замовлень.
    //   - Перевіряє умову на дату: OrderDate >= 1998-01-01.
    //   - Порівнюються підходи: Compound from, Query Syntax Join та Method Syntax Join.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 16: Вибірка замовлень перших 10 клієнтів, зроблених у 1998 році або пізніше,
        /// за допомогою складеного from (Query Syntax).
        /// </summary>
        public void DataSetLinq16()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();

            #region LINQ Query Syntax (Compound from)

            var myOrders =
                from c in customers.Take(10)
                from o in orders
                where c.Field<string>("CustomerID") == o.Field<string>("CustomerID") &&
                      o.Field<DateTime>("OrderDate") >= new DateTime(1998, 1, 1)
                select new
                {
                    CustomerID = c.Field<string>("CustomerID"),
                    OrderID = o.Field<int>("OrderID"),
                    OrderDate = o.Field<DateTime>("OrderDate")
                };

            Console.WriteLine("Query Syntax (Compound from) — Замовлення від 1998 року:");
            ObjectDumper.Write(myOrders.Take(10));

            #endregion
        }

        /// <summary>
        /// Приклад 16A: Порівняння підходів з'єднання: через оператор join у Query Syntax та метод Join() у Lambda Syntax.
        /// </summary>
        public void DataSetLinq16A()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();

            #region Query Syntax Join

            var myOrders =
                from c in customers.Take(10)
                join o in orders
                on c.Field<string>("CustomerID") equals o.Field<string>("CustomerID")
                where o.Field<DateTime>("OrderDate") >= new DateTime(1998, 1, 1)
                select new
                {
                    CustomerID = o.Field<string>("CustomerID"),
                    OrderID = o.Field<int>("OrderID"),
                    OrderDate = o.Field<DateTime>("OrderDate")
                };

            Console.WriteLine();
            Console.WriteLine("***************************************");
            Console.WriteLine("Query Syntax з оператором 'join':");
            ObjectDumper.Write(myOrders.Take(10));

            #endregion

            #region Lambda Syntax Join

            var myOrdersLambda = customers
                .Take(10)
                .Join(orders, c => c.Field<string>("CustomerID"), o => o.Field<string>("CustomerID"),
                (c, o) => new
                {
                    CustomerID = c.Field<string>("CustomerID"),
                    OrderID = o.Field<int>("OrderID"),
                    OrderDate = o.Field<DateTime>("OrderDate")
                })
                .Where(o => o.OrderDate >= new DateTime(1998, 1, 1));

            Console.WriteLine();
            Console.WriteLine("***************************************");
            Console.WriteLine("Lambda Syntax з методом Join():");
            ObjectDumper.Write(myOrdersLambda.Take(10));

            #endregion
        }
    }
}
