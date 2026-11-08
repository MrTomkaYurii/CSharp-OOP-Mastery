using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Notifications
{
    // Реалізація 3: Push сповіщення
    public class PushNotificationSender : INotificationSender
    {
        public void Send(string recipient, string subject, string message)
        {
            Console.WriteLine($"[PUSH SENDER] Push-сповіщення для {recipient}");
            Console.WriteLine($"              {subject}: {message}");
        }
    }
}
