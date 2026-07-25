using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O.OCPGoodExample
{
    public class CryptoPayment : IPaymentMethod
    {
        public string Name => "Криптовалюта";

        public void Process(decimal amount)
        {
            Console.WriteLine($"[{Name.ToUpper()}] Обробка оплати на суму {amount} грн");
            Console.WriteLine($"[{Name.ToUpper()}] Генерація Bitcoin адреси...");
            Console.WriteLine($"[{Name.ToUpper()}] QR-код для сканування: █▀▀█ █▀▀▄");
            Console.WriteLine($"[{Name.ToUpper()}] Очікування підтвердження в блокчейні...");
            Console.WriteLine($"[{Name.ToUpper()}] Підтверджено 3/6 блоків...");
            Console.WriteLine($"[{Name.ToUpper()}] [ОК] Оплата успішна");
        }

        public decimal CalculateFee(decimal amount)
        {
            return amount * 0.01m; // 1% комісія
        }
    }
}
