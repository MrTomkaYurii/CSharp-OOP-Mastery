namespace DependencyInjection.Container;

/// <summary>
/// Інтерфейс IoC/DI контейнера, що абстрагує механізми реєстрації та розв'язання залежностей.
/// Дотримується Dependency Inversion Principle (DIP): споживачі взаємодіють з інтерфейсом, а не з конкретною реалізацією.
/// </summary>
public interface IContainer
{
    /// <summary>
    /// Реєструє зіставлення між інтерфейсом TService та реалізацією TImplementation із життєвим циклом Transient
    /// (новий екземпляр створюється при кожному зверненні).
    /// </summary>
    void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService;

    /// <summary>
    /// Реєструє конкретний клас TService із життєвим циклом Transient.
    /// </summary>
    void RegisterTransient<TService>() where TService : class;

    /// <summary>
    /// Реєструє зіставлення між інтерфейсом TService та реалізацією TImplementation із життєвим циклом Singleton
    /// (екземпляр створюється один раз і перевикористовується протягом усього часу роботи програми).
    /// </summary>
    void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService;

    /// <summary>
    /// Реєструє конкретний клас TService із життєвим циклом Singleton.
    /// </summary>
    void RegisterSingleton<TService>() where TService : class;

    /// <summary>
    /// Рекурсивно розв'язує граф залежностей та повертає готовий екземпляр типу TService.
    /// </summary>
    TService Resolve<TService>();

    /// <summary>
    /// Рекурсивно розв'язує граф залежностей та повертає екземпляр за об'єктом метаданих Type.
    /// </summary>
    object Resolve(Type serviceType);
}
