using System;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP
{
    // Низькорівневий модуль: конкретна реалізація для реляційної СУБД MySQL.
    // Завдяки принципу DIP цей клас реалізує інтерфейс IUserRepository,
    // а не бізнес-сервіс залежить від нього безпосередньо.
    public class MySqlUserRepository : IUserRepository
    {
        public void Save(string userId, string userName, string email)
        {
            Console.WriteLine($"[MySQL] INSERT INTO Users (Id, Name, Email) VALUES ('{userId}', '{userName}', '{email}')");
        }

        public string? GetById(string userId)
        {
            Console.WriteLine($"[MySQL] SELECT * FROM Users WHERE Id = '{userId}'");
            return $"User_{userId}";
        }
    }
}
