namespace DependencyInjection.Container;

/// <summary>
/// Опис реєстрації сервісу в IoC-контейнері.
/// Зберігає тип контракту, тип конкретної реалізації, стратегію життєвого циклу
/// або готову користувацьку фабрику створення.
/// </summary>
public sealed class ServiceDescriptor
{
    /// <summary>
    /// Базовий тип контракту (інтерфейс або базовий клас).
    /// </summary>
    public Type ServiceType { get; }

    /// <summary>
    /// Конкретний тип реалізації, екземпляр якого буде створено через рефлексію.
    /// </summary>
    public Type? ImplementationType { get; }

    /// <summary>
    /// Життєвий цикл сервісу (Transient або Singleton).
    /// </summary>
    public ServiceLifetime Lifetime { get; }

    /// <summary>
    /// Користувацька фабрика для створення екземпляра (якщо передана).
    /// </summary>
    public Func<IContainer, object>? Factory { get; }

    /// <summary>
    /// Кешований екземпляр для сервісів із життєвим циклом Singleton.
    /// </summary>
    public object? SingletonInstance { get; set; }

    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
    }

    public ServiceDescriptor(Type serviceType, Func<IContainer, object> factory, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        Factory = factory;
        Lifetime = lifetime;
    }

    public ServiceDescriptor(Type serviceType, object instance)
    {
        ServiceType = serviceType;
        SingletonInstance = instance;
        Lifetime = ServiceLifetime.Singleton;
    }
}
