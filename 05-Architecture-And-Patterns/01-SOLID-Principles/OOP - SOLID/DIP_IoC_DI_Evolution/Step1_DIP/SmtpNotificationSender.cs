using System;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP
{
    // Низькорівневий модуль: відправка сповіщень через поштовий протокол SMTP.
    public class SmtpNotificationSender : INotificationSender
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"[SMTP] Надсилання листа на адресу <{recipient}>: \"{message}\"");
        }
    }
}
