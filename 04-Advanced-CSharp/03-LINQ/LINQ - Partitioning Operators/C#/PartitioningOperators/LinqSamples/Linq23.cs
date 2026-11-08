using System;
using System.Collections.Generic;
using System.Linq;
using LinqSamplesCommon.Model;

namespace PartitioningOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Skip у складених запитах (Пропуск записів)
    // ==============================================================================================
    // Категорія: Partitioning Operators (Оператори розбиття / пагінації)
    // Призначення: Пропуск перших N результатів запиту після фільтрації та сплющення пов'язаних даних.
    //
    // Принцип роботи:
    //   - Спочатку формується запит (клієнти з Вашингтона та їхні замовлення).
    //   - За допомогою .Skip(2) перші 2 знайдені замовлення пропускаються, а всі подальші повертаються.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesPartitioningOperators
    {
        /// <summary>
        /// Приклад 23: Вибірка всіх замовлень клієнтів із Вашингтона, крім перших двох.
        /// </summary>
        public void Linq23()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region LINQ Query Syntax + Skip

            var waOrders =
                from cust in customers
                from order in cust.Orders
                where cust.Region == "WA"
                select new { cust.CustomerID, order.OrderID, order.OrderDate };

            // Пропускаємо перші 2 замовлення
            var allButFirst2Orders = waOrders.Skip(2);

            Console.WriteLine("Усі замовлення з Вашингтона, крім перших двох:");
            foreach (var order in allButFirst2Orders)
            {
                ObjectDumper.Write(order);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 23A: Аналогічна вибірка з безпосереднім викликом .Skip(2) над згрупованим у дужки виразом запиту.
        /// </summary>
        public void Linq23A()
        {
            List<Customers.Customer> customers = Customers.GetCustomerList();

            #region Компактний синтаксис запиту із дужками

            var allButFirst2Orders = (
                from cust in customers
                from order in cust.Orders
                where cust.Region == "WA"
                select new { cust.CustomerID, order.OrderID, order.OrderDate })
                .Skip(2);

            Console.WriteLine();
            Console.WriteLine("*****************************************");
            Console.WriteLine("Компактний запис — Усі замовлення з Вашингтона, крім перших 2:");
            foreach (var order in allButFirst2Orders)
            {
                ObjectDumper.Write(order);
            }

            #endregion
        }
    }
}
