using System.Reflection;

namespace DependencyInjection.Container;

/// <summary>
/// Навчальний повнофункціональний IoC/DI контейнер на базі рефлексії .NET.
/// Демонструє аналіз метаданих конструкторів, рекурсивне розв'язання параметрів,
/// Property Injection, керування життєвими циклами (Transient/Singleton)
/// та детекцію циклічних залежностей (Circular Dependency Detection).
/// </summary>
public class Container : IContainer
{
    private readonly Dictionary<Type, ServiceDescriptor> _registrations = new();
    private readonly Stack<Type> _resolutionStack = new();
    private int _resolutionDepth;

    #region Реєстрація компонентів

    public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
    {
        Register(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
    }

    public void RegisterTransient<TService>() where TService : class
    {
        Register(typeof(TService), typeof(TService), ServiceLifetime.Transient);
    }

    public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
    {
        Register(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
    }

    public void RegisterSingleton<TService>() where TService : class
    {
        Register(typeof(TService), typeof(TService), ServiceLifetime.Singleton);
    }

    public void RegisterInstance<TService>(TService instance) where TService : class
    {
        ArgumentNullException.ThrowIfNull(instance);
        _registrations[typeof(TService)] = new ServiceDescriptor(typeof(TService), instance);
        Console.WriteLine($"  [Container] Зареєстровано екземпляр: {typeof(TService).Name} (Singleton)");
    }

    public void RegisterFactory<TService>(Func<IContainer, TService> factory, ServiceLifetime lifetime = ServiceLifetime.Transient) where TService : class
    {
        ArgumentNullException.ThrowIfNull(factory);
        _registrations[typeof(TService)] = new ServiceDescriptor(typeof(TService), c => factory(c), lifetime);
        Console.WriteLine($"  [Container] Зареєстровано фабрику: {typeof(TService).Name} [{lifetime}]");
    }

    private void Register(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        if ((serviceType.IsInterface || serviceType.IsAbstract) && !serviceType.IsAssignableFrom(implementationType))
        {
            throw new InvalidOperationException($"Тип {implementationType.Name} не реалізує контракт {serviceType.Name}");
        }

        if (implementationType.IsInterface || implementationType.IsAbstract)
        {
            throw new InvalidOperationException($"Тип реалізації {implementationType.Name} не може бути абстрактним класом або інтерфейсом.");
        }

        _registrations[serviceType] = new ServiceDescriptor(serviceType, implementationType, lifetime);
        Console.WriteLine($"  [Container] Зареєстровано: {serviceType.Name} -> {implementationType.Name} [{lifetime}]");
    }

    #endregion

    #region Розв'язання залежностей

    public TService Resolve<TService>()
    {
        return (TService)Resolve(typeof(TService));
    }

    public object Resolve(Type serviceType)
    {
        // 1. Захист від циклічних залежностей (Circular Dependency Detection)
        // Якщо тип уже присутній у поточному ланцюжку викликів, виникло замкнене коло
        if (_resolutionStack.Contains(serviceType))
        {
            var cycleChain = _resolutionStack.Reverse().Concat(new[] { serviceType }).Select(t => t.Name);
            string formattedChain = string.Join(" -> ", cycleChain);
            throw new CircularDependencyException($"Виявлено циклічну залежність! Ланцюжок: {formattedChain}");
        }

        // 2. Отримання дескриптора сервісу
        if (!_registrations.TryGetValue(serviceType, out ServiceDescriptor? descriptor))
        {
            // Якщо тип є конкретним неабстрактним класом, дозволяємо його створення on-the-fly
            if (serviceType.IsClass && !serviceType.IsAbstract)
            {
                descriptor = new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Transient);
            }
            else
            {
                throw new InvalidOperationException($"Сервіс типу '{serviceType.FullName}' не зареєстровано в контейнері.");
            }
        }

        // 3. Перевірка кешу для Singleton
        if (descriptor.Lifetime == ServiceLifetime.Singleton && descriptor.SingletonInstance != null)
        {
            LogResolutionStep(serviceType, descriptor.SingletonInstance.GetType(), isCached: true);
            return descriptor.SingletonInstance;
        }

        _resolutionStack.Push(serviceType);
        _resolutionDepth++;

        try
        {
            object instance;

            // 4. Створення екземпляра (через фабрику або рефлексію)
            if (descriptor.Factory != null)
            {
                LogResolutionStep(serviceType, null, isFactory: true);
                instance = descriptor.Factory(this);
            }
            else
            {
                Type implType = descriptor.ImplementationType!;
                LogResolutionStep(serviceType, implType, isCached: false);

                // Інстанціювання через рефлексію з рекурсивним пошуком параметрів
                instance = CreateInstanceViaReflection(implType);

                // 5. Впровадження залежностей у властивості (Property Injection)
                InjectProperties(instance, implType);
            }

            // 6. Кешування екземпляра для Singleton
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.SingletonInstance = instance;
            }

            return instance;
        }
        finally
        {
            _resolutionDepth--;
            _resolutionStack.Pop();
        }
    }

