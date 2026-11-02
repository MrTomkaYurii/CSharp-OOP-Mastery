namespace DependencyInjection.Services.Diagnostics;

/// <summary>
/// Тестовий сервіс для перевірки виявлення циклічних залежностей: ServiceB -> ServiceC.
/// </summary>
public class ServiceB
{
    public ServiceC DependencyC { get; }

    public ServiceB(ServiceC dependencyC)
    {
        DependencyC = dependencyC;
    }
}
