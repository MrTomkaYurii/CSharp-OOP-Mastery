using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC
{
    // Конкретна фабрика 1: створює стек інфраструктури для середовища MySQL.
    public class MySqlInfrastructureFactory : IInfrastructureFactory
    {
        public IUserRepository CreateUserRepository() => new MySqlUserRepository();
        public INotificationSender CreateNotificationSender() => new SmtpNotificationSender();
        public ILogger CreateLogger() => new ConsoleLogger();
    }

    // Конкретна фабрика 2: створює стек інфраструктури для хмарного середовища PostgreSQL.
    public class PostgresInfrastructureFactory : IInfrastructureFactory
    {
        public IUserRepository CreateUserRepository() => new PostgresUserRepository();
        public INotificationSender CreateNotificationSender() => new PushNotificationSender();
        public ILogger CreateLogger() => new FileLogger();
    }
}
