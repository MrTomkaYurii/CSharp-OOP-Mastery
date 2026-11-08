using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample
{
    // ДОБРИЙ ПРИКЛАД: Сервіс залежить виключно від АБСТРАКЦІЙ, а не від конкретних деталей
    public class UserServiceGood
    {
        private readonly ILogger _logger;
        private readonly INotificationSender _notificationSender;
        private readonly IUserRepository _userRepository;

        // Dependency Injection через конструктор
        public UserServiceGood(
            ILogger logger,
            INotificationSender notificationSender,
            IUserRepository userRepository)
        {
            // Отримуємо залежності ззовні (DI)
            _logger = logger;
            _notificationSender = notificationSender;
            _userRepository = userRepository;
        }

        public void RegisterUser(string email, string name)
        {
            _logger.Log($"Початок реєстрації користувача: {email}");

            try
            {
                // Валідація
                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    _logger.Log($"Помилка: невалідний email - {email}");
                    throw new ArgumentException("Невалідний email");
                }

                // Збереження в БД (не знаємо яка конкретно БД - це абстракція!)
                string userId = Guid.NewGuid().ToString().Substring(0, 8);
                _userRepository.Save(userId, $"Name: {name}, Email: {email}");

                // Відправка повідомлення (не знаємо як саме - це абстракція!)
                _notificationSender.Send(
                    email,
                    "Ласкаво просимо!",
                    $"Вітаємо, {name}! Ваш акаунт створено."
                );

                _logger.Log($"Користувач {email} успішно зареєстрований");
            }
            catch (Exception ex)
            {
                _logger.Log($"Критична помилка: {ex.Message}");
                throw;
            }
        }

        // Переваги дизайну:
        // 1. Можна вільно змінити СУБД, логування чи спосіб сповіщень без зміни жодного рядка цього класу.
        // 2. Легко тестувати в Unit-тестах: мок-об'єкти передаються через конструктор за 1 рядок коду.
        // 3. UserServiceGood не залежить від деталей інфраструктури (чиста архітектура).
    }
}
