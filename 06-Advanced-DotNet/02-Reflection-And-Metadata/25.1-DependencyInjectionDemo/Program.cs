using DependencyInjectionDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 25.1. НАВЧАЛЬНИЙ ВСТУП ДО DEPENDENCY INJECTION (DI) ЧЕРЕЗ РЕФЛЕКСІЮ");
Console.WriteLine(" Як 4 рядки рефлексії автоматично будують ланцюжок об'єктів замість 'new'");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Проблема ручного створення об'єктів (Manual Wiring)
// -----------------------------------------------------------------------
// Уявіть типовий ланцюжок залежностей:
//   OrderService потребує -> IUserRepository та ILogger
//   UserRepository потребує -> ILogger
//   ConsoleLogger не потребує нічого (конструктор без параметрів)
Console.WriteLine("--- Крок 1: Як виглядає ручне створення об'єктів без контейнера ---");
Console.WriteLine("  ILogger logger = new ConsoleLogger();");
Console.WriteLine("  IUserRepository repo = new UserRepository(logger);");
Console.WriteLine("  OrderService service = new OrderService(repo, logger);\n");

ILogger manualLogger = new ConsoleLogger();
IUserRepository manualRepo = new UserRepository(manualLogger);
OrderService manualService = new OrderService(manualRepo, manualLogger);
manualService.PlaceOrder("Ноутбук Lenovo", "Олена Коваль");

Console.WriteLine("\nПроблема: якщо завтра у UserRepository з'явиться ще одна залежність,");
Console.WriteLine("нам доведеться вручну переписувати всі місця в програмі, де ми писали 'new UserRepository(...)'.\n");

// -----------------------------------------------------------------------
// Крок 2: Реєстрація типів у нашому ультра-простому SimpleContainer
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 2: Реєстрація відповідностей у SimpleContainer ---");
Console.WriteLine("--------------------------------------------------------------------------");

var container = new SimpleContainer();

// Ми просто заповнюємо словник: "Інтерфейс -> Клас"
container.Register<ILogger, ConsoleLogger>();
container.Register<IUserRepository, UserRepository>();
container.Register<OrderService>();

Console.WriteLine("  Зареєстровано: ILogger         -> ConsoleLogger");
Console.WriteLine("  Зареєстровано: IUserRepository -> UserRepository");
Console.WriteLine("  Зареєстровано: OrderService    -> OrderService\n");

// -----------------------------------------------------------------------
// Крок 3: Магія рефлексії — автоматичне збирання графа об'єктів
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 3: Виклик container.Resolve<OrderService>() ---");
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("Дивіться, як контейнер сам проходить по дереву конструкторів:\n");

OrderService autoOrderService = container.Resolve<OrderService>();

Console.WriteLine("\nОб'єкт OrderService повністю зібрано! Викликаємо метод PlaceOrder:");
autoOrderService.PlaceOrder("Смартфон Samsung", "Тарас Шевченко");

// -----------------------------------------------------------------------
// Підсумок для студентів
// -----------------------------------------------------------------------
Console.WriteLine("\n==========================================================================");
Console.WriteLine(" ГОЛОВНА ІДЕЯ (УВЕСЬ АЛГОРИТМ КОНТЕЙНЕРА У 4 КРОКИ):");
Console.WriteLine("==========================================================================");
Console.WriteLine(" 1. targetType.GetConstructors()[0] -> Знаходимо конструктор класу");
Console.WriteLine(" 2. constructor.GetParameters()     -> Дізнаємося, які типи йому потрібні");
Console.WriteLine(" 3. Resolve(param.ParameterType)    -> Рекурсивно створюємо кожен параметр");
Console.WriteLine(" 4. constructor.Invoke(arguments)   -> Запускаємо конструктор і повертаємо об'єкт!");
Console.WriteLine(" А в наступному прикладі (25.2) ми додамо сюди Singleton-кеш та захист від циклів.");
Console.WriteLine("==========================================================================");

// =======================================================================
// Прості доменні класи для наочності (3 рівні вкладеності)
// =======================================================================

public interface ILogger
{
    void Log(string message);
}

// Рівень 1: Не має жодних залежностей у конструкторі (кінцевий лист дерева рекурсії)
public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"    [LOG]: {message}");
}

public interface IUserRepository
{
    bool Exists(string userName);
}

// Рівень 2: Потребує ILogger у своєму конструкторі
public class UserRepository : IUserRepository
{
    private readonly ILogger _logger;

    public UserRepository(ILogger logger)
    {
        _logger = logger;
    }

    public bool Exists(string userName)
    {
        _logger.Log($"UserRepository: перевірка наявності клієнта '{userName}' у БД...");
        return true;
    }
}

// Рівень 3: Потребує І репозиторій (IUserRepository), І логер (ILogger)
public class OrderService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public OrderService(IUserRepository userRepository, ILogger logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public void PlaceOrder(string product, string customer)
    {
        if (_userRepository.Exists(customer))
        {
            _logger.Log($"OrderService: замовлення на '{product}' для '{customer}' успішно оформлено!");
        }
    }
}
