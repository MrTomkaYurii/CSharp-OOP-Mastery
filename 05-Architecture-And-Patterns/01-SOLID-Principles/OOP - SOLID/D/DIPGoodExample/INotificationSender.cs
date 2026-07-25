using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample
{
    // Абстракція для відправки повідомлень
    public interface INotificationSender
    {
        void Send(string recipient, string subject, string message);
    }
}
