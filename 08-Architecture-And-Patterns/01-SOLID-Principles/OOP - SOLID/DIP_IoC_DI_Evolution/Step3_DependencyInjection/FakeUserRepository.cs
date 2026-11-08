using System;
using System.Collections.Generic;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step3_DependencyInjection
{
    // Імітація (Mock/Fake) репозиторію для модульного тестування в оперативній пам'яті.
    // Завдяки DI ми можемо протестувати всю бізнес-логіку UserService за 1 мілісекунду
    // без жодного реального мережевого з'єднання з базою даних!
    public class FakeUserRepository : IUserRepository
    {
        private readonly Dictionary<string, string> _storage = new();

        public int SaveCallCount { get; private set; }

        public void Save(string userId, string userName, string email)
        {
            SaveCallCount++;
            _storage[userId] = $"{userName} ({email})";
            Console.WriteLine($"[TEST IN-MEMORY MOCK] Збережено користувача {userId} у словник пам'яті. Всього викликів: {SaveCallCount}");
        }

        public string? GetById(string userId)
        {
            _storage.TryGetValue(userId, out var val);
            return val;
        }
    }
}
