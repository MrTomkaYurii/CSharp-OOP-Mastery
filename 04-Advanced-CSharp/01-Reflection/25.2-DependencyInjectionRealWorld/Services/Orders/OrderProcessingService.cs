using DependencyInjection.Container;
using DependencyInjection.Data;
using DependencyInjection.Models;
using DependencyInjection.Repositories;
using DependencyInjection.Services.Common;
using DependencyInjection.Services.Notifications;
using DependencyInjection.Services.Payments;

namespace DependencyInjection.Services.Orders;

/// <summary>
/// Головний бізнес-сервіс інтернет-магазину.
/// Демонструє комплексне використання IoC/DI контейнера:
/// 1. Впровадження залежностей через конструктор (Constructor Injection) з маркуванням [InjectConstructor].
/// 2. Впровадження залежності через властивість (Property Injection) з маркуванням [InjectProperty].
/// 3. Координацію роботи між контекстом бази даних, репозиторіями, платіжною системою та логуванням.
/// </summary>
public class OrderProcessingService : IOrderProcessingService
{
    private readonly IDatabaseContext _dbContext;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly ILogger _logger;

    /// <summary>
    /// Властивість для Property Injection: заповнюється контейнером після створення екземпляра.
    /// </summary>
    [InjectProperty]
    public INotificationService? NotificationService { get; set; }

    /// <summary>
    /// Конструктор без параметрів. Завдяки наявності атрибута [InjectConstructor] на цільовому конструкторі,
    /// IoC-контейнер не викликатиме цей метод.
    /// </summary>
    public OrderProcessingService()
    {
        throw new NotSupportedException("Конструктор без параметрів не повинен викликатися контейнером за наявності [InjectConstructor].");
    }

    /// <summary>
    /// Цільовий конструктор для рефлексії контейнера.
    /// </summary>
    [InjectConstructor]
    public OrderProcessingService(
        IDatabaseContext dbContext,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IPaymentService paymentService,
        ILogger logger)
    {
        _dbContext = dbContext;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _paymentService = paymentService;
        _logger = logger;
    }

    public Order? PlaceOrder(int customerId, int productId, int quantity)
    {
        Console.WriteLine($"\n--- Початок обробки замовлення (Клієнт ID: {customerId}, Товар ID: {productId}, Кількість: {quantity}) ---");

        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            _logger.Log($"Помилка: Клієнта з ID {customerId} не знайдено в базі даних.");
            return null;
        }

        var product = _productRepository.GetById(productId);
        if (product == null)
        {
            _logger.Log($"Помилка: Товару з ID {productId} не знайдено в каталозі.");
            return null;
        }

        if (product.StockQuantity < quantity)
        {
            _logger.Log($"Помилка: Недостатньо товару на складі '{product.Name}' (запитано {quantity}, залишок {product.StockQuantity}).");
            return null;
        }

        decimal totalAmount = product.Price * quantity;
        _logger.Log($"Клієнт '{customer.FullName}' купує '{product.Name}' x{quantity} на суму {totalAmount:C}.");

        // 1. Оплата через платіжний сервіс
        var paymentResult = _paymentService.ProcessPayment(customer.FullName, totalAmount);
        if (!paymentResult.Success)
        {
            _logger.Log($"Відмова платежу: {paymentResult.Message}");
            return null;
        }

        // 2. Зменшення залишків у репозиторії
        _productRepository.UpdateStock(productId, -quantity);

        // 3. Створення та фіксація замовлення в базі даних
        string orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        var order = new Order(orderNumber, customerId, productId, quantity, totalAmount)
        {
            Status = OrderStatus.Paid
        };

        _orderRepository.Add(order);

        // 4. Відправка повідомлення (через Property Injection)
        if (NotificationService != null)
        {
            NotificationService.SendNotification(
                customer.Email,
                $"Оплата замовлення #{orderNumber} успішна",
                $"Шановний(а) {customer.FullName}, товар '{product.Name}' ({quantity} шт.) успішно оплачено на суму {totalAmount:C}.");
        }
        else
        {
            _logger.Log("Увага: NotificationService не налаштовано; повідомлення не надіслано.");
        }

        Console.WriteLine($"--- Замовлення #{orderNumber} успішно виконано та зафіксовано в базі даних ---\n");
        return order;
    }

    public void PrintOrderHistory()
    {
        var orders = _orderRepository.GetAll();
        Console.WriteLine("\n--- Перегляд збережених замовлень у базі даних (Orders Collection) ---");
        if (orders.Count == 0)
        {
            Console.WriteLine("  Список замовлень порожній.");
            return;
        }

        foreach (var order in orders)
        {
            Console.WriteLine($"  {order}");
        }
    }
}
