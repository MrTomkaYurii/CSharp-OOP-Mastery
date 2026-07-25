using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O.OCPGoodExample
{
    public class CashPayment : IPaymentMethod
    {
        public string Name => "Готівка";

        public void Process(decimal amount)
        {
            Console.WriteLine($"[{Name.ToUpper()}] Обробка оплати на суму {amount} грн");
            Console.WriteLine($"[{Name.ToUpper()}] Генерація рахунку для оплати...");
            Console.WriteLine($"[{Name.ToUpper()}] Відправка інструкцій клієнту...");
            Console.WriteLine($"[{Name.ToUpper()}] Очікування підтвердження оплати...");
            Console.WriteLine($"[{Name.ToUpper()}] [ОК] Оплата успішна");
        }

        public decimal CalculateFee(decimal amount)
        {
            return 0; // Без комісії
        }
    }
}
