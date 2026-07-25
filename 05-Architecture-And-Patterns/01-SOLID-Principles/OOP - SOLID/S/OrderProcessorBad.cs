using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S
{
    // ПОГАНИЙ ПРИКЛАД: Клас з надлишковою відповідальністю (порушення SRP)
    // Цей клас одночасно відповідає за: бізнес-логіку замовлення, валідацію,
    // збереження в БД, відправку email, генерацію звітів та логування.
    public class OrderProcessorBad
    {
        public void ProcessOrder(string customerEmail, List<string> items, decimal totalPrice)
        {
            // 1. Валідація замовлення (бізнес-логіка)
            if (string.IsNullOrEmpty(customerEmail))
                throw new ArgumentException("Email не може бути порожнім");

            if (items == null || !items.Any())
                throw new ArgumentException("Замовлення повинно містити товари");

            // 2. Розрахунок знижки (бізнес-логіка)
            decimal discount = 0;
            if (totalPrice > 1000)
                discount = totalPrice * 0.1m;

            decimal finalPrice = totalPrice - discount;

            // 3. Збереження в базу даних (робота з даними)
            Console.WriteLine("[БД] Підключення до бази даних...");
            Console.WriteLine($"[БД] INSERT INTO Orders (Email, Total) VALUES ('{customerEmail}', {finalPrice})");
            foreach (var item in items)
            {
                Console.WriteLine($"[БД] INSERT INTO OrderItems (Item) VALUES ('{item}')");
            }

            // 4. Відправка email повідомлення (зовнішній сервіс)
            Console.WriteLine("\n[EMAIL] Відправка підтвердження замовлення...");
            Console.WriteLine($"[EMAIL] Кому: {customerEmail}");
            Console.WriteLine($"[EMAIL] Тема: Ваше замовлення підтверджено");
            Console.WriteLine($"[EMAIL] Тіло листа: Дякуємо за замовлення на суму {finalPrice} грн");

            // 5. Генерація звіту (звітність)
            Console.WriteLine("\n[ЗВІТ] Генерація PDF звіту...");
            Console.WriteLine($"[ЗВІТ] Замовлення №{Guid.NewGuid()}");
            Console.WriteLine($"[ЗВІТ] Клієнт: {customerEmail}");
            Console.WriteLine($"[ЗВІТ] Товари: {string.Join(", ", items)}");
            Console.WriteLine($"[ЗВІТ] Сума зі знижкою: {finalPrice} грн");

            // 6. Логування (технічна деталь)
            Console.WriteLine($"\n[ЛОГ] {DateTime.Now}: Замовлення оброблено успішно");
        }
    }
}
