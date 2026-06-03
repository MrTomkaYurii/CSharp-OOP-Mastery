// Приклад демонструє нові можливості інтерфейсів, що з'явилися починаючи з C# 8.0:
// default-реалізації методів інтерфейсу, статичні поля/методи (у т.ч. static factory)
// прямо в інтерфейсі, приватні/захищені (protected) члени інтерфейсу та явні модифікатори
// доступу (public/internal/private/protected) для методів інтерфейсу.
// Кожен розділ (1-4) — окрема задача: платіжна система з логуванням, фабрика документів,
// сервіс аутентифікації з прихованою внутрішньою логікою та API з різними рівнями доступу.
// Мета — показати, що інтерфейс у сучасному C# може містити не лише "контракт", а й готову
// реалізацію поведінки, яку класи-реалізатори отримують "безкоштовно".
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp8InterfaceFeatures
{
    // ========== 1. DEFAULT INTERFACE METHODS ==========
    // Реалістичний приклад: Платіжна система з логуванням транзакцій
    interface IPaymentProcessor
    {
        decimal ProcessPayment(decimal amount, string currency);

        // Default метод - автоматичне логування всіх платежів
        void LogTransaction(decimal amount, string currency, bool success)
        {
            var timestamp = DateTime.Now;
            var status = success ? "SUCCESS" : "FAILED";
            Console.WriteLine($"[{timestamp:yyyy-MM-dd HH:mm:ss}] {status}: {amount} {currency}");
        }

        // Default метод для валідації
        bool ValidateAmount(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("❌ Сума повинна бути більше нуля");
                return false;
            }
            if (amount > 1000000)
            {
                Console.WriteLine("❌ Сума перевищує ліміт транзакції");
                return false;
            }
            return true;
        }
    }

    // Реалізує лише обов'язковий метод ProcessPayment(); LogTransaction() та
    // ValidateAmount() клас отримує "готовими" з default-реалізації інтерфейсу.
    class CreditCardProcessor : IPaymentProcessor
    {
        public decimal ProcessPayment(decimal amount, string currency)
        {

            // Явне приведення до інтерфейсу для доступу до default методів
            IPaymentProcessor processor = this; //Якщо не робити приведення, то default методи інтерфейсу недоступні в класі.


                            // interface ILogger
                            // {
                            //    void LogInfo(string message) => Console.WriteLine($"INFO: {message}");
                            // }
                            // class MyLogger : ILogger
                            // {
                            //    public void Test()
                            //    {
                            //        ((ILogger)this).LogInfo("Hello"); // OK
                            //    }
                            // }

                            // if (!((IPaymentProcessor)this).ValidateAmount(amount)) return 0;

            if (!processor.ValidateAmount(amount)) return 0;

            Console.WriteLine($"💳 Обробка платежу картою: {amount} {currency}");
            var fee = amount * 0.029m; // 2.9% комісія
            processor.LogTransaction(amount, currency, true);
            return fee;
        }
    }

    // Другий реалізатор того ж інтерфейсу: своя бізнес-логіка (менша комісія),
    // але ті самі default-методи логування й валідації, успадковані з IPaymentProcessor.
    class CryptoProcessor : IPaymentProcessor
    {
        public decimal ProcessPayment(decimal amount, string currency)
        {
            // Явне приведення до інтерфейсу для доступу до default методів
            IPaymentProcessor processor = this; //Якщо не робити приведення, то default методи інтерфейсу недоступні в класі.

            if (!processor.ValidateAmount(amount)) return 0;

            Console.WriteLine($"₿ Обробка криптоплатежу: {amount} {currency}");
            var fee = amount * 0.01m; // 1% комісія
            processor.LogTransaction(amount, currency, true);
            return fee;
        }
    }



    // ========== 2. STATIC MEMBERS IN INTERFACES ==========
    // Реалістичний приклад: Фабрика для створення документів різних форматів
    interface IDocument
    {
        void Generate();
        string GetContent();

        // Статичне поле - підтримувані формати
        static readonly List<string> SupportedFormats = new List<string>
        {
            "PDF", "DOCX", "HTML", "XML"
        };

        // Статичний метод-фабрика
        static IDocument CreateDocument(string format, string content)
        {
            if (!SupportedFormats.Contains(format.ToUpper()))
            {
                throw new ArgumentException($"Формат {format} не підтримується");
            }

            //switch expression
            //var result = value switch
            //{
            //    case1 => value1,
            //    case2 => value2,
            //    _ => defaultValue // Любий інший випадок   
            //};


            return format.ToUpper() switch
            {
                "PDF" => new PdfDocument(content),
                "DOCX" => new DocxDocument(content),
                "HTML" => new HtmlDocument(content),
                "XML" => new XmlDocument(content),
                _ => throw new NotImplementedException()    
            };
        }

        // Статичний метод для отримання інформації про формат
        static string GetFormatInfo(string format)
        {
            return format.ToUpper() switch
            {
                "PDF" => "Portable Document Format - для друку та архівування",
                "DOCX" => "Microsoft Word - для редагування",
                "HTML" => "Web-документ - для публікації в інтернеті",
                "XML" => "Структурований формат - для обміну даними",
                _ => "Невідомий формат"
            };
        }
    }

    // Конкретні формати документів реалізують лише два обов'язкові методи інтерфейсу
    // (Generate/GetContent). Об'єкти цих класів створює статична фабрика IDocument.CreateDocument().
    class PdfDocument : IDocument
    {
        private readonly string content;
        public PdfDocument(string content) => this.content = content;

        public void Generate() => Console.WriteLine("📄 Генерація PDF документу...");
        public string GetContent() => $"[PDF] {content}";
    }

    class DocxDocument : IDocument
    {
        private readonly string content;
        public DocxDocument(string content) => this.content = content;

        public void Generate() => Console.WriteLine("📝 Генерація DOCX документу...");
        public string GetContent() => $"[DOCX] {content}";
    }

    class HtmlDocument : IDocument
    {
        private readonly string content;
        public HtmlDocument(string content) => this.content = content;

        public void Generate() => Console.WriteLine("🌐 Генерація HTML документу...");
        public string GetContent() => $"<html><body>{content}</body></html>";
    }

    class XmlDocument : IDocument
    {
        private readonly string content;
        public XmlDocument(string content) => this.content = content;

        public void Generate() => Console.WriteLine("📋 Генерація XML документу...");
        public string GetContent() => $"<document>{content}</document>";
    }

    // ========== 3. PRIVATE/PROTECTED MEMBERS ==========
    // Реалістичний приклад: Аутентифікація з внутрішньою логікою
    interface IAuthenticationService
    {
        bool Authenticate(string username, string password);

        // Приватний метод для хешування - не доступний зовні
        private string HashPassword(string password)
        {
            // Спрощена версія хешування (в реальності використовувати BCrypt)
            int hash = 7;
            foreach (char c in password)
            {
                hash = hash * 31 + c;
            }
            return $"HASH_{Math.Abs(hash):X8}";
        }

        // Приватний метод для перевірки складності пароля
        private bool ValidatePasswordComplexity(string password)
        {
            if (password.Length < 8)
            {
                Console.WriteLine("⚠️  Пароль занадто короткий (мінімум 8 символів)");
                return false;
            }

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);

            if (!hasUpper || !hasLower || !hasDigit)
            {
                Console.WriteLine("⚠️  Пароль повинен містити великі, малі літери та цифри");
                return false;
            }

            return true;
        }

        // Публічний метод з валідацією
        bool RegisterUser(string username, string password)
        {
            if (!ValidatePasswordComplexity(password))
                return false;

            string hashedPassword = HashPassword(password);
            Console.WriteLine($"✅ Користувач {username} зареєстрований");
            Console.WriteLine($"   Хеш пароля: {hashedPassword}");
            return true;
        }
    }

    // Реалізує лише Authenticate(); метод RegisterUser() та приватні helper-методи
    // (HashPassword, ValidatePasswordComplexity) успадковуються з інтерфейсу як default-реалізація.
    class DatabaseAuthService : IAuthenticationService
    {
        private Dictionary<string, string> users = new Dictionary<string, string>();

        public bool Authenticate(string username, string password)
        {
            // Використовує приватний метод HashPassword з інтерфейсу
            Console.WriteLine($"🔐 Спроба входу користувача: {username}");

            if (users.ContainsKey(username))
            {
                Console.WriteLine("✅ Аутентифікація успішна");
                return true;
            }

            Console.WriteLine("❌ Користувач не знайдений");
            return false;
        }
    }

    class LdapAuthService : IAuthenticationService
    {
        public bool Authenticate(string username, string password)
        {
            Console.WriteLine($"🔐 LDAP аутентифікація: {username}");
            Console.WriteLine("✅ Підключення до Active Directory успішне");
            return true;
        }
    }

    // ========== 4. EXPLICIT ACCESS MODIFIERS ==========
    // Реалістичний приклад: API ендпоінти з різними рівнями доступу
    interface IApiEndpoint
    {
        // Публічний метод - доступний всім
        public string HandleRequest(string request);

        // Внутрішній метод - для логування (доступний в межах assembly)
        internal void LogAccess(string endpoint, string user)
        {
            Console.WriteLine($"📊 Access Log: {user} → {endpoint} at {DateTime.Now:HH:mm:ss}");
        }

        // Приватний метод - для внутрішньої обробки
        private string FormatResponse(object data)
        {
            return $"{{\"status\": \"success\", \"data\": \"{data}\"}}";
        }

        // Default публічний метод з використанням приватного
        string ProcessAndFormat(string data)
        {
            return FormatResponse(data);
        }

        // Default метод для валідації (публічний, але з логікою)
        bool ValidateApiKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("⚠️  API ключ не може бути порожнім");
                return false;
            }

            if (!apiKey.StartsWith("sk_"))
            {
                Console.WriteLine("⚠️  Невалідний формат API ключа (має починатися з 'sk_')");
                return false;
            }

            if (apiKey.Length < 15)
            {
                Console.WriteLine("⚠️  API ключ занадто короткий");
                return false;
            }

            return true;
        }
    }

    // Розширений інтерфейс з protected методами (для демонстрації)
    interface ISecureApiEndpoint : IApiEndpoint
    {
        // Protected метод - доступний тільки в похідних інтерфейсах
        protected string EncryptData(string data)
        {
            // Проста "шифрування" для демонстрації
            var encrypted = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(data)
            );
            return $"ENC_{encrypted}";
        }

        // Використання protected методу всередині інтерфейсу
        string HandleSecureRequest(string request)
        {
            var encrypted = EncryptData(request);
            return $"{{\"encrypted\": \"{encrypted}\"}}";
        }
    }

    class PublicApiEndpoint : IApiEndpoint
    {
        public string HandleRequest(string request)
        {
            // Явне приведення до інтерфейсу для доступу до internal методів
            IApiEndpoint endpoint = this;
            endpoint.LogAccess("/public", "anonymous");
            return endpoint.ProcessAndFormat($"Public data: {request}");
        }
    }

    class ProtectedApiEndpoint : IApiEndpoint
    {
        private readonly string apiKey;

        public ProtectedApiEndpoint(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public string HandleRequest(string request)
        {
            // Явне приведення до інтерфейсу
            IApiEndpoint endpoint = this;

            if (!endpoint.ValidateApiKey(apiKey))
            {
                return "{\"error\": \"Invalid API key\"}";
            }

            endpoint.LogAccess("/protected", "authenticated_user");
            return endpoint.ProcessAndFormat($"Protected data: {request}");
        }
    }

    class SecureApiEndpoint : ISecureApiEndpoint
    {
        private readonly string apiKey;

        public SecureApiEndpoint(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public string HandleRequest(string request)
        {
            ISecureApiEndpoint endpoint = this;

            if (!endpoint.ValidateApiKey(apiKey))
            {
                return "{\"error\": \"Invalid API key\"}";
            }

            endpoint.LogAccess("/secure", "admin_user");

            // Використання методу з розширеного інтерфейсу
            return endpoint.HandleSecureRequest(request);
        }
    }

    // ========== MAIN PROGRAM ==========
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
                Console.WriteLine("║     C# 8.0 INTERFACE FEATURES - ПРАКТИЧНА ДЕМОНСТРАЦІЯ    ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("  1️  Default Interface Methods (Платіжна система)");
                Console.WriteLine("  2️  Static Members in Interfaces (Фабрика документів)");
                Console.WriteLine("  3️  Private/Protected Members (Система аутентифікації)");
                Console.WriteLine("  4️  Explicit Access Modifiers (API ендпоінти)");
                Console.WriteLine("  0️  Вихід");
                Console.WriteLine();
                Console.Write("➤ Оберіть опцію: ");

                var choice = Console.ReadLine();

                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DemoDefaultMethods();
                        break;
                    case "2":
                        DemoStaticMembers();
                        break;
                    case "3":
                        DemoPrivateMembers();
                        break;
                    case "4":
                        DemoAccessModifiers();
                        break;
                    case "0":
                        Console.WriteLine("До побачення! 👋");
                        return;
                    default:
                        Console.WriteLine("❌ Невірний вибір!");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        static void DemoDefaultMethods()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("  🔹 DEFAULT INTERFACE METHODS");
            Console.WriteLine("  Інтерфейс IPaymentProcessor містить default методи:");
            Console.WriteLine("  - LogTransaction() - логування всіх транзакцій");
            Console.WriteLine("  - ValidateAmount() - валідація суми платежу");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine();

            IPaymentProcessor creditCard = new CreditCardProcessor();
            IPaymentProcessor crypto = new CryptoProcessor();

            Console.WriteLine("Тест 1: Оплата кредитною карткою");
            Console.WriteLine("─────────────────────────────────");
            var fee1 = creditCard.ProcessPayment(5000, "UAH");
            Console.WriteLine($"Комісія: {fee1:F2} UAH\n");

            Console.WriteLine("Тест 2: Оплата криптовалютою");
            Console.WriteLine("─────────────────────────────────");
            var fee2 = crypto.ProcessPayment(3000, "USDT");
            Console.WriteLine($"Комісія: {fee2:F2} USDT\n");

            Console.WriteLine("Тест 3: Некоректна сума (використовується ValidateAmount)");
            Console.WriteLine("─────────────────────────────────");
            creditCard.ProcessPayment(-100, "UAH");
            Console.WriteLine();

            Console.WriteLine("Тест 4: Перевищення ліміту");
            Console.WriteLine("─────────────────────────────────");
            crypto.ProcessPayment(2000000, "BTC");
        }

        static void DemoStaticMembers()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("  🔹 STATIC MEMBERS IN INTERFACES");
            Console.WriteLine("  Інтерфейс IDocument містить:");
            Console.WriteLine("  - Статичне поле SupportedFormats");
            Console.WriteLine("  - Статичний метод-фабрику CreateDocument()");
            Console.WriteLine("  - Статичний метод GetFormatInfo()");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine();

            Console.WriteLine("Підтримувані формати:");
            Console.WriteLine("─────────────────────────────────");
            foreach (var format in IDocument.SupportedFormats)
            {
                Console.WriteLine($"  • {format}: {IDocument.GetFormatInfo(format)}");
            }
            Console.WriteLine();

            string content = "Фінансовий звіт за Q3 2025";

            Console.WriteLine("Генерація документів різних форматів:");
            Console.WriteLine("─────────────────────────────────");
            foreach (var format in IDocument.SupportedFormats)
            {
                var doc = IDocument.CreateDocument(format, content);
                doc.Generate();
                Console.WriteLine($"  Контент: {doc.GetContent()}");
                Console.WriteLine();
            }

            Console.WriteLine("Спроба створити документ невідомого формату:");
            Console.WriteLine("─────────────────────────────────");
            try
            {
                IDocument.CreateDocument("TXT", content);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ❌ {ex.Message}");
            }
        }

        static void DemoPrivateMembers()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("  🔹 PRIVATE/PROTECTED MEMBERS IN INTERFACES");
            Console.WriteLine("  Інтерфейс IAuthenticationService містить:");
            Console.WriteLine("  - private HashPassword() - недоступний зовні");
            Console.WriteLine("  - private ValidatePasswordComplexity()");
            Console.WriteLine("  - public RegisterUser() - використовує приватні методи");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine();

            IAuthenticationService dbAuth = new DatabaseAuthService();
            IAuthenticationService ldapAuth = new LdapAuthService();

            Console.WriteLine("Тест 1: Реєстрація зі слабким паролем");
            Console.WriteLine("─────────────────────────────────");
            dbAuth.RegisterUser("user1", "123");
            Console.WriteLine();

            Console.WriteLine("Тест 2: Реєстрація з паролем без цифр");
            Console.WriteLine("─────────────────────────────────");
            dbAuth.RegisterUser("user2", "Password");
            Console.WriteLine();

            Console.WriteLine("Тест 3: Реєстрація з правильним паролем");
            Console.WriteLine("─────────────────────────────────");
            dbAuth.RegisterUser("user3", "SecurePass123");
            Console.WriteLine();

            Console.WriteLine("Тест 4: Аутентифікація через різні сервіси");
            Console.WriteLine("─────────────────────────────────");
            dbAuth.Authenticate("user3", "SecurePass123");
            Console.WriteLine();
            ldapAuth.Authenticate("admin", "AdminPass123");
        }

        static void DemoAccessModifiers()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("  🔹 EXPLICIT ACCESS MODIFIERS");
            Console.WriteLine("  Інтерфейс IApiEndpoint містить методи з різними рівнями:");
            Console.WriteLine("  - public HandleRequest()");
            Console.WriteLine("  - internal LogAccess() - доступний в межах assembly");
            Console.WriteLine("  - private FormatResponse() - тільки всередині інтерфейсу");
            Console.WriteLine("  ISecureApiEndpoint додає:");
            Console.WriteLine("  - protected EncryptData() - для похідних інтерфейсів");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine();

            IApiEndpoint publicApi = new PublicApiEndpoint();
            IApiEndpoint protectedApi = new ProtectedApiEndpoint("sk_live_abc123xyz");
            IApiEndpoint invalidApi = new ProtectedApiEndpoint("invalid_key");
            ISecureApiEndpoint secureApi = new SecureApiEndpoint("sk_secure_admin_xyz789");

            Console.WriteLine("Тест 1: Публічний API ендпоінт");
            Console.WriteLine("─────────────────────────────────");
            var response1 = publicApi.HandleRequest("get_public_stats");
            Console.WriteLine($"  Відповідь: {response1}");
            Console.WriteLine();

            Console.WriteLine("Тест 2: Захищений API ендпоінт з валідним ключем");
            Console.WriteLine("─────────────────────────────────");
            var response2 = protectedApi.HandleRequest("get_user_data");
            Console.WriteLine($"  Відповідь: {response2}");
            Console.WriteLine();

            Console.WriteLine("Тест 3: Захищений API ендпоінт з невалідним ключем");
            Console.WriteLine("─────────────────────────────────");
            var response3 = invalidApi.HandleRequest("get_user_data");
            Console.WriteLine($"  Відповідь: {response3}");
            Console.WriteLine();

            Console.WriteLine("Тест 4: Безпечний API з шифруванням (protected метод)");
            Console.WriteLine("─────────────────────────────────");
            var response4 = secureApi.HandleRequest("sensitive_data_12345");
            Console.WriteLine($"  Відповідь: {response4}");
        }
    }
}