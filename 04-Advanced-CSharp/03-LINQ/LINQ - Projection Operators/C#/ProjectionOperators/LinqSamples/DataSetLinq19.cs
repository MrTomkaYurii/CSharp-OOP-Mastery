using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор SelectMany з індексом (Indexed SelectMany)
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції та сплющення)
    // Призначення: Сплющення вкладених колекцій із передачею індексу батьківського елемента у дочірній результат.
    //
    // Принцип роботи:
    //   - Перевантаження: SelectMany<TSource, TResult>(this IEnumerable<TSource> source,
    //       Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
    //       Func<TSource, TCollection, TResult> resultSelector).
    //   - Параметр int (custIndex) передає позицію поточного батьківського елемента (клієнта),
    //     що дозволяє нумерувати або ідентифікувати записи за їхнім порядком у вхідній вибірці.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 19: Використання індексованого SelectMany для вибірки замовлень із нумерацією клієнтів (Method Syntax).
        /// </summary>
        public void DataSetLinq19()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();

            #region LINQ Method Syntax (Indexed SelectMany)

            // custIndex — індекс поточного клієнта у вхідній послідовності
            var customerOrders = customers.SelectMany(
                (cust, custIndex) =>
                orders.Where(o => cust.Field<string>("CustomerID") == o.Field<string>("CustomerID"))
                      .Select(o => new { CustomerIndex = custIndex + 1, OrderID = o.Field<int>("OrderID") }));

            Console.WriteLine("Method Syntax (Індексований SelectMany):");
            foreach (var c in customerOrders.Take(10))
            {
                Console.WriteLine("Клієнт №{0} має замовлення з OrderID {1}", c.CustomerIndex, c.OrderID);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 19A: Еквівалент індексованої вибірки за допомогою Query Syntax із попереднім індексуванням через Select.
        /// </summary>
        public void DataSetLinq19A()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();

            #region LINQ Query Syntax з попередньою індексацією

            // Оскільки в чистому Query Syntax немає індексованого from, спочатку індексуємо клієнтів через Select
            var customerOrders =
                from cust in customers.Select((value, index) => new { value, index })
                from o in orders
                where cust.value.Field<string>("CustomerID") == o.Field<string>("CustomerID")
                select new { CustomerIndex = cust.index + 1, OrderID = o.Field<int>("OrderID") };

            Console.WriteLine();
            Console.WriteLine("**************************************");
            Console.WriteLine("Query Syntax з попередньою індексацією:");
            foreach (var c in customerOrders.Take(10))
            {
                Console.WriteLine("Клієнт №{0} має замовлення з OrderID {1}", c.CustomerIndex, c.OrderID);
            }

            #endregion
        }
    }
}
