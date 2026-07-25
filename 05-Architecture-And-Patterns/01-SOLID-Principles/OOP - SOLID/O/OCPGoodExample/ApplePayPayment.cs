using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O.OCPGoodExample
{
    public class ApplePayPayment : IPaymentMethod
    {
        public string Name => "Apple Pay";

        public void Process(decimal amount)
        {
            Console.WriteLine($"[{Name.ToUpper()}] Обробка оплати на суму {amount} грн");
            Console.WriteLine($"[{Name.ToUpper()}] Виклик Apple Pay API...");
            Console.WriteLine($"[{Name.ToUpper()}] Авторизація через Face ID...");
            Console.WriteLine($"[{Name.ToUpper()}] Захищена транзакція через Secure Element...");
            Console.WriteLine($"[{Name.ToUpper()}] [ОК] Оплата успішна");
        }

        public decimal CalculateFee(decimal amount)
        {
            return amount * 0.02m; // 2% комісія
        }
    }
}
