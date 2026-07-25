using OOP___SOLID.S.SRPGoodExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S
{
    // 8. Координатор (Orchestrator / Facade) - відповідає ЛИШЕ за координацію процесу.
    // Він не виконує деталі роботи сам, а делегує їх спеціалізованим класам.
    public class OrderProcessorGood
    {
        private readonly OrderValidator _validator;
        private readonly DiscountCalculator _discountCalculator;
        private readonly OrderRepository _repository;
        private readonly EmailService _emailService;
        private readonly ReportGenerator _reportGenerator;
        private readonly Logger _logger;

        // Конструктор за замовчуванням (для спрощеного запуску демонстрації)
        public OrderProcessorGood()
            : this(new OrderValidator(),
                   new DiscountCalculator(),
                   new OrderRepository(),
                   new EmailService(),
                   new ReportGenerator(),
                   new Logger())
        {
        }

        // Повний конструктор (дозволяє підставляти залежності для тестування та гнучкості)
        public OrderProcessorGood(
            OrderValidator validator,
            DiscountCalculator discountCalculator,
            OrderRepository repository,
            EmailService emailService,
            ReportGenerator reportGenerator,
            Logger logger)
        {
            _validator = validator;
            _discountCalculator = discountCalculator;
            _repository = repository;
            _emailService = emailService;
            _reportGenerator = reportGenerator;
            _logger = logger;
        }

        public void ProcessOrder(string customerEmail, List<string> items, decimal totalPrice)
        {
            try
            {
                // Створюємо замовлення як модель даних
                var order = new Order
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 8),
                    CustomerEmail = customerEmail,
                    Items = items,
                    TotalPrice = totalPrice,
                    CreatedAt = DateTime.Now
                };

                // Кожна операція виконується окремим відповідальним класом
                _validator.Validate(order);
                order.Discount = _discountCalculator.Calculate(order.TotalPrice);
                _repository.Save(order);
                _emailService.SendOrderConfirmation(order);
                _reportGenerator.GeneratePdfReport(order);
                _logger.Log($"Замовлення №{order.Id} успішно оброблено");

                Console.WriteLine("\n[УСПІХ] Замовлення успішно оброблено всіма спеціалізованими сервісами.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Помилка: {ex.Message}");
                throw;
            }
        }
    }
}
