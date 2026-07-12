using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Ключове слово 'let' (Проміжні змінні в LINQ)
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції)
    // Призначення: Оголошення локальної змінної всередині LINQ-запиту (Query Syntax) для збереження
    //              проміжного результату та уникнення його повторного обчислення.
    //
    // Принцип роботи:
    //   - 'let variable = expression' створює нову змінну в області видимості запиту.
    //   - Змінну можна використовувати в наступних реченнях where, orderby, select.
    //   - Компілятор перетворює let на анонімний тип, який пакує попередній діапазон змінних
    //     разом із новою змінною (transparent identifier).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 17: Використання ключового слова 'let' для збереження суми замовлення (Total)
        /// та її фільтрації (Total >= 2000.00).
        /// </summary>
        public void DataSetLinq17()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();

            #region LINQ Query Syntax з ключовим словом 'let'

            var myOrders =
                from c in customers
                from o in orders
                let total = o.Field<decimal>("Total")
                where c.Field<string>("CustomerID") == o.Field<string>("CustomerID")
                    && total >= 2000.0M
                select new { CustomerID = c.Field<string>("CustomerID"), OrderID = o.Field<int>("OrderID"), total };

            Console.WriteLine("Query Syntax з використанням 'let' (сума >= 2000):");
            ObjectDumper.Write(myOrders.Take(10));

            #endregion
        }

        /// <summary>
        /// Приклад 17A: Порівняння реалізації через Query Syntax з join та через Method Syntax (Join + Where).
        /// </summary>
        public void DataSetLinq17A()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();
            var orders = testDS.Tables["Orders"].AsEnumerable();

            #region Query Syntax з оператором 'join' та 'let'

            var myOrders =
                from c in customers
                join o in orders on c.Field<string>("CustomerID") equals o.Field<string>("CustomerID")
                let total = o.Field<decimal>("Total")
                where total >= 2000.0M
                select new { CustomerID = c.Field<string>("CustomerID"), OrderID = o.Field<int>("OrderID"), total };

            Console.WriteLine();
            Console.WriteLine("**************************************");
            Console.WriteLine("Query syntax з 'join' та 'let':");
            ObjectDumper.Write(myOrders.Take(10));

            #endregion

            #region Lambda Syntax з методом Join()

            var myOrders2 = customers
                .Join(orders,
                    c => c.Field<string>("CustomerID"),
                    o => o.Field<string>("CustomerID"),
                    (c, o) => new
                    {
                        CustomerID = c.Field<string>("CustomerID"),
                        OrderID = o.Field<int>("OrderID"),
                        Total = o.Field<decimal>("Total")
                    })
                .Where(o => o.Total >= 2000.0M);

            Console.WriteLine();
            Console.WriteLine("**************************************");
            Console.WriteLine("Lambda syntax з методом Join():");
            ObjectDumper.Write(myOrders2.Take(10));

            #endregion
        }
    }
}
