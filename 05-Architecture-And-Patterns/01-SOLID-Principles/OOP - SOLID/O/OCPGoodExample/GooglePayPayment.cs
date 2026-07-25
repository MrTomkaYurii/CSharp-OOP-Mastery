using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O.OCPGoodExample
{
    public class GooglePayPayment : IPaymentMethod
    {
        public string Name => "Google Pay";

        public void Process(decimal amount)
        {
            Console.WriteLine($"[{Name.ToUpper()}] Обробка оплати на суму {amount} грн");
            Console.WriteLine($"[{Name.ToUpper()}] Виклик Google Pay API...");
            Console.WriteLine($"[{Name.ToUpper()}] Авторизація через біометрію...");
            Console.WriteLine($"[{Name.ToUpper()}] Токенізація платіжних даних...");
            Console.WriteLine($"[{Name.ToUpper()}] [ОК] Оплата успішна");
        }

        public decimal CalculateFee(decimal amount)
        {
            return amount * 0.02m; // 2% комісія
        }
    }
}
