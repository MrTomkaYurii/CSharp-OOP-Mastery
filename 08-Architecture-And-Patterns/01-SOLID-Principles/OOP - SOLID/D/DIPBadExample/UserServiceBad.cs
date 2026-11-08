using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPBadExample
{
    // ПОГАНИЙ ПРИКЛАД: Порушення DIP
    // Високорівневий сервіс прямо залежить від низькорівневих реалізацій
    public class UserServiceBad
    {
        // Жорстка прив'язка до конкретних класів замість абстракцій
        private readonly FileLogger _logger;
        private readonly SmtpEmailSender _emailSender;
        private readonly MySqlDatabase _database;

        public UserServiceBad()
        {
            // Створюємо конкретні залежності всередині класу (жорстке зачеплення - Tight Coupling)
            _logger = new FileLogger();
            _emailSender = new SmtpEmailSender();
            _database = new MySqlDatabase();
        }

        public void RegisterUser(string email, string name)
        {
            _logger.LogToFile($"Початок реєстрації користувача: {email}");

            try
            {
                // Валідація
                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    _logger.LogToFile($"Помилка: невалідний email - {email}");
                    throw new ArgumentException("Невалідний email");
                }

                // Збереження в БД
                string userId = Guid.NewGuid().ToString().Substring(0, 8);
                _database.SaveToMySQL(userId, $"Name: {name}, Email: {email}");

                // Відправка email
                _emailSender.SendEmailViaSMTP(
                    email,
                    "Ласкаво просимо!",
                    $"Вітаємо, {name}! Ваш акаунт створено."
                );

                _logger.LogToFile($"Користувач {email} успішно зареєстрований");
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Критична помилка: {ex.Message}");
                throw;
            }
        }

        // Проблеми підтримки:
        // 1. Зміна БД з MySQL на PostgreSQL змушує змінювати код цього класу.
        // 2. Зміна логування на ConsoleLogger чи DatabaseLogger вимагає переписування коду.
        // 3. Неможливо написати ізольовані Unit-тести без реальної файлової системи, мережі й СУБД.
    }

}
