namespace DependencyInjection.Services.Diagnostics;

/// <summary>
/// Тестовий сервіс для перевірки виявлення циклічних залежностей: ServiceA -> ServiceB.
/// </summary>
public class ServiceA
{
    public ServiceB DependencyB { get; }

    public ServiceA(ServiceB dependencyB)
    {
        DependencyB = dependencyB;
    }
}
