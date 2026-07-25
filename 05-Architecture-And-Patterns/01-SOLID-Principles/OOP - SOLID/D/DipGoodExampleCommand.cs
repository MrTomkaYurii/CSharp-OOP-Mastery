using OOP___SOLID.D.DIPGoodExample;
using OOP___SOLID.D.DIPGoodExample.Loggers;
using OOP___SOLID.D.DIPGoodExample.Notifications;
using OOP___SOLID.D.DIPGoodExample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D
{
    public class DipGoodExampleCommand : IMenuCommand
    {
        public string Title => "DIP - ГАРНИЙ приклад (Дотримання принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  DIP - ГАРНИЙ ПРИКЛАД: Залежність від абстракцій           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПЕРЕВАГИ цього підходу:");
            Console.WriteLine("[+] UserServiceGood залежить виключно від АБСТРАКЦІЙ (інтерфейсів):");
            Console.WriteLine("   • ILogger (можна підставити файл, консоль, БД або Elastic)");
            Console.WriteLine("   • INotificationSender (email, SMS, push або месенджер)");
            Console.WriteLine("   • IUserRepository (MySQL, PostgreSQL, MongoDB чи in-memory mock)\n");

            Console.WriteLine("[+] Залежності передаються ззовні через конструктор (Constructor Dependency Injection)");
            Console.WriteLine("[+] Реалізація легко підміняється без зміни та рекомпіляції класу UserServiceGood");
            Console.WriteLine("[+] Бездоганна тестованість: можна передати mock/stub об'єкти за 1 рядок");
            Console.WriteLine("[+] Суворе дотримання DIP: як високорівневі, так і низькорівневі модулі залежать від абстракцій");
            Console.WriteLine("[+] Інверсія контролю (IoC): життєвий цикл залежностей контролюється ззовні\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 1: MySQL + Email + FileLogger\n");

            var service1 = new UserServiceGood(
                new FileLoggerImpl(),
                new SmtpNotificationSender(),
                new MySqlUserRepository()
            );
            service1.RegisterUser("user1@example.com", "Олександр");

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 2: PostgreSQL + SMS + ConsoleLogger\n");

            var service2 = new UserServiceGood(
                new ConsoleLoggerImpl(),
                new SmsNotificationSender(),
                new PostgresUserRepository()
            );
            service2.RegisterUser("user2@example.com", "Марія");

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 3: MongoDB + Push + DatabaseLogger\n");

            var service3 = new UserServiceGood(
                new DatabaseLoggerImpl(),
                new PushNotificationSender(),
                new MongoUserRepository()
            );
            service3.RegisterUser("user3@example.com", "Тарас");

            Console.WriteLine("\n\n[РЕЗУЛЬТАТ] ПРИНЦИП ІНВЕРСІЇ ЗАЛЕЖНОСТЕЙ (DIP) У ДІЇ:");
            Console.WriteLine("   Один і той самий незмінний UserServiceGood безшовно працює з:");
            Console.WriteLine("   • трьома різними базами даних (MySQL, Postgres, Mongo);");
            Console.WriteLine("   • трьома різними каналами логування (File, Console, Database);");
            Console.WriteLine("   • трьома різними провайдерами сповіщень (SMTP, SMS, Push).");
            Console.WriteLine("   І все це БЕЗ жодної модифікації вихідного коду класу UserServiceGood!");
            Console.WriteLine("\n   Для підключення Redis, Kafka чи Telegram достатньо лише реалізувати контракт.");
        }
    }
}
