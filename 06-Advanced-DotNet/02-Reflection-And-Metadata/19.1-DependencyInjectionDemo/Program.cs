using DependencyInjectionDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 19.1. НАВЧАЛЬНИЙ ВСТУП ДО DEPENDENCY INJECTION (DI) ЧЕРЕЗ РЕФЛЕКСІЮ");
Console.WriteLine(" Як рефлексія автоматизує створення об'єктів та ін'єкцію залежностей");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Проблема ручного зв'язування залежностей (Manual Wiring)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Життя без IoC-контейнера (ручне створення графу залежностей) ---\n");

Console.WriteLine("Щоб створити OrderService вручну, розробник змушений писати такий ланцюг:");
Console.WriteLine("  ILogger logger = new ConsoleLogger();");
Console.WriteLine("  IDatabase db = new InMemoryDatabase(logger);");
Console.WriteLine("  IUserService userService = new UserService(db, logger);");
Console.WriteLine("  OrderService orderService = new OrderService(userService, logger);\n");

// Ручне створення
ILogger manualLogger = new ConsoleLogger();
IDatabase manualDb = new InMemoryDatabase(manualLogger);
IUserService manualUserService = new UserService(manualDb, manualLogger);
OrderService manualOrder = new OrderService(manualUserService, manualLogger);
manualOrder.CreateOrder("Ноутбук", "Олена Коваль");

Console.WriteLine("\nНедоліки ручного підходу:");
Console.WriteLine("1. Складність супроводу: зміна конструктора будь-якого класу ламає код у багатьох місцях.");
Console.WriteLine("2. Жорстке зв'язування: складно контролювати життєвий цикл (Singleton / Transient).\n");

// -----------------------------------------------------------------------
// Крок 2: Налаштування та реєстрація в SimpleContainer
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 2: Реєстрація сервісів у SimpleContainer ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

var container = new SimpleContainer();

// Реєструємо залежності:
// ILogger як Singleton (один журнал для всієї програми)
container.RegisterSingleton<ILogger, ConsoleLogger>();

// IDatabase як Singleton (єдине підключення до бази даних)
container.RegisterSingleton<IDatabase, InMemoryDatabase>();

// IUserService як Transient (новий сервіс для кожної операції)
container.RegisterTransient<IUserService, UserService>();

// OrderService як Transient
container.RegisterTransient<OrderService>();

Console.WriteLine("Сервіси успішно зареєстровані в контейнері:");
Console.WriteLine("  [Singleton] ILogger     -> ConsoleLogger");
Console.WriteLine("  [Singleton] IDatabase   -> InMemoryDatabase");
Console.WriteLine("  [Transient] IUserService-> UserService");
Console.WriteLine("  [Transient] OrderService-> OrderService\n");

// -----------------------------------------------------------------------
// Крок 3: Автоматичне розв'язання графу залежностей через рефлексію
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 3: Виклик container.Resolve<OrderService>() ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

Console.WriteLine("Контейнер починає рекурсивний аналіз конструкторів через рефлексію:\n");
OrderService resolvedOrderService1 = container.Resolve<OrderService>();

Console.WriteLine("\nГраф об'єктів успішно побудовано! Викликаємо бізнес-метод:");
resolvedOrderService1.CreateOrder("Смартфон", "Тарас Шевченко");

// -----------------------------------------------------------------------
// Крок 4: Перевірка життєвих циклів (Transient vs Singleton)
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 4: Перевірка Singleton vs Transient (ReferenceEquals) ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

Console.WriteLine("Запитуємо OrderService вдруге...");
OrderService resolvedOrderService2 = container.Resolve<OrderService>();

bool isSameOrderService = ReferenceEquals(resolvedOrderService1, resolvedOrderService2);
Console.WriteLine($"\n1. Перевірка OrderService (Transient):");
Console.WriteLine($"   Чи однакове посилання (ReferenceEquals)? {isSameOrderService}");
Console.WriteLine($"   Пояснення: OrderService зареєстровано як Transient, тому створено два РІЗНІ екземпляри.\n");

bool isSameDb = ReferenceEquals(resolvedOrderService1.DatabaseInstance, resolvedOrderService2.DatabaseInstance);
Console.WriteLine($"2. Перевірка IDatabase всередині обох OrderService (Singleton):");
Console.WriteLine($"   Чи однакове посилання (ReferenceEquals)? {isSameDb}");
Console.WriteLine($"   Пояснення: IDatabase зареєстровано як Singleton, тому обидва сервіси отримали ОДИН І ТОЙ САМИЙ об'єкт!");

// -----------------------------------------------------------------------
// Крок 5: Підсумок для студентів
// -----------------------------------------------------------------------
Console.WriteLine("\n==========================================================================");
Console.WriteLine(" ПІДСУМОК: ЯК ПРАЦЮЄ IoC/DI КОНТЕЙНЕР НА ОСНОВІ РЕФЛЕКСІЇ");
Console.WriteLine("==========================================================================");
Console.WriteLine("1. type.GetConstructors()      -> Знаходить доступні конструктори класу.");
Console.WriteLine("2. constructor.GetParameters() -> Читає типи аргументів, які потрібні класу.");
Console.WriteLine("3. container.Resolve(paramType)-> Рекурсивно знаходить або створює кожен аргумент.");
Console.WriteLine("4. constructor.Invoke(args)    -> Створює готовий екземпляр з усіма залежностями.");
Console.WriteLine("Це звільняє розробника від ручного написання 'new' та забезпечує принцип DIP (SOLID)!");
Console.WriteLine("==========================================================================");

// -----------------------------------------------------------------------
// Доменні типи для демонстрації
// -----------------------------------------------------------------------

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"   [LOG] {message}");
}

public interface IDatabase
{
    void Save(string entity);
}

public class InMemoryDatabase : IDatabase
{
    private readonly ILogger _logger;

    public InMemoryDatabase(ILogger logger)
    {
        _logger = logger;
        _logger.Log("InMemoryDatabase: підключення до бази даних ініціалізовано.");
    }

    public void Save(string entity) => _logger.Log($"InMemoryDatabase: запис '{entity}' збережено в таблицю.");
}

public interface IUserService
{
    IDatabase Database { get; }
    void RegisterUser(string userName);
}

public class UserService : IUserService
{
    public IDatabase Database { get; }
    private readonly ILogger _logger;

    public UserService(IDatabase database, ILogger logger)
    {
        Database = database;
        _logger = logger;
        _logger.Log("UserService: створено новий екземпляр сервісу користувачів.");
    }

    public void RegisterUser(string userName)
    {
        _logger.Log($"UserService: реєстрація користувача '{userName}'...");
        Database.Save($"User:{userName}");
    }
}

public class OrderService
{
    private readonly IUserService _userService;
    private readonly ILogger _logger;

    public IDatabase DatabaseInstance => _userService.Database;

    public OrderService(IUserService userService, ILogger logger)
    {
        _userService = userService;
        _logger = logger;
        _logger.Log("OrderService: створено новий екземпляр сервісу замовлень.");
    }

    public void CreateOrder(string item, string customer)
    {
        _logger.Log($"OrderService: оформлення замовлення '{item}' для клієнта '{customer}'...");
        _userService.RegisterUser(customer);
        DatabaseInstance.Save($"Order:{item} (Customer:{customer})");
        _logger.Log($"OrderService: замовлення '{item}' успішно створено!");
    }
}
