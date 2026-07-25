using System;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC
{
    // Демонстрація Кроку 2: Інверсія контролю (IoC) за допомогою патерна Абстрактна Фабрика.
    public static class FactoryIoCDemo
    {
        public static void Run()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  КРОК 2. IoC через АБСТРАКТНУ ФАБРИКУ (Abstract Factory)   ");
            Console.WriteLine("  Принцип проектування: Делегація створення сімейств об'єктів");
            Console.WriteLine("============================================================\n");

            Console.WriteLine("ТЕОРЕТИЧНЕ ОБҐРУНТУВАННЯ:");
            Console.WriteLine("1. Патерн GoF 'Abstract Factory' забезпечує перший рівень IoC (Inversion of Control).");
            Console.WriteLine("   Клас UserServiceWithFactory більше НЕ створює об'єкти самостійно.");
            Console.WriteLine("   Контроль за вибором конкретного сімейства драйверів інвертовано у фабрику.\n");

            Console.WriteLine("2. Принцип Голлівуду (Hollywood Principle) у дії:");
            Console.WriteLine("   'Не створюй деталі сам — звернися до фабрики!'\n");

            Console.WriteLine("--- СЦЕНАРІЙ 1: Запуск на стеку MySQL (Локальний сервер) ---");
            IInfrastructureFactory mysqlFactory = new MySqlInfrastructureFactory();
            var service1 = new UserServiceWithFactory(mysqlFactory);
            service1.RegisterUser("usr_201", "Сергій Ребров", "rebrov@uaf.ua");

            Console.WriteLine("\n--- СЦЕНАРІЙ 2: Перемикання на хмарний стек PostgreSQL (AWS/Azure) ---");
            IInfrastructureFactory postgresFactory = new PostgresInfrastructureFactory();
            var service2 = new UserServiceWithFactory(postgresFactory);
            service2.RegisterUser("usr_202", "Олександр Шовковський", "shovkovskyi@fcdk.ua");

            Console.WriteLine("\nПРОБЛЕМА, ЯКА ПРИЗВЕЛА ДО ПОЯВИ DI (Dependency Injection):");
            Console.WriteLine("1. Сервіс все ще змушений знати про інтерфейс фабрики 'IInfrastructureFactory'.");
            Console.WriteLine("2. Сервіс використовує модель PULL (сам запитує залежності у фабрики).");
            Console.WriteLine("3. Якщо сервісу потрібен лише репозиторій, фабрика все одно тягне створення пошти та логера.");
            Console.WriteLine("4. Це веде нас до Кроку 3: Dependency Injection (модель PUSH замість PULL)!");
        }
    }
}
