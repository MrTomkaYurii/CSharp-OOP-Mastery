using System;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC
{
    // Сервіс користувачів, який використовує Абстрактну Фабрику для отримання залежностей.
    //
    // ЩО ЗМІНИЛОСЯ (Інверсія контролю - IoC):
    // Сервіс БІЛЬШЕ НЕ СТВОРЮЄ конкретні екземпляри MySqlDatabase чи SmtpSender через 'new'.
    // Управління створенням конкретних типів інвертовано і передано реалізації IInfrastructureFactory.
    //
    // ПРОБЛЕМА МОДЕЛІ "PULL" (Витягування):
    // Сервіс все ще змушений самостійно викликати методи фабрики (factory.CreateUserRepository()).
    // Сервіс прив'язаний до контракту фабрики, хоча йому потрібні лише самі об'єкти (репозиторій і логер).
    public class UserServiceWithFactory
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationSender _notificationSender;
        private readonly ILogger _logger;

        public UserServiceWithFactory(IInfrastructureFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            // Залежності "витягуються" з фабрики (Pull model)
            _userRepository = factory.CreateUserRepository();
            _notificationSender = factory.CreateNotificationSender();
            _logger = factory.CreateLogger();
        }

        public void RegisterUser(string userId, string userName, string email)
        {
            _logger.LogInfo($"[UserServiceWithFactory] Реєстрація користувача '{userName}'...");

            _userRepository.Save(userId, userName, email);
            _notificationSender.Send(email, $"Шановний(а) {userName}, ваш акаунт успішно створено!");

            _logger.LogInfo($"[UserServiceWithFactory] Користувача '{userId}' зареєстровано.");
        }
    }
}
