using System;
using Microsoft.Extensions.DependencyInjection;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step3_DependencyInjection;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step4_IoCContainer.DotNetContainer
{
    // Демонстрація повноцінної інтеграції сучасного промислового контейнера
    // Microsoft.Extensions.DependencyInjection (стандартного для .NET 8 / 9 та ASP.NET Core).
    public static class DotNetContainerDemo
    {
        public static void Run()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  КРОК 4.3. ПРОМИСЛОВИЙ DI-КОНТЕЙНЕР (Microsoft DI)         ");
            Console.WriteLine("  Сучасний стандарт розробки .NET 8 / .NET 9                ");
            Console.WriteLine("============================================================\n");

            Console.WriteLine("ЯК ПРАЦЮЄ ПОВНИЙ ЛАНЦЮЖОК:");
            Console.WriteLine("1. Створюємо ServiceCollection (Кошик реєстрацій).");
            Console.WriteLine("2. Реєструємо сервіси із зазначенням життєвих циклів.");
            Console.WriteLine("3. Будуємо ServiceProvider (Двигун побудови графа об'єктів).");
            Console.WriteLine("4. Запитуємо готовий сервіс високого рівня однією командою.\n");

            // 1. Створення колекції служб
            var services = new ServiceCollection();

            // 2. Декларативна конфігурація залежностей
            services.AddScoped<IUserRepository, PostgresUserRepository>();
            services.AddTransient<INotificationSender, PushNotificationSender>();
            services.AddSingleton<ILogger, ConsoleLogger>();

            // Реєстрація самого бізнес-сервісу
            services.AddTransient<UserServiceWithDI>();

            // 3. Побудова контейнера
            using ServiceProvider serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("[КОНТЕЙНЕР ЗБУДОВАНО]");
            Console.WriteLine("Отримання екземпляра UserServiceWithDI без жодного 'new'...");

            // 4. Отримання та виконання
            var userService = serviceProvider.GetRequiredService<UserServiceWithDI>();
            userService.RegisterUser("usr_501", "Богдан Хмельницький", "hetman@cossacks.ua");

            Console.WriteLine("\n[ПІДСУМОК ЕВОЛЮЦІЇ]:");
            Console.WriteLine("1. DIP:      Задали правила архітектури (залежність від інтерфейсів).");
            Console.WriteLine("2. IoC:      Забрали право у сервісу створювати свої залежності (Abstract Factory).");
            Console.WriteLine("3. DI:       Почали передавати готові об'єкти ззовні через конструктор (Constructor DI).");
            Console.WriteLine("4. Container:Автоматизували рутинне створення об'єктів та життєвих циклів.");
        }
    }
}
