using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O
{
    public class OcpGoodExampleCommand : IMenuCommand
    {
        public string Title => "OCP - ГАРНИЙ приклад (Дотримання принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  OCP - ГАРНИЙ ПРИКЛАД: Розширення без модифікації          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПЕРЕВАГИ цього підходу:");
            Console.WriteLine("[+] Для додавання нового способу оплати:");
            Console.WriteLine("   • Створюємо новий клас (наприклад, ApplePayPayment)");
            Console.WriteLine("   • Реалізуємо інтерфейс IPaymentMethod");
            Console.WriteLine("   • Реєструємо у системі (через DI або конфігурацію)");
            Console.WriteLine("   • НЕ ЗМІНЮЄМО жодного рядка наявного коду процессора!\n");

            Console.WriteLine("[+] Легко тестувати - кожен метод оплати тестується окремо у ізольованому Unit-тесті");
            Console.WriteLine("[+] Легко розширювати - просто додаємо нові класи за контрактом");
            Console.WriteLine("[+] Безпечно - стабільна бізнес-логіка залишається недоторканою");
            Console.WriteLine("[+] Дотримання OCP - відкрито для розширення, закрито для модифікації");
            Console.WriteLine("[+] Кожен клас відповідає за один конкретний платіжний протокол (SRP)\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            var processor = new PaymentProcessorGood();

            processor.ShowAvailablePayments();

            Console.WriteLine("\n[ТЕСТ 1] Оплата кредитною карткою:");
            processor.ProcessPayment("Кредитна картка", 500m);

            Console.WriteLine("\n\n[ТЕСТ 2] Оплата через PayPal:");
            processor.ProcessPayment("PayPal", 300m);

            Console.WriteLine("\n\n[ТЕСТ 3] Оплата криптовалютою:");
            processor.ProcessPayment("Криптовалюта", 1000m);

            Console.WriteLine("\n\n[ТЕСТ 4] Оплата через Google Pay (НОВИЙ МЕТОД):");
            processor.ProcessPayment("Google Pay", 750m);

            Console.WriteLine("\n\n[ТЕСТ 5] Оплата через Apple Pay (НОВИЙ МЕТОД):");
            processor.ProcessPayment("Apple Pay", 850m);

            Console.WriteLine("\n\n[РЕЗУЛЬТАТ] Всі платежі успішно оброблені поліморфно.");
            Console.WriteLine("\n[ВИСНОВОК] Ми додали Google Pay і Apple Pay БЕЗ зміни");
            Console.WriteLine("   алгоритмічного коду PaymentProcessorGood!");
            Console.WriteLine("   Це і є суть принципу Open/Closed!");
        }
    }
}
