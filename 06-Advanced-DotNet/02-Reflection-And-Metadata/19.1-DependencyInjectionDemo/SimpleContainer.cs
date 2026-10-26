using System.Reflection;

namespace DependencyInjectionDemo;

/// <summary>
/// Життєвий цикл об'єктів у контейнері:
/// - Transient: новий екземпляр створюється щоразу під час виклику Resolve.
/// - Singleton: створюється один єдиний екземпляр при першому зверненні та кешується.
/// </summary>
public enum ServiceLifetime
{
    Transient,
    Singleton
}

/// <summary>
/// Опис реєстрації сервісу в контейнері.
/// </summary>
public class ServiceDescriptor
{
    public Type ServiceType { get; }
    public Type ImplementationType { get; }
    public ServiceLifetime Lifetime { get; }
    public object? SingletonInstance { get; set; }

    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
    }
}

/// <summary>
/// Простий, прозорий навчальний IoC-контейнер для студентів.
/// Працює на базі трьох фундаментальних методів рефлексії:
/// 1. Type.GetConstructors()
/// 2. ConstructorInfo.GetParameters()
/// 3. ConstructorInfo.Invoke(parameters)
/// </summary>
public class SimpleContainer
{
    // Таблиця реєстрацій: Interface/ServiceType -> ServiceDescriptor
    private readonly Dictionary<Type, ServiceDescriptor> _registrations = new();

    /// <summary>
    /// Реєстрація сервісу як Transient (новий екземпляр щоразу).
    /// </summary>
    public void RegisterTransient<TService, TImplementation>() 
        where TImplementation : TService
    {
        _registrations[typeof(TService)] = new ServiceDescriptor(
            typeof(TService), 
            typeof(TImplementation), 
            ServiceLifetime.Transient);
    }

    /// <summary>
    /// Реєстрація конкретного класу без інтерфейсу як Transient.
    /// </summary>
    public void RegisterTransient<TService>()
    {
        RegisterTransient<TService, TService>();
    }

    /// <summary>
    /// Реєстрація сервісу як Singleton (один екземпляр на весь додаток).
    /// </summary>
    public void RegisterSingleton<TService, TImplementation>() 
        where TImplementation : TService
    {
        _registrations[typeof(TService)] = new ServiceDescriptor(
            typeof(TService), 
            typeof(TImplementation), 
            ServiceLifetime.Singleton);
    }

    /// <summary>
    /// Типізований метод отримання сервісу.
    /// </summary>
    public TService Resolve<TService>()
    {
        return (TService)Resolve(typeof(TService));
    }

    /// <summary>
    /// Головний метод створення об'єкта через рефлексію.
    /// </summary>
    public object Resolve(Type serviceType)
    {
        // 1. Шукаємо реєстрацію типу в словнику
        if (!_registrations.TryGetValue(serviceType, out ServiceDescriptor? descriptor))
        {
            throw new InvalidOperationException($"Тип '{serviceType.Name}' не зареєстровано в контейнері!");
        }

        // 2. Якщо сервіс є Singleton і його вже створювали раніше — повертаємо готовий екземпляр
        if (descriptor.Lifetime == ServiceLifetime.Singleton && descriptor.SingletonInstance != null)
        {
            Console.WriteLine($"[DI Container] Singleton для '{serviceType.Name}' вже існує в пам'яті. Повертаємо кешований екземпляр.");
            return descriptor.SingletonInstance;
        }

        Type targetType = descriptor.ImplementationType;
        Console.WriteLine($"[DI Container] Створення екземпляра '{targetType.Name}' для запиту '{serviceType.Name}'...");

        // 3. РЕФЛЕКСІЯ КРОК 1: Отримуємо список публічних конструкторів
        ConstructorInfo[] constructors = targetType.GetConstructors();
        if (constructors.Length == 0)
        {
            throw new InvalidOperationException($"Клас '{targetType.Name}' не має публічних конструкторів!");
        }

        // Для простоти обираємо перший конструктор
        ConstructorInfo constructor = constructors[0];

        // 4. РЕФЛЕКСІЯ КРОК 2: Досліджуємо параметри конструктора
        ParameterInfo[] parameters = constructor.GetParameters();
        object[] parameterInstances = new object[parameters.Length];

        // 5. РЕФЛЕКСІЯ КРОК 3: Рекурсивно розв'язуємо кожну залежність через контейнер
        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo param = parameters[i];
            Console.WriteLine($"  -> Пошук параметра [{i}]: '{param.Name}' типу '{param.ParameterType.Name}'...");
            parameterInstances[i] = Resolve(param.ParameterType);
        }

        // 6. РЕФЛЕКСІЯ КРОК 4: Викликаємо конструктор з передачею розв'язаних залежностей
        object createdInstance = constructor.Invoke(parameterInstances);

        // 7. Якщо тип зареєстровано як Singleton — зберігаємо його в дескрипторі
        if (descriptor.Lifetime == ServiceLifetime.Singleton)
        {
            descriptor.SingletonInstance = createdInstance;
            Console.WriteLine($"[DI Container] Екземпляр '{targetType.Name}' збережено як Singleton.");
        }

        return createdInstance;
    }
}
