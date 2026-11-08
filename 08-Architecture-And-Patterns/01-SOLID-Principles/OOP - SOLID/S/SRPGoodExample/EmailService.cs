using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S.SRPGoodExample
{
    // 5. Email сервіс - відповідає ЛИШЕ за відправку email
    public class EmailService
    {
        public void SendOrderConfirmation(Order order)
        {
            Console.WriteLine("\n[EMAIL] Відправка підтвердження замовлення...");
            Console.WriteLine($"[EMAIL] Кому: {order.CustomerEmail}");
            Console.WriteLine($"[EMAIL] Тема: Ваше замовлення №{order.Id} підтверджено");
            Console.WriteLine($"[EMAIL] Товари: {string.Join(", ", order.Items)}");
            Console.WriteLine($"[EMAIL] Сума: {order.FinalPrice} грн");
            Console.WriteLine("[EMAIL] [ОК] Email успішно відправлено");
        }
    }
}
