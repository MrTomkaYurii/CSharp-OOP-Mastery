using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор SelectMany / Compound from — З'єднання сутностей
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції та сплющення)
    // Призначення: Вибірка пов'язаних сутностей за ключем (Join-подібний патерн) із додатковою фільтрацією.
    //
    // Принцип роботи:
    //   - Використовується складене речення 'from c in customers from o in orders'.
    //   - У реченні 'where' зв'язуються первинний та зовнішній ключі (c.CustomerID == o.CustomerID)
    //     та накладається обмеження на суму замовлення (o.Total < 500.00M).
    //   - Результатом є плоска послідовність анонімних об'єктів.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 15: Вибірка всіх замовлень із загальною сумою менше 500.00 за допомогою складеного from (Query Syntax).
        /// </summary>
        public void DataSetLinq15()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();

            #region LINQ Query Syntax (Compound from з фільтром замовлень)

            var smallOrders =
                from c in customers
                from o in orders
                where c.Field<string>("CustomerID") == o.Field<string>("CustomerID")
                    && o.Field<decimal>("Total") < 500.00M
                select new
                {
                    CustomerID = c.Field<string>("CustomerID"),
                    OrderID = o.Field<int>("OrderID"),
                    Total = o.Field<decimal>("Total")
                };

            #endregion

            // Виведення результату за допомогою ObjectDumper
            ObjectDumper.Write(smallOrders);
        }
    }
}
