using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O
{
    public class OcpBadExampleCommand : IMenuCommand
    {
        public string Title => "OCP - ПОГАНИЙ приклад (Порушення принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  OCP - ПОГАНИЙ ПРИКЛАД: Модифікація існуючого коду         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПРОБЛЕМИ цього підходу:");
            Console.WriteLine("[!] Для додавання нового способу оплати треба:");
            Console.WriteLine("   • Модифікувати метод ProcessPayment (додати новий if/else)");
            Console.WriteLine("   • Модифікувати метод CalculateFee (додати ще один if/else)");
            Console.WriteLine("   • Створюється прямий ризик порушити роботу вже протестованого коду");
            Console.WriteLine("   • Пряме порушення фундаментального принципу OCP\n");

            Console.WriteLine("[-] Важко тестувати - треба повторно регресійно тестувати весь монолітний клас");
            Console.WriteLine("[-] Неможливо додати новий тип оплати без фізичного редагування вихідного коду");
            Console.WriteLine("[-] Метод ProcessPayment прогресивно розростається (Anti-pattern God Method)");
            Console.WriteLine("[-] Порушення SRP - клас знає деталі всіх платіжних систем світу одночасно\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            var processor = new PaymentProcessorBad();

            Console.WriteLine("1. Оплата кредитною карткою:");
            processor.ProcessPayment("CreditCard", 500m);
            Console.WriteLine($"   Комісія: {processor.CalculateFee("CreditCard", 500m)} грн\n");

            Console.WriteLine("2. Оплата через PayPal:");
            processor.ProcessPayment("PayPal", 300m);
            Console.WriteLine($"   Комісія: {processor.CalculateFee("PayPal", 300m)} грн\n");

            Console.WriteLine("3. Оплата криптовалютою:");
            processor.ProcessPayment("Crypto", 1000m);
            Console.WriteLine($"   Комісія: {processor.CalculateFee("Crypto", 1000m)} грн\n");

            Console.WriteLine("[!] Проблема масштабування: якщо додати ще Google Pay, Apple Pay, IBAN...");
            Console.WriteLine("    Метод ProcessPayment перетворюється на некероване дерево умовних переходів.");
        }
    }
}
