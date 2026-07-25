using SolidPrinciplesDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S
{
    // Команда для демонстрації поганого прикладу SRP
    public class SrpBadExampleCommand : IMenuCommand
    {
        public string Title => "SRP - ПОГАНИЙ приклад (Порушення принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  SRP - ПОГАНИЙ ПРИКЛАД: Клас з множинними відповідальностями");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПРОБЛЕМИ цього підходу:");
            Console.WriteLine("[!] Клас OrderProcessorBad відповідає за 6 різних речей:");
            Console.WriteLine("   1. Валідацію даних");
            Console.WriteLine("   2. Бізнес-логіку (розрахунок знижок)");
            Console.WriteLine("   3. Роботу з базою даних");
            Console.WriteLine("   4. Відправку email");
            Console.WriteLine("   5. Генерацію звітів");
            Console.WriteLine("   6. Логування\n");

            Console.WriteLine("[-] Якщо зміниться логіка знижок - треба міняти цей клас");
            Console.WriteLine("[-] Якщо зміниться формат email - треба міняти цей клас");
            Console.WriteLine("[-] Якщо зміниться БД - треба міняти цей клас");
            Console.WriteLine("[-] Важко тестувати - треба створювати/мокати 5+ залежностей");
            Console.WriteLine("[-] Високий ризик внесення дефектів при будь-яких змінах\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            var processor = new OrderProcessorBad();
            var items = new List<string> { "Ноутбук", "Миша", "Клавіатура" };

            processor.ProcessOrder("customer@example.com", items, 1200m);
        }
    }
}
