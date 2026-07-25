using System;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP
{
    // Демонстрація Кроку 1: Принцип інверсії залежностей (DIP).
    // Пояснює, що таке інверсія напрямку залежностей і як абстракція захищає бізнес-логіку.
    public static class DipPrincipleDemo
    {
        public static void Run()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  КРОК 1. DIP (Dependency Inversion Principle)             ");
            Console.WriteLine("  Архітектурний принцип: Інверсія напрямку залежностей       ");
            Console.WriteLine("============================================================\n");

            Console.WriteLine("ТЕОРЕТИЧНЕ ОБҐРУНТУВАННЯ:");
            Console.WriteLine("1. Раніше (у поганому коді) UserService залежав безпосередньо від MySQL:");
            Console.WriteLine("   [UserService] ----> [MySqlDatabase]");
            Console.WriteLine("   Будь-яка зміна версії БД чи драйвера змушувала переписувати сервіс.\n");

            Console.WriteLine("2. Застосувавши DIP, ми інвертуємо стрілку залежності через АБСТРАКЦІЮ:");
            Console.WriteLine("   [UserService]       ----> [IUserRepository] (Контракт)");
            Console.WriteLine("   [MySqlUserRepository] --> [IUserRepository] (Реалізація)");
            Console.WriteLine("   Тепер обидва модулі дивляться на один і той самий спільний контракт!\n");

            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ ЧЕРЕЗ АБСТРАКЦІЇ:");
            IUserRepository repository = new MySqlUserRepository();
            INotificationSender notifier = new SmtpNotificationSender();
            ILogger logger = new ConsoleLogger();

            logger.LogInfo("Створення користувача за контрактом...");
            repository.Save("usr_101", "Андрій Шевченко", "sheva@football.ua");
            notifier.Send("sheva@football.ua", "Вітаємо у системі!");
            logger.LogInfo("Користувача успішно оброблено через інтерфейси.\n");

            Console.WriteLine("ПРОБЛЕМА, ЯКА ЗАЛИШАЄТЬСЯ НА ЦЬОМУ ЕТАПІ:");
            Console.WriteLine("Хоча ми працюємо через інтерфейси, залишається відкритим питання:");
            Console.WriteLine("Хто, де і коли повинен викликати 'new MySqlUserRepository()'?\n");
            Console.WriteLine("Якщо сам UserService напише 'new' всередині — він все одно керуватиме створенням.");
            Console.WriteLine("Це веде нас до Кроку 2: Інверсія контролю (IoC) через Абстрактну Фабрику!");
        }
    }
}
