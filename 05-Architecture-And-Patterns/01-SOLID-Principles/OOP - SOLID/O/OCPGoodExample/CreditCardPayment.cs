using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O.OCPGoodExample
{
    // Конкретні реалізації - кожна є окремим класом
    public class CreditCardPayment : IPaymentMethod
    {
        public string Name => "Кредитна картка";

        public void Process(decimal amount)
        {
            Console.WriteLine($"[{Name.ToUpper()}] Обробка оплати на суму {amount} грн");
            Console.WriteLine($"[{Name.ToUpper()}] Підключення до банківського API...");
            Console.WriteLine($"[{Name.ToUpper()}] Перевірка CVV коду...");
            Console.WriteLine($"[{Name.ToUpper()}] Авторизація 3D-Secure...");
            Console.WriteLine($"[{Name.ToUpper()}] Списання коштів...");
            Console.WriteLine($"[{Name.ToUpper()}] [ОК] Оплата успішна");
        }

        public decimal CalculateFee(decimal amount)
        {
            return amount * 0.025m; // 2.5% комісія
        }
    }
}
