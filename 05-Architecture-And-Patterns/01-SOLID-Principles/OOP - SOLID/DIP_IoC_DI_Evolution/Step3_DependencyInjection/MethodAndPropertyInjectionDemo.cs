using System;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step3_DependencyInjection
{
    // Інтерфейс для демонстрації Method Injection: форматування експорту звіту.
    public interface IReportFormatter
    {
        string Format(string data);
    }

    public class JsonReportFormatter : IReportFormatter
    {
        public string Format(string data) => $"{{ \"report\": \"{data}\", \"format\": \"JSON\" }}";
    }

    public class XmlReportFormatter : IReportFormatter
    {
        public string Format(string data) => $"<Report><Data>{data}</Data><Format>XML</Format></Report>";
    }

    // Клас для демонстрації всіх трьох видів Dependency Injection:
    // 1. Constructor Injection (Обов'язкова базова залежність - IUserRepository)
    // 2. Property Injection (Опціональна залежність з безпечним значенням за замовчуванням - ILogger)
    // 3. Method Injection (Залежність потрібна лише для однієї конкретної операції - IReportFormatter)
    public class UserReportService
    {
        private readonly IUserRepository _repository;

        // 2. Property (Setter) Injection:
        // Використовується для опціональних залежностей. Якщо значення не передано, діє безпечний дефолт.
        public ILogger Logger { get; set; } = new ConsoleLogger();

        // 1. Constructor Injection:
        // Обов'язкова залежність, без якої сервіс не може функціонувати в принципі.
        public UserReportService(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // 3. Method Injection:
        // Залежність IReportFormatter потрібна ВИКЛЮЧНО цьому методу під час його виклику.
        // Зберігати її як поле класу було б нераціонально (over-engineering).
        public string ExportUserReport(string userId, IReportFormatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            Logger.LogInfo($"[Method Injection] Формування звіту для користувача {userId}...");

            string userData = _repository.GetById(userId) ?? "Анонім";
            string formattedResult = formatter.Format(userData);

            Logger.LogInfo($"[Method Injection] Звіт сформовано у форматі {formatter.GetType().Name}.");
            return formattedResult;
        }
    }
}
