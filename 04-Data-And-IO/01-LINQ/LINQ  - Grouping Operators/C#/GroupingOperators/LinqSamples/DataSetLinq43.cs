using System;
using System.Data;
using System.Linq;

namespace GroupingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Багаторівневе (ієрархічне) групування
    // ==============================================================================================
    // Категорія: Grouping Operators (Оператори групування)
    // Призначення: Побудова складної деревної структури даних через вкладені оператори групування
    //              (наприклад, Клієнт -> Замовлення за роками -> Замовлення за місяцями).
    //
    // Принцип роботи:
    //   - Зовнішній запит вибирає клієнтів.
    //   - Для кожного клієнта його замовлення спочатку групуються за роком (OrderDate.Year).
    //   - Всередині кожної річної групи замовлення вдруге групуються за місяцем (OrderDate.Month).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesGroupingOperators
    {
        /// <summary>
        /// Приклад 43: Ієрархічне групування замовлень клієнта спочатку за роком, а потім за місяцем (Query Syntax).
        /// </summary>
        public void DataSetLinq43()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();

            #region LINQ Query Syntax (Вкладені group by: Рік -> Місяць)

            var customerOrderGroups =
                from c in customers
                select new
                {
                    CompanyName = c.Field<string>("CompanyName"),
                    YearGroups =
                        from o in c.GetChildRows("CustomersOrders")
                        group o by o.Field<DateTime>("OrderDate").Year into yg
                        select new
                        {
                            Year = yg.Key,
                            MonthGroups =
                                from o in yg
                                group o by o.Field<DateTime>("OrderDate").Month into mg
                                select new { Month = mg.Key, Orders = mg }
                        }
                };

            Console.WriteLine("Query Syntax — Ієрархічне групування замовлень (Рік -> Місяць):");
            foreach (var cog in customerOrderGroups.Take(3))
            {
                Console.WriteLine("Компанія: {0}", cog.CompanyName);
                foreach (var yg in cog.YearGroups.Take(3))
                {
                    Console.WriteLine("\t Рік: {0}", yg.Year);
                    foreach (var mg in yg.MonthGroups.Take(3))
                    {
                        Console.WriteLine("\t\t Місяць: {0}", mg.Month);
                        foreach (var order in mg.Orders)
                        {
                            Console.WriteLine("\t\t\t OrderID: {0}, Дата: {1:d}",
                                order.Field<int>("OrderID"), order.Field<DateTime>("OrderDate"));
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Приклад 43A: Ієрархічне групування замовлень за допомогою Method Syntax (GroupBy всередині GroupBy).
        /// </summary>
        public void DataSetLinq43A()
        {
            var customers = testDS.Tables["Customers"].AsEnumerable();

            #region LINQ Method Syntax (Nested GroupBy)

            var customerOrderGroups = customers
                .Select(c => new
                {
                    CompanyName = c.Field<string>("CompanyName"),
                    YearGroups = c.GetChildRows("CustomersOrders")
                        .GroupBy(o => o.Field<DateTime>("OrderDate").Year)
                        .Select(yg => new
                        {
                            Year = yg.Key,
                            MonthGroups = yg
                                .GroupBy(o => o.Field<DateTime>("OrderDate").Month)
                                .Select(mg => new { Month = mg.Key, Orders = mg }).Take(3)
                        }).Take(3)
                }).Take(3);

            Console.WriteLine();
            Console.WriteLine("********************************************************");
            Console.WriteLine("Method Syntax — Ієрархічне групування замовлень:");
            foreach (var cog in customerOrderGroups)
            {
                Console.WriteLine("Компанія: {0}", cog.CompanyName);
                foreach (var yg in cog.YearGroups)
                {
                    Console.WriteLine("\t Рік: {0}", yg.Year);
                    foreach (var mg in yg.MonthGroups)
                    {
                        Console.WriteLine("\t\t Місяць: {0}", mg.Month);
                        foreach (var order in mg.Orders)
                        {
                            Console.WriteLine("\t\t\t OrderID: {0}, Дата: {1:d}",
                                order.Field<int>("OrderID"), order.Field<DateTime>("OrderDate"));
                        }
                    }
                }
            }

            #endregion
        }
    }
}
