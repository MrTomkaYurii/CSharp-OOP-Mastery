using OOP___SOLID.D.DIPBadExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D
{
    public class DipBadExampleCommand : IMenuCommand
    {
        public string Title => "DIP - ПОГАНИЙ приклад (Порушення принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  DIP - ПОГАНИЙ ПРИКЛАД: Залежність від конкретних класів   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПРОБЛЕМИ цього підходу:");
            Console.WriteLine("[!] UserServiceBad прямо та жорстко залежить від конкретних реалізацій:");
            Console.WriteLine("   • FileLogger (не можна змінити спосіб логування без правки коду)");
            Console.WriteLine("   • SmtpEmailSender (не можна використати інший сервіс відправки)");
            Console.WriteLine("   • MySqlDatabase (жорстко прив'язані до конкретної СУБД MySQL)\n");

            Console.WriteLine("[-] Залежності створюються всередині класу через ключове слово new (Tight Coupling)");
            Console.WriteLine("[-] Неможливо підмінити залежності під час виконання без рекомпіляції");
            Console.WriteLine("[-] Неможливо писати Unit-тести - потрібні реальні файли, SMTP-сервер та MySQL");
            Console.WriteLine("[-] Якщо бізнес вимагає PostgreSQL замість MySQL - доведеться змінювати UserServiceBad");
            Console.WriteLine("[-] Модулі високого рівня (бізнес-логіка) прямо залежать від низькорівневих деталей (драйверів)");
            Console.WriteLine("[-] Пряме порушення DIP - відсутній шар абстракцій між архітектурними рівнями\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            var userService = new UserServiceBad();

            Console.WriteLine("Спроба 1: Реєстрація валідного користувача");
            Console.WriteLine(new string('═', 60));
            userService.RegisterUser("ivan@example.com", "Іван Петренко");

            Console.WriteLine("\n\nСпроба 2: Реєстрація з невалідним email");
            Console.WriteLine(new string('═', 60));
            try
            {
                userService.RegisterUser("invalid-email", "Тест");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ПОМИЛКА] {ex.Message}");
            }

            Console.WriteLine("\n\n[ВИСНОВОК]");
            Console.WriteLine("   Якщо виникає потреба:");
            Console.WriteLine("   • Зберігати логи у базі даних замість файлу;");
            Console.WriteLine("   • Відправляти SMS/Push замість SMTP;");
            Console.WriteLine("   • Перейти на PostgreSQL чи MongoDB замість MySQL;");
            Console.WriteLine("   -> Доведеться переписувати та повторно тестувати UserServiceBad!");
        }
    }
}
