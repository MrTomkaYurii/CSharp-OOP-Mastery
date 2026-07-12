using System;
using System.Linq;
using Customer = LinqSamplesCommon.Model.Customers;

namespace PartitioningOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Take у складених запитах (Обмеження вибірки)
    // ==============================================================================================
    // Категорія: Partitioning Operators (Оператори розбиття / пагінації)
    // Призначення: Обмеження результуючого набору даних першими N записами після фільтрації та проєкції.
    //
    // Принцип роботи:
    //   - Запит у Query Syntax огортається дужками, після чого викликається метод .Take(count).
    //   - Завдяки відкладеному виконанню зовнішній цикл зупиняє генерацію результатів після знаходження
    //     перших N відповідних записів.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesPartitioningOperators
    {
        /// <summary>
        /// Приклад 21: Вибірка перших 3 замовлень клієнтів із регіону Вашингтон (Region == "WA").
        /// </summary>
        public void Linq21()
        {
            var customers = Customer.GetCustomerList();

            #region LINQ Query Syntax + Take

            // Складений запит для пошуку замовлень клієнтів із Вашингтона, обмежений першими 3 записами
            var first3WAOrders = (
                from cust in customers
                from order in cust.Orders
                where cust.Region == "WA"
                select new { cust.CustomerID, order.OrderID, order.OrderDate })
                .Take(3);

            Console.WriteLine("Перші 3 замовлення клієнтів із Вашингтона:");
            foreach (var order in first3WAOrders)
            {
                ObjectDumper.Write(order);
            }

            #endregion
        }
    }
}
