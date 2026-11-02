using System.Reflection;

namespace DependencyInjection.Container;

/// <summary>
/// Повнофункціональний навчальний IoC/DI контейнер для реальної модульної архітектури.
/// Розширює базовий контейнер із прикладу 25.1 чотирма промисловими механізмами:
///   1. Керування життєвим циклом (Transient vs Singleton).
///   2. Розумний вибір конструктора ([InjectConstructor] або Greedy Constructor).
///   3. Впровадження залежностей у властивості ([InjectProperty]).
///   4. Виявлення циклічних залежностей (Circular Dependency Detection через стек викликів).
/// </summary>
public class Container : IContainer
{
    // Таблиця зареєстрованих сервісів: Тип контракту -> Паспорт реєстрації (ServiceDescriptor)
    private readonly Dictionary<Type, ServiceDescriptor> _registrations = new();

    // Стек активного ланцюжка розв'язання: дозволяє миттєво помітити цикл A -> B -> C -> A
    private readonly Stack<Type> _resolutionStack = new();

    #region 1. Реєстрація компонентів у контейнері

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

    private void Register(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        // Перевірка через рефлексію: чи дійсно клас реалізує заявлений інтерфейс?
        if (!serviceType.IsAssignableFrom(implementationType))
        {
            throw new InvalidOperationException($"Тип {implementationType.Name} не реалізує контракт {serviceType.Name}");
        }

        // Перевірка через рефлексію: не можна створити екземпляр інтерфейсу або абстрактного класу
        if (implementationType.IsInterface || implementationType.IsAbstract)
        {
            throw new InvalidOperationException($"Тип реалізації {implementationType.Name} не може бути абстрактним.");
        }

        _registrations[serviceType] = new ServiceDescriptor(serviceType, implementationType, lifetime);
        Console.WriteLine($"  [Container] Зареєстровано: {serviceType.Name} -> {implementationType.Name} [{lifetime}]");
    }

    #endregion

    #region 2. Головний алгоритм розв'язання залежностей (Resolve)

    public TService Resolve<TService>()
    {
        return (TService)Resolve(typeof(TService));
    }

    public object Resolve(Type serviceType)
    {
        // КРОК 1: Захист від циклічних залежностей (Circular Dependency Detection)
        // Якщо тип вже лежить у стеку поточного розв'язання — ми пішли по колу!
        if (_resolutionStack.Contains(serviceType))
        {
            var cycleChain = _resolutionStack.Reverse().Concat(new[] { serviceType }).Select(t => t.Name);
            string formattedChain = string.Join(" -> ", cycleChain);
            throw new CircularDependencyException($"Виявлено циклічну залежність! Ланцюжок: {formattedChain}");
        }

        // КРОК 2: Пошук опису реєстрації в словнику
        if (!_registrations.TryGetValue(serviceType, out ServiceDescriptor? descriptor))
        {
            throw new InvalidOperationException($"Сервіс типу '{serviceType.Name}' не зареєстровано в контейнері.");
        }

        // КРОК 3: Якщо це Singleton і об'єкт вже був створений раніше — одразу повертаємо його з кешу
        if (descriptor.Lifetime == ServiceLifetime.Singleton && descriptor.SingletonInstance != null)
        {
            Console.WriteLine($"  [КЕШ SINGLETON] Повернуто існуючий екземпляр {serviceType.Name}");
            return descriptor.SingletonInstance;
        }

        // Поміщаємо поточний тип у стек перед початком конструювання його залежностей
        _resolutionStack.Push(serviceType);

        try
        {
            Type implType = descriptor.ImplementationType;
            Console.WriteLine($"  [СТВОРЕННЯ] {serviceType.Name} -> {implType.Name}");

            // КРОК 4: Створення екземпляра через рефлексію конструктора
            object instance = CreateInstanceViaReflection(implType);

            // КРОК 5: Додаткове впровадження залежностей у властивості з атрибутом [InjectProperty]
            InjectProperties(instance, implType);

            // КРОК 6: Якщо сервіс має життєвий цикл Singleton — запам'ятовуємо створений екземпляр
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.SingletonInstance = instance;
            }

            return instance;
        }
        finally
        {
            // Обов'язково прибираємо тип зі стеку, коли його успішно створено
            _resolutionStack.Pop();
        }
    }

    #endregion

    #region 3. Механізми рефлексії (Конструктори та Властивості)

    private object CreateInstanceViaReflection(Type implType)
    {
        // Отримуємо всі відкриті конструктори класу
        ConstructorInfo[] constructors = implType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        if (constructors.Length == 0)
        {
            throw new InvalidOperationException($"Клас '{implType.Name}' не має жодного публічного конструктора.");
        }

        // Правило вибору конструктора:
        // 1. Якщо є конструктор з атрибутом [InjectConstructor] — обираємо його.
        // 2. Інакше обираємо конструктор із найбільшою кількістю параметрів (Greedy Constructor).
        ConstructorInfo selectedCtor = constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectConstructorAttribute>() != null)
                                      ?? constructors.OrderByDescending(c => c.GetParameters().Length).First();

        // Рекурсивно розв'язуємо всі параметри обраного конструктора
        ParameterInfo[] parameters = selectedCtor.GetParameters();
        object[] constructorArgs = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            constructorArgs[i] = Resolve(parameters[i].ParameterType);
        }

        // Створюємо екземпляр класу викликом конструктора
        return selectedCtor.Invoke(constructorArgs);
    }

    private void InjectProperties(object instance, Type implType)
    {
        // Скануємо всі відкриті властивості класу: якщо на властивості стоїть атрибут [InjectProperty],
        // автоматично знаходимо потрібний сервіс і записуємо його через prop.SetValue(...)
        foreach (PropertyInfo prop in implType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetCustomAttribute<InjectPropertyAttribute>() != null && prop.CanWrite)
            {
                object dependency = Resolve(prop.PropertyType);
                prop.SetValue(instance, dependency);
            }
        }
    }

    #endregion
}
