namespace DependencyInjection.Services.Diagnostics;

/// <summary>
/// Тестовий сервіс для перевірки виявлення циклічних залежностей: ServiceC -> ServiceA (утворює цикл).
/// </summary>
public class ServiceC
{
    public ServiceA DependencyA { get; }

    public ServiceC(ServiceA dependencyA)
    {
        DependencyA = dependencyA;
    }
}
