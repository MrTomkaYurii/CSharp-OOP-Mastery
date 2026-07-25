using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S.SRPGoodExample
{
    // 4. Репозиторій - відповідає ЛИШЕ за роботу з базою даних
    public class OrderRepository
    {
        public void Save(Order order)
        {
            Console.WriteLine("\n[БД] Підключення до бази даних...");
            Console.WriteLine($"[БД] INSERT INTO Orders (Id, Email, Total) VALUES " +
                            $"('{order.Id}', '{order.CustomerEmail}', {order.FinalPrice})");

            foreach (var item in order.Items)
            {
                Console.WriteLine($"[БД] INSERT INTO OrderItems (OrderId, Item) VALUES ('{order.Id}', '{item}')");
            }

            Console.WriteLine("[БД] [ОК] Замовлення збережено в базі даних");
        }
    }
}
