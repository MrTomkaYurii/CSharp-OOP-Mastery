using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC
{
    // Патерн Абстрактна Фабрика (GoF Abstract Factory):
    // Надає інтерфейс для створення сімейств взаємопов'язаних або взаємозалежних об'єктів
    // без вказування їхніх конкретних класів.
    //
    // Роль в інверсії контролю (IoC):
    // Бізнес-сервіс більше не викликає 'new' для конкретних класів драйверів,
    // а делегує це завдання абстрактній фабриці.
    public interface IInfrastructureFactory
    {
        IUserRepository CreateUserRepository();
        INotificationSender CreateNotificationSender();
        ILogger CreateLogger();
    }
}