    #endregion

    #region Механізми рефлексії

    private object CreateInstanceViaReflection(Type implType)
    {
        // Отримуємо всі публічні конструктори екземпляра
        ConstructorInfo[] constructors = implType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        if (constructors.Length == 0)
        {
            throw new InvalidOperationException($"Клас '{implType.FullName}' не має жодного публічного конструктора для створення.");
        }

        // Стратегія вибору конструктора:
        // А. Якщо присутній конструктор з атрибутом [InjectConstructor] - надаємо йому найвищий пріоритет.
        // Б. Якщо атрибута немає - застосовуємо жадібний вибір: сортуємо за кількістю параметрів у порядку спадання.
        ConstructorInfo selectedCtor = constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectConstructorAttribute>() != null)
                                      ?? constructors.OrderByDescending(c => c.GetParameters().Length).First();

        ParameterInfo[] parameters = selectedCtor.GetParameters();
        object[] constructorArgs = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo param = parameters[i];
            Type paramType = param.ParameterType;

            // Якщо параметр не зареєстрований, але має значення за замовчуванням (DefaultValue)
            if (!_registrations.ContainsKey(paramType) && param.HasDefaultValue)
            {
                constructorArgs[i] = param.DefaultValue!;
            }
            else
            {
                try
                {
                    // Рекурсивно розв'язуємо залежність для аргументу конструктора
                    constructorArgs[i] = Resolve(paramType);
                }
                catch when (param.HasDefaultValue)
                {
                    // Якщо розв'язати не вдалося, але є значення за замовчуванням - підставляємо його
                    constructorArgs[i] = param.DefaultValue!;
                }
            }
        }

        // Викликаємо конструктор з розв'язаними аргументами через рефлексію
        return selectedCtor.Invoke(constructorArgs);
    }

    private void InjectProperties(object instance, Type implType)
    {
        // Шукаємо відкриті властивості екземпляра, помічені атрибутом [InjectProperty]
        PropertyInfo[] properties = implType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in properties)
        {
            if (prop.GetCustomAttribute<InjectPropertyAttribute>() != null && prop.CanWrite)
            {
                object dependency = Resolve(prop.PropertyType);
                prop.SetValue(instance, dependency);
            }
        }
    }

    private void LogResolutionStep(Type serviceType, Type? implType, bool isCached = false, bool isFactory = false)
    {
        int depth = Math.Max(0, _resolutionDepth - 1);
        string indent = new string(' ', depth * 3);
        string branch = _resolutionDepth <= 1 ? "[ROOT] " : "|-- ";

        if (isCached)
        {
            Console.WriteLine($"{indent}{branch}[КЕШ SINGLETON] {serviceType.Name} (перевикористано існуючий об'єкт)");
        }
        else if (isFactory)
        {
            Console.WriteLine($"{indent}{branch}[ФАБРИКА] {serviceType.Name} (створено через користувацький делегат)");
        }
        else
        {
            string implDetails = implType != null && implType != serviceType ? $" -> {implType.Name}" : "";
            Console.WriteLine($"{indent}{branch}[СТВОРЕННЯ] {serviceType.Name}{implDetails}");
        }
    }

    #endregion
}
