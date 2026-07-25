using System;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step3_DependencyInjection
{
    // Демонстрація Кроку 3: Dependency Injection (DI) та концепції Pure DI (Ручне впровадження).
    public static class PureDiDemo
    {
        public static void Run()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  КРОК 3. DEPENDENCY INJECTION (DI) & PURE DI               ");
            Console.WriteLine("  Патерн проектування: Впровадження залежностей ззовні      ");
            Console.WriteLine("============================================================\n");

            Console.WriteLine("ТЕОРЕТИЧНЕ ОБҐРУНТУВАННЯ:");
            Console.WriteLine("1. Перехід від PULL (Абстрактна Фабрика) до PUSH (Dependency Injection):");
            Console.WriteLine("   Сервіс більше не просить: 'Фабрико, дай мені репозиторій'.");
            Console.WriteLine("   Сервіс пасивно очікує: залежності впорскуються у конструктор.\n");

            Console.WriteLine("2. Що таке 'Pure DI' (Чистий / Ручний DI без контейнерів)?");
            Console.WriteLine("   Це створення графа об'єктів у точці входу (Composition Root)");
            Console.WriteLine("   за допомогою звичайних конструкторів мови C# (new).\n");

            Console.WriteLine("--- СЦЕНАРІЙ 1: Constructor Injection (Продакшн середовище) ---");
            IUserRepository prodRepo = new MySqlUserRepository();
            INotificationSender prodNotifier = new SmtpNotificationSender();
            ILogger prodLogger = new ConsoleLogger();

            var prodService = new UserServiceWithDI(prodRepo, prodNotifier, prodLogger);
            prodService.RegisterUser("usr_301", "Валерій Залужний", "zaluzhnyi@army.ua");

            Console.WriteLine("\n--- СЦЕНАРІЙ 2: Тестування через Fake/Mock (Ізольований тест) ---");
            var fakeRepo = new FakeUserRepository();
            var testService = new UserServiceWithDI(fakeRepo, prodNotifier, prodLogger);
            testService.RegisterUser("test_1", "Тестовий Користувач", "test@domain.com");

            Console.WriteLine($"[ПЕРЕВІРКА ТЕСТУ]: Репозиторій зафіксував викликів Save: {fakeRepo.SaveCallCount} (Успішно!)");

            Console.WriteLine("\n--- СЦЕНАРІЙ 3: Демонстрація Method Injection та Property Injection ---");
            var reportService = new UserReportService(prodRepo);

            // Property Injection: замінюємо дефолтний ConsoleLogger на FileLogger
            reportService.Logger = new FileLogger();

            // Method Injection: передаємо форматер Json безпосередньо у метод
            string jsonReport = reportService.ExportUserReport("usr_301", new JsonReportFormatter());
            Console.WriteLine($"Результат (JSON): {jsonReport}");

            // Method Injection: передаємо форматер Xml у той самий сервіс
            string xmlReport = reportService.ExportUserReport("usr_301", new XmlReportFormatter());
            Console.WriteLine($"Результат (XML):  {xmlReport}\n");

            Console.WriteLine("ПРОБЛЕМА PURE DI, ЯКА ПРИЗВЕЛА ДО СТВОРЕННЯ IoC-КОНТЕЙНЕРІВ:");
            Console.WriteLine("Коли в реальній системі не 3 класи, а 200:");
            Console.WriteLine("   var svc = new OrderService(");
            Console.WriteLine("       new CustomerRepo(new DbConnection(config)),");
            Console.WriteLine("       new TaxCalculator(new ExchangeRateService(new HttpClient())),");
            Console.WriteLine("       new InvoiceGenerator(new PdfEngine(new FontProvider())),");
            Console.WriteLine("       new EmailNotifier(new SmtpClient(config)),");
            Console.WriteLine("       ... і так далі до нескінченності);");
            Console.WriteLine("Виникає 'Constructor Hell' (Пекло ручного конструювання)!");
            Console.WriteLine("Це веде нас до Кроку 4: IoC Контейнери — автоматизовані фабрики об'єктів!");
        }
    }
}
