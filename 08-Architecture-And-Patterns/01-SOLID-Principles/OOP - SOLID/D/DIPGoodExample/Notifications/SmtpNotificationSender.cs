using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Notifications
{
    // Реалізація 1: Email через SMTP
    public class SmtpNotificationSender : INotificationSender
    {
        public void Send(string recipient, string subject, string message)
        {
            Console.WriteLine($"[SMTP SENDER] Відправка через smtp.gmail.com");
            Console.WriteLine($"              -> {recipient}: {subject}");
        }
    }
}
