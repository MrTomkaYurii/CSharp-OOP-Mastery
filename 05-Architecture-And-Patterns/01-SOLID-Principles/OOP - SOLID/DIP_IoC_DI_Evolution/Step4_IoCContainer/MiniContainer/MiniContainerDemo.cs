using System;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step3_DependencyInjection;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step4_IoCContainer.MiniContainer
{
    // Демонстрація роботи навчального міні-контейнера SimpleContainer.
    public static class MiniContainerDemo
    {
        public static void Run()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  КРОК 4.1. ВЛАСНИЙ МІНІ IoC-КОНТЕЙНЕР (Under The Hood)    ");
            Console.WriteLine("  Як влаштований контейнер зсередини: Рефлексія та Словник  ");
            Console.WriteLine("============================================================\n");

            Console.WriteLine("ЯК ЦЕ ПРАЦЮЄ ПІД КАПОТОМ:");
            Console.WriteLine("1. Ми реєструємо зв'язки Interface -> Implementation у словник.");
            Console.WriteLine("2. Запитуємо 'Resolve<UserServiceWithDI>()'.");
            Console.WriteLine("3. Контейнер бачить, що UserServiceWithDI вимагає (IUserRepository, INotificationSender, ILogger).");
            Console.WriteLine("4. Контейнер автоматично знаходить зареєстровані класи і рекурсивно створює весь граф!\n");

            var container = new SimpleContainer();

            // 1. Конфігурація (Реєстрація залежностей)
            container.Register<IUserRepository, PostgresUserRepository>();
            container.Register<INotificationSender, PushNotificationSender>();
            container.RegisterSingleton<ILogger, ConsoleLogger>(); // Сінглтон

            Console.WriteLine("[РЕЄСТРАЦІЯ]:");
            Console.WriteLine("   IUserRepository     -> PostgresUserRepository (Transient)");
            Console.WriteLine("   INotificationSender -> PushNotificationSender (Transient)");
            Console.WriteLine("   ILogger             -> ConsoleLogger          (Singleton)\n");

            // 2. Резолвінг: жодного 'new UserServiceWithDI(...)' руками!
            Console.WriteLine("[АВТОМАТИЧНИЙ РЕЗОЛВІНГ]:");
            var userService = container.Resolve<UserServiceWithDI>();

            userService.RegisterUser("usr_401", "Ліна Костенко", "kostenko@poetry.ua");

            Console.WriteLine("\n[ПЕРЕВІРКА СІНГЛТОНА В МІНІ-КОНТЕЙНЕРІ]:");
            var logger1 = container.Resolve<ILogger>();
            var logger2 = container.Resolve<ILogger>();
            bool isSameLogger = ReferenceEquals(logger1, logger2);
            Console.WriteLine($"Обидва екземпляри логера посилаються на один і той самий об'єкт: {isSameLogger} (Успіх!)");
        }
    }
}
