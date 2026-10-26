namespace DependencyInjection.Container;

/// <summary>
/// Інтерфейс IoC/DI контейнера, що абстрагує механізми реєстрації та розв'язання залежностей.
/// Дотримується Dependency Inversion Principle (DIP): споживачі взаємодіють з інтерфейсом, а не з конкретною реалізацією.
/// </summary>
public interface IContainer
{
    /// <summary>
    /// Реєструє зіставлення між інтерфейсом TService та реалізацією TImplementation із життєвим циклом Transient.
    /// </summary>
    void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService;

    /// <summary>
    /// Реєструє конкретний клас TService із життєвим циклом Transient.
    /// </summary>
    void RegisterTransient<TService>() where TService : class;

    /// <summary>
    /// Реєструє зіставлення між інтерфейсом TService та реалізацією TImplementation із життєвим циклом Singleton.
    /// </summary>
    void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService;

    /// <summary>
    /// Реєструє конкретний клас TService із життєвим циклом Singleton.
    /// </summary>
    void RegisterSingleton<TService>() where TService : class;

    /// <summary>
    /// Реєструє вже створений екземпляр як Singleton.
    /// </summary>
    void RegisterInstance<TService>(TService instance) where TService : class;

    /// <summary>
    /// Реєструє фабричний делегат для ручного конструювання об'єкта.
    /// </summary>
    void RegisterFactory<TService>(Func<IContainer, TService> factory, ServiceLifetime lifetime = ServiceLifetime.Transient) where TService : class;

    /// <summary>
    /// Рекурсивно розв'язує граф залежностей та повертає екземпляр типу TService.
    /// </summary>
    TService Resolve<TService>();

    /// <summary>
    /// Рекурсивно розв'язує граф залежностей та повертає екземпляр за типом Type.
    /// </summary>
    object Resolve(Type serviceType);
}
