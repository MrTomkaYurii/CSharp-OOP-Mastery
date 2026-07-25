using System;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step3_DependencyInjection
{
    // Сервіс користувачів із застосуванням патерна Constructor Injection (Впровадження через конструктор).
    //
    // ЕВОЛЮЦІЙНИЙ СТРИБОК (Від PULL до PUSH):
    // 1. Клас НЕ знає ні про які фабрики (IInfrastructureFactory видалено).
    // 2. Клас НЕ викликає жодних методів створення.
    // 3. Залежності передаються безпосередньо у вигляді вже готових об'єктів (PUSH-модель).
    //
    // ПЕРЕВАГИ CONSTRUCTOR INJECTION:
    // - Повна незмінність (Immutability): поля readonly, гарантовано ініціалізовані.
    // - Явний контракт: будь-який розробник одразу бачить, що потрібно класу для роботи.
    // - Захист від невалідного стану: неможливо створити напівініціалізований сервіс.
    public class UserServiceWithDI
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationSender _notificationSender;
        private readonly ILogger _logger;

        public UserServiceWithDI(
            IUserRepository userRepository,
            INotificationSender notificationSender,
            ILogger logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _notificationSender = notificationSender ?? throw new ArgumentNullException(nameof(notificationSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RegisterUser(string userId, string userName, string email)
        {
            _logger.LogInfo($"[UserServiceWithDI] Реєстрація користувача '{userName}' через Constructor DI");

            _userRepository.Save(userId, userName, email);
            _notificationSender.Send(email, $"Привіт, {userName}! Твій акаунт активовано через DI.");

            _logger.LogInfo($"[UserServiceWithDI] Успішно збережено в репозиторій.");
        }
    }
}
