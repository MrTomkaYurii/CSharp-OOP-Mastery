namespace DependencyInjection
{
    #region Інтерфейси та сервіси інфраструктури

    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        private readonly int _instanceId = Random.Shared.Next(1000, 9999);

        public void Log(string message)
        {
            Console.WriteLine($"      [Logger #{_instanceId}] {message}");
        }
    }

    public interface IDatabaseConnection
    {
        string ConnectionString { get; }
        void ExecuteQuery(string sql);
    }

    public class SqlDatabaseConnection : IDatabaseConnection
    {
        private readonly int _connectionId = Random.Shared.Next(100, 999);
        public string ConnectionString { get; }

        public SqlDatabaseConnection(string connectionString = "Server=localhost;Database=ShopDb;")
        {
            ConnectionString = connectionString;
        }

        public void ExecuteQuery(string sql)
        {
            Console.WriteLine($"      [DB Conn #{_connectionId}] Виконано SQL: {sql}");
        }
    }

    #endregion

    #region Доменні репозиторії та платіжні шлюзи

    public interface IOrderRepository
    {
        void SaveOrder(string orderId, decimal amount);
    }

    public class SqlOrderRepository : IOrderRepository
    {
        private readonly IDatabaseConnection _db;
        private readonly ILogger _logger;

        public SqlOrderRepository(IDatabaseConnection db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public void SaveOrder(string orderId, decimal amount)
        {
            _logger.Log($"Збереження замовлення {orderId} на суму {amount:C}");
            _db.ExecuteQuery($"INSERT INTO Orders (Id, Amount) VALUES ('{orderId}', {amount})");
        }
    }

    public interface IPaymentGateway
    {
        bool Charge(string customer, decimal amount);
    }

    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly ILogger _logger;

        public StripePaymentGateway(ILogger logger)
        {
            _logger = logger;
        }

        public bool Charge(string customer, decimal amount)
        {
            _logger.Log($"Списання коштів зі Stripe для {customer}: {amount:C}");
            return true;
        }
    }

    public interface INotificationService
    {
        void SendReceipt(string email, string message);
    }

    public class EmailNotificationService : INotificationService
    {
        private readonly ILogger _logger;

        public EmailNotificationService(ILogger logger)
        {
            _logger = logger;
        }

        public void SendReceipt(string email, string message)
        {
            _logger.Log($"Надіслано Email на {email}: {message}");
        }
    }

    #endregion

    #region Головний бізнес-сервіс (OrderService)

    public class OrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IPaymentGateway _payment;
        private readonly ILogger _logger;

        // Властивість, яка буде автоматично заповнена контейнером через Property Injection
        [InjectProperty]
        public INotificationService? NotificationService { get; set; }

        // Додатковий конструктор (не використовується контейнером)
        public OrderService()
        {
            throw new NotSupportedException("Конструктор без параметрів не повинен викликатися контейнером.");
        }

        // Цільовий конструктор, явно позначений атрибутом
        [InjectConstructor]
        public OrderService(IOrderRepository repository, IPaymentGateway payment, ILogger logger)
        {
            _repository = repository;
            _payment = payment;
            _logger = logger;
        }

        public void ProcessOrder(string orderId, string customer, decimal amount)
        {
            Console.WriteLine($"\n  🛒 === Початок обробки замовлення {orderId} ===");
            _logger.Log($"Валідація замовлення {orderId} для покупця {customer}");

            if (_payment.Charge(customer, amount))
            {
                _repository.SaveOrder(orderId, amount);

                if (NotificationService != null)
                {
                    NotificationService.SendReceipt($"{customer.ToLowerInvariant()}@mail.com", $"Ваше замовлення {orderId} успішно оплачено!");
                }
                else
                {
                    _logger.Log("⚠️ Повідомлення не надіслано: NotificationService не налаштовано.");
                }
            }
            Console.WriteLine($"  ✅ === Замовлення {orderId} успішно виконано ===\n");
        }
    }

    #endregion

    #region Моделі для перевірки виявлення циклічних залежностей (Circular Dependency)

    public class ServiceA
    {
        public ServiceA(ServiceB b) { }
    }

    public class ServiceB
    {
        public ServiceB(ServiceC c) { }
    }

    public class ServiceC
    {
        public ServiceC(ServiceA a) { }
    }

    #endregion
}
