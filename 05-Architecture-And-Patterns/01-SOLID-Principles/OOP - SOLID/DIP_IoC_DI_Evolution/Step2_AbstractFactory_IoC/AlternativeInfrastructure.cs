using System;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC
{
    // Друга конкретна реалізація репозиторію: робота з PostgreSQL.
    public class PostgresUserRepository : IUserRepository
    {
        public void Save(string userId, string userName, string email)
        {
            Console.WriteLine($"[PostgreSQL] INSERT INTO users (id, name, email) VALUES ('{userId}', '{userName}', '{email}') RETURNING id;");
        }

        public string? GetById(string userId)
        {
            Console.WriteLine($"[PostgreSQL] SELECT * FROM users WHERE id = '{userId}' LIMIT 1;");
            return $"PgUser_{userId}";
        }
    }

    // Друга конкретна реалізація сповіщувача: мобільні Push-повідомлення.
    public class PushNotificationSender : INotificationSender
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"[PUSH] Надсилання Push-сповіщення на пристрій користувача '{recipient}': \"{message}\"");
        }
    }

    // Друга конкретна реалізація логера: запис у локальний файл.
    public class FileLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"[FILE LOG -> app.log] [INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[FILE LOG -> error.log] [ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");
        }
    }
}
