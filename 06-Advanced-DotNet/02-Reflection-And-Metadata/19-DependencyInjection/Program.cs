using DependencyInjection;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 🚀 ФІНАЛЬНИЙ ПРОЕКТ: ВЛАСНИЙ IoC/DI КОНТЕЙНЕР НА БАЗІ РЕФЛЕКСІЇ");
Console.WriteLine("==========================================================================\n");

// 1. Ініціалізація контейнера
var container = new Container();

Console.WriteLine("📋 Крок 1: Реєстрація сервісів та життєвих циклів у контейнері:");
// Singleton: єдиний екземпляр на весь час життя додатка
container.RegisterSingleton<ILogger, ConsoleLogger>();
container.RegisterSingleton<IDatabaseConnection, SqlDatabaseConnection>();

// Transient: новий екземпляр при кожному запиті
container.RegisterTransient<IOrderRepository, SqlOrderRepository>();
container.RegisterTransient<IPaymentGateway, StripePaymentGateway>();
container.RegisterTransient<INotificationService, EmailNotificationService>();

// Реєстрація класу замовлень
container.RegisterTransient<OrderService>();

Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("🔍 Крок 2: Розв'язання графу залежностей (Recursive Resolution & Tree):");
Console.WriteLine("--------------------------------------------------------------------------");

// Вирішуємо сервіс OrderService
// Контейнер автоматично:
//  - знаходить конструктор з [InjectConstructor]
//  - рекурсивно створює IOrderRepository -> SqlDatabaseConnection, ILogger
//  - створює IPaymentGateway -> ILogger (використовує існуючий Singleton)
//  - знаходить властивість з [InjectProperty] та інжектує INotificationService!
var orderService = container.Resolve<OrderService>();

Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("🧪 Крок 3: Перевірка життєвих циклів (Singleton vs Transient):");
Console.WriteLine("--------------------------------------------------------------------------");

var logger1 = container.Resolve<ILogger>();
var logger2 = container.Resolve<ILogger>();
bool isSameLogger = ReferenceEquals(logger1, logger2);
Console.WriteLine($"ILogger (Singleton): екземпляри 1 та 2 однакові в пам'яті -> {isSameLogger} (Очікувано: True)");

var repo1 = container.Resolve<IOrderRepository>();
var repo2 = container.Resolve<IOrderRepository>();
bool isSameRepo = ReferenceEquals(repo1, repo2);
Console.WriteLine($"IOrderRepository (Transient): екземпляри 1 та 2 різні в пам'яті -> {!isSameRepo} (Очікувано: True)");

Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("💼 Крок 4: Виконання бізнес-операції через зібраний граф:");
Console.WriteLine("--------------------------------------------------------------------------");

orderService.ProcessOrder("ORD-2026-X1", "Олександр", 4500.50m);

Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("🛡️ Крок 5: Демонстрація виявлення циклічних залежностей (Circular Dependency):");
Console.WriteLine("--------------------------------------------------------------------------");

// Створюємо окремий тестовий контейнер для перевірки циклу: ServiceA -> ServiceB -> ServiceC -> ServiceA
var testCycleContainer = new Container();
testCycleContainer.RegisterTransient<ServiceA>();
testCycleContainer.RegisterTransient<ServiceB>();
testCycleContainer.RegisterTransient<ServiceC>();

try
{
    Console.WriteLine("Спроба розв'язати граф із замкненим колом (ServiceA -> ServiceB -> ServiceC -> ServiceA)...");
    testCycleContainer.Resolve<ServiceA>();
}
catch (CircularDependencyException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n[УСПІШНО ПЕРЕХОПЛЕНО]: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("Контейнер захистив додаток від аварійного падіння через StackOverflowException!");
}

Console.WriteLine("\n==========================================================================");
Console.WriteLine(" 🎉 Демонстрацію IoC-контейнера успішно завершено!");
Console.WriteLine("==========================================================================");
