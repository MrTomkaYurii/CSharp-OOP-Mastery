using System.Reflection;

namespace DependencyInjectionDemo;

/// <summary>
/// Ультра-простий навчальний IoC/DI контейнер (~60 рядків коду).
/// Створений спеціально для першого знайомства студентів із тим,
/// як рефлексія замінює ручний виклик оператора 'new'.
///
/// Увесь секрет роботи контейнера складається з 4 кроків у методі Resolve:
///   Крок 1: Знайти конструктор класу       -> targetType.GetConstructors()[0]
///   Крок 2: Дізнатися список параметрів    -> constructor.GetParameters()
///   Крок 3: Рекурсивно створити аргументи  -> Resolve(param.ParameterType)
///   Крок 4: Викликати конструктор          -> constructor.Invoke(arguments)
/// </summary>
public class SimpleContainer
{
    // Єдина структура даних контейнера — словник відповідностей:
    // Ключ:   Абстракція / Інтерфейс (наприклад, typeof(ILogger))
    // Значення: Конкретний клас реалізації (наприклад, typeof(ConsoleLogger))
    private readonly Dictionary<Type, Type> _registrations = new();

    /// <summary>
    /// Реєструє зв'язок: "Коли хтось попросить інтерфейс TService — створи клас TImplementation".
    /// </summary>
    public void Register<TService, TImplementation>()
        where TImplementation : TService
    {
        _registrations[typeof(TService)] = typeof(TImplementation);
    }

    /// <summary>
    /// Реєструє сам конкретний клас (наприклад, OrderService -> OrderService).
    /// </summary>
    public void Register<TConcrete>()
    {
        _registrations[typeof(TConcrete)] = typeof(TConcrete);
    }

    /// <summary>
    /// Зручна generic-обгортка для отримання готового об'єкта.
    /// </summary>
    public TService Resolve<TService>()
    {
        return (TService)Resolve(typeof(TService));
    }

    /// <summary>
    /// Головний рекурсивний двигун контейнера: збирає об'єкт та всі його залежності через рефлексію.
    /// </summary>
    public object Resolve(Type serviceType)
    {
        // 0. Перевіряємо, чи знає контейнер, який клас потрібно створити для цього типу
        if (!_registrations.TryGetValue(serviceType, out Type? implementationType))
        {
            throw new InvalidOperationException($"Тип '{serviceType.Name}' не зареєстровано в контейнері!");
        }

        Console.WriteLine($"  [SimpleContainer] Збираємо '{implementationType.Name}' (для запиту '{serviceType.Name}')...");

        // КРОК 1 (Рефлексія): Беремо перший публічний конструктор класу
        ConstructorInfo constructor = implementationType.GetConstructors()[0];

        // КРОК 2 (Рефлексія): Читаємо список параметрів, яких потребує цей конструктор
        ParameterInfo[] parameters = constructor.GetParameters();
        object[] arguments = new object[parameters.Length];

        // КРОК 3 (Рекурсія): Для кожного параметра конструктора знову викликаємо Resolve(...)
        // Якщо конструктор не має параметрів (як у ConsoleLogger), цикл просто не виконається і рекурсія завершиться!
        for (int i = 0; i < parameters.Length; i++)
        {
            Type dependencyType = parameters[i].ParameterType;
            Console.WriteLine($"    -> Конструктору '{implementationType.Name}' потрібен параметр '{parameters[i].Name}' типу '{dependencyType.Name}'");
            arguments[i] = Resolve(dependencyType);
        }

        // КРОК 4 (Рефлексія): Викликаємо конструктор і передаємо в нього зібрані аргументи
        object instance = constructor.Invoke(arguments);
        return instance;
    }
}
