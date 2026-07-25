using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O.OCPGoodExample
{
    public class PayPalPayment : IPaymentMethod
    {
        public string Name => "PayPal";

        public void Process(decimal amount)
        {
            Console.WriteLine($"[{Name.ToUpper()}] Обробка оплати на суму {amount} грн");
            Console.WriteLine($"[{Name.ToUpper()}] Перенаправлення на PayPal.com...");
            Console.WriteLine($"[{Name.ToUpper()}] Авторизація користувача...");
            Console.WriteLine($"[{Name.ToUpper()}] Підтвердження платежу...");
            Console.WriteLine($"[{Name.ToUpper()}] Повернення на сайт...");
            Console.WriteLine($"[{Name.ToUpper()}] [ОК] Оплата успішна");
        }

        public decimal CalculateFee(decimal amount)
        {
            return amount * 0.035m; // 3.5% комісія
        }
    }
}
