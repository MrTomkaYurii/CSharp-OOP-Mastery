using System;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP
{
    // Низькорівневий модуль: виведення журналу подій у консоль.
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"[LOG INFO] {DateTime.Now:HH:mm:ss} | {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[LOG ERROR] {DateTime.Now:HH:mm:ss} | {message}");
        }
    }
}
