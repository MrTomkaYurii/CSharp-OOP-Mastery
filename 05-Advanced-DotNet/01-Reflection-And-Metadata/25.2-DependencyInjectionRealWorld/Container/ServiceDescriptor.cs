namespace DependencyInjection.Container;

/// <summary>
/// Паспорт (дескриптор) зареєстрованого сервісу в IoC-контейнері.
/// Зберігає:
/// 1. Який інтерфейс запитують (ServiceType).
/// 2. Який конкретний клас потрібно створити через рефлексію (ImplementationType).
/// 3. Який час життя обрано (Transient чи Singleton).
/// 4. Збережений екземпляр (SingletonInstance), якщо сервіс є Singleton.
/// </summary>
public sealed class ServiceDescriptor
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
