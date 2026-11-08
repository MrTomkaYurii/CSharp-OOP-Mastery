using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPBadExample
{
    // Конкретна реалізація відправки email
    public class SmtpEmailSender
    {
        public void SendEmailViaSMTP(string to, string subject, string body)
        {
            Console.WriteLine($"[SMTP] Підключення до smtp.gmail.com:587...");
            Console.WriteLine($"[SMTP] Відправка email:");
            Console.WriteLine($"       Кому: {to}");
            Console.WriteLine($"       Тема: {subject}");
            Console.WriteLine($"       Текст: {body}");
        }
    }
}
