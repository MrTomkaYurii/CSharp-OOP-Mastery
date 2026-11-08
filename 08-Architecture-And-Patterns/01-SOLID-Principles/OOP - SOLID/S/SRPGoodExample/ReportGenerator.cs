using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S.SRPGoodExample
{
    // 6. Генератор звітів - відповідає ЛИШЕ за створення звітів
    public class ReportGenerator
    {
        public void GeneratePdfReport(Order order)
        {
            Console.WriteLine("\n[ЗВІТ] Генерація PDF звіту...");
            Console.WriteLine($"[ЗВІТ] ═══════════════════════════════");
            Console.WriteLine($"[ЗВІТ] Замовлення №{order.Id}");
            Console.WriteLine($"[ЗВІТ] Дата: {order.CreatedAt}");
            Console.WriteLine($"[ЗВІТ] Клієнт: {order.CustomerEmail}");
            Console.WriteLine($"[ЗВІТ] ───────────────────────────────");
            Console.WriteLine($"[ЗВІТ] Товари:");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"[ЗВІТ]   • {item}");
            }
            Console.WriteLine($"[ЗВІТ] ───────────────────────────────");
            Console.WriteLine($"[ЗВІТ] Сума: {order.TotalPrice} грн");
            Console.WriteLine($"[ЗВІТ] Знижка: {order.Discount} грн");
            Console.WriteLine($"[ЗВІТ] До сплати: {order.FinalPrice} грн");
            Console.WriteLine($"[ЗВІТ] ═══════════════════════════════");
            Console.WriteLine("[ЗВІТ] [ОК] PDF-звіт успішно згенеровано");
        }
    }
}
