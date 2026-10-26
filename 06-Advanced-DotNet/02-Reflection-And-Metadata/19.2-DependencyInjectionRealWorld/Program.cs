using DependencyInjection.Container;
using DependencyInjection.Data;
using DependencyInjection.Models;
using DependencyInjection.Repositories;
using DependencyInjection.Services.Common;
using DependencyInjection.Services.Diagnostics;
using DependencyInjection.Services.Notifications;
using DependencyInjection.Services.Orders;
using DependencyInjection.Services.Payments;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 19.2. РЕАЛЬНИЙ ПРОМИСЛОВИЙ IoC/DI КОНТЕЙНЕР (REAL-WORLD MODULAR ARCHITECTURE)");
Console.WriteLine(" Демонстрація системи сервісів, репозиторіїв та емуляції бази даних");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Етап 1: Конфігурація та реєстрація компонентів у IoC-контейнері
// -----------------------------------------------------------------------
Console.WriteLine("--- Етап 1: Реєстрація сервісів, репозиторіїв та контексту бази даних ---");
IContainer container = new Container();

// Singleton: єдиний екземпляр на весь життєвий цикл додатка
container.RegisterSingleton<ILogger, ConsoleLogger>();
container.RegisterSingleton<IDatabaseContext, InMemoryDatabaseContext>();

// Transient: створення нового екземпляра при кожному розв'язанні залежності
container.RegisterTransient<IProductRepository, ProductRepository>();
container.RegisterTransient<IOrderRepository, OrderRepository>();
container.RegisterTransient<IPaymentService, CreditCardPaymentService>();
container.RegisterTransient<INotificationService, EmailNotificationService>();
container.RegisterTransient<IOrderProcessingService, OrderProcessingService>();

// -----------------------------------------------------------------------
// Етап 2: Розв'язання дерева залежностей (Recursive Resolution)
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("--- Етап 2: Рекурсивне розв'язання залежностей для IOrderProcessingService ---");
Console.WriteLine("--------------------------------------------------------------------------");

// Контейнер виконує:
// 1. Пошук конструктора OrderProcessingService з атрибутом [InjectConstructor].
// 2. Аналіз параметрів (IDatabaseContext, IProductRepository, IOrderRepository, IPaymentService, ILogger).
// 3. Рекурсивне створення кожного параметра (репозиторії отримують спільний Singleton IDatabaseContext).
// 4. Property Injection: виявлення властивості NotificationService з [InjectProperty] та її заповнення.
var orderService = container.Resolve<IOrderProcessingService>();

// -----------------------------------------------------------------------
// Етап 3: Огляд початкового стану емуляції бази даних
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("--- Етап 3: Огляд каталогу товарів та клієнтів з InMemoryDatabaseContext ---");
Console.WriteLine("--------------------------------------------------------------------------");

var dbContext = container.Resolve<IDatabaseContext>();

Console.WriteLine("Наявні товари в базі даних:");
foreach (var product in dbContext.Products)
{
    Console.WriteLine($"  {product}");
}

Console.WriteLine("\nЗареєстровані клієнти:");
foreach (var customer in dbContext.Customers)
{
    Console.WriteLine($"  {customer}");
}

// -----------------------------------------------------------------------
// Етап 4: Виконання бізнес-сценаріїв через консольну аплікацію
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("--- Етап 4: Виконання бізнес-операцій оформлення замовлень ---");
Console.WriteLine("--------------------------------------------------------------------------");

// Успішне замовлення 1: Олександр купує 2 ноутбуки Dell
orderService.PlaceOrder(customerId: 1, productId: 1, quantity: 2);

// Успішне замовлення 2: Марія купує 1 навушники Sony
orderService.PlaceOrder(customerId: 2, productId: 3, quantity: 1);

// Спроба замовлення з помилкою валідації: запит на 10 ноутбуків при залишку 3
Console.WriteLine("Спроба замовити товар у кількості, що перевищує складський залишок:");
orderService.PlaceOrder(customerId: 3, productId: 1, quantity: 10);

// Виведення збережених замовлень у базі даних
orderService.PrintOrderHistory();

// -----------------------------------------------------------------------
// Етап 5: Перевірка коректності життєвих циклів (Lifetimes Verification)
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("--- Етап 5: Верифікація життєвих циклів (Singleton vs Transient) ---");
Console.WriteLine("--------------------------------------------------------------------------");

var dbContext1 = container.Resolve<IDatabaseContext>();
var dbContext2 = container.Resolve<IDatabaseContext>();
bool isSameDbContext = ReferenceEquals(dbContext1, dbContext2);
Console.WriteLine($"IDatabaseContext (Singleton): посилання однакові -> {isSameDbContext} (Очікувано: True)");
Console.WriteLine($"Кількість замовлень у другому екземплярі DbContext: {dbContext2.Orders.Count} (стан зберігся!)");

var repo1 = container.Resolve<IOrderRepository>();
var repo2 = container.Resolve<IOrderRepository>();
bool isSameRepo = ReferenceEquals(repo1, repo2);
Console.WriteLine($"IOrderRepository (Transient): посилання різні -> {!isSameRepo} (Очікувано: True)");

// -----------------------------------------------------------------------
// Етап 6: Демонстрація перехоплення циклічних залежностей (Circular Dependency)
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("--- Етап 6: Демонстрація захисту від циклічних залежностей ---");
Console.WriteLine("--------------------------------------------------------------------------");

IContainer diagnosticContainer = new Container();
diagnosticContainer.RegisterTransient<ServiceA>();
diagnosticContainer.RegisterTransient<ServiceB>();
diagnosticContainer.RegisterTransient<ServiceC>();

try
{
    Console.WriteLine("Спроба розв'язати граф із циклом (ServiceA -> ServiceB -> ServiceC -> ServiceA)...");
    diagnosticContainer.Resolve<ServiceA>();
}
catch (CircularDependencyException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"[ПЕРЕХОПЛЕНО ЦИКЛ]: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("Пояснення: IoC-контейнер виявив повторний запит типу у стеку викликів і запобіг аварії StackOverflowException.");
}

Console.WriteLine("\n==========================================================================");
Console.WriteLine(" Демонстрацію проєкту 19-DependencyInjection успішно завершено.");
Console.WriteLine("==========================================================================");
