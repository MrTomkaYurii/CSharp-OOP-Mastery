using SolidPrinciplesDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S
{
    // Команда для демонстрації гарного прикладу SRP
    public class SrpGoodExampleCommand : IMenuCommand
    {
        public string Title => "SRP - ГАРНИЙ приклад (Дотримання принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  SRP - ГАРНИЙ ПРИКЛАД: Розділення відповідальностей        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПЕРЕВАГИ цього підходу:");
            Console.WriteLine("[+] Кожен клас має одну чітку відповідальність:");
            Console.WriteLine("   • OrderValidator - валідація");
            Console.WriteLine("   • DiscountCalculator - розрахунок знижок");
            Console.WriteLine("   • OrderRepository - робота з БД");
            Console.WriteLine("   • EmailService - відправка email");
            Console.WriteLine("   • ReportGenerator - генерація звітів");
            Console.WriteLine("   • Logger - логування");
            Console.WriteLine("   • OrderProcessorGood - координація процесу\n");

            Console.WriteLine("[+] Легко тестувати - кожен клас можна тестувати ізольовано в Unit-тестах");
            Console.WriteLine("[+] Легко змінювати - зміна логіки знижок не впливає на email");
            Console.WriteLine("[+] Легко розширювати - можна додати нові правила розрахунків");
            Console.WriteLine("[+] Низький ризик регресій - зміни суворо локалізовані");
            Console.WriteLine("[+] Можна повторно використовувати класи в інших частинах системи\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            var processor = new OrderProcessorGood();
            var items = new List<string> { "Ноутбук", "Миша", "Клавіатура" };

            processor.ProcessOrder("customer@example.com", items, 1200m);
        }
    }
}
