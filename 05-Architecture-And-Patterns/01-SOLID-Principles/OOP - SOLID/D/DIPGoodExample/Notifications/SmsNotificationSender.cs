using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Notifications
{
    // Реалізація 2: SMS повідомлення
    public class SmsNotificationSender : INotificationSender
    {
        public void Send(string recipient, string subject, string message)
        {
            Console.WriteLine($"[SMS SENDER] Відправка SMS на {recipient}");
            Console.WriteLine($"             Текст: {message}");
        }
    }
}
