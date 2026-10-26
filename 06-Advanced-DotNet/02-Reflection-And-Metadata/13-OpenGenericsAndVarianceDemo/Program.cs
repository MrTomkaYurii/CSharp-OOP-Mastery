using System.Reflection;
using Shared.Common;

namespace OpenGenericsAndVarianceDemo;

#region Відкриті узагальнені інтерфейси та варіантність

public interface IRepository<T> where T : class
{
    T? GetById(int id);
    void Save(T entity);
}

public class SqlRepository<T> : IRepository<T> where T : class
{
    public T? GetById(int id) => null;
    public void Save(T entity) => Console.WriteLine($"[SqlRepository<{typeof(T).Name}>]: Збережено {entity}");
}

// Коваріантний інтерфейс (out T)
public interface IProducer<out T>
{
    T Produce();
}

// Контраваріантний інтерфейс (in T)
public interface IConsumer<in T>
{
    void Consume(T item);
}

// Інваріантний інтерфейс (без модифікаторів)
public interface IStorage<T>
{
    T Value { get; set; }
}

public class CarProducer : IProducer<Car>
{
    public Car Produce() => new Car("Red", 120);
}

#endregion

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 13. Рефлексія відкритих дженериків та варіантність (Open Generics & Variance) ===\n");

        // 1. Інспекція та зіставлення відкритих типів (Open Generic Type Matching)
        Console.WriteLine("--- 1. Резолюція відкритих типів (як у DI: IRepository<> -> SqlRepository<>) ---");
        Type openInterface = typeof(IRepository<>);
        Type openImplementation = typeof(SqlRepository<>);

        Console.WriteLine($"Відкритий інтерфейс:      {openInterface.Name} (IsGenericTypeDefinition = {openInterface.IsGenericTypeDefinition})");
        Console.WriteLine($"Відкрита імплементація:   {openImplementation.Name} (IsGenericTypeDefinition = {openImplementation.IsGenericTypeDefinition})");

        // Перевіряємо, чи клас реалізує відкритий інтерфейс
        bool implementsOpenGeneric = openImplementation.GetInterfaces().Any(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == openInterface);

        Console.WriteLine($"Чи реалізує SqlRepository<> інтерфейс IRepository<>: {implementsOpenGeneric}");

        // Динамічне закриття типів для доменної сутності Car:
        Type closedInterface = openInterface.MakeGenericType(typeof(Car));
        Type closedImplementation = openImplementation.MakeGenericType(typeof(Car));

        Console.WriteLine($"Закритий інтерфейс:      {closedInterface.FullName}");
        Console.WriteLine($"Закрита імплементація:   {closedImplementation.FullName}");

        // Створення екземпляра закритого типу та виклик методу
        var repoInstance = (IRepository<Car>)Activator.CreateInstance(closedImplementation)!;
        repoInstance.Save(new Car("Blue", 100));
        Console.WriteLine();

        // 2. Інспекція коваріантності та контраваріантності в метаданих
        Console.WriteLine("--- 2. Інспекція параметрів варіантності у метаданих CLR ---");
        InspectVariance(typeof(IProducer<>));
        InspectVariance(typeof(IConsumer<>));
        InspectVariance(typeof(IStorage<>));

        // 3. Перевірка сумісності типів з урахуванням коваріантності через IsAssignableFrom
        Console.WriteLine("--- 3. Перевірка поліморфної сумісності (Variance Assignment) ---");
        Type producerCar = typeof(IProducer<Car>);
        Type producerObject = typeof(IProducer<object>);

        // Оскільки IProducer<out T> є коваріантним, IProducer<Car> можна безпечно присвоїти IProducer<object>
        bool isCovariantAssignable = producerObject.IsAssignableFrom(producerCar);
        Console.WriteLine($"Чи можна IProducer<Car> присвоїти IProducer<object>? {isCovariantAssignable} (Коваріантність out)");

        Type storageCar = typeof(IStorage<Car>);
        Type storageObject = typeof(IStorage<object>);
        bool isInvariantAssignable = storageObject.IsAssignableFrom(storageCar);
        Console.WriteLine($"Чи можна IStorage<Car> присвоїти IStorage<object>?  {isInvariantAssignable} (Інваріантність без out)");
    }

    private static void InspectVariance(Type genericInterface)
    {
        Type typeParam = genericInterface.GetGenericArguments()[0];
        GenericParameterAttributes attributes = typeParam.GenericParameterAttributes;

        string variance = "Інваріантний (Invariant)";
        if ((attributes & GenericParameterAttributes.Covariant) != 0)
        {
            variance = "Коваріантний (Covariant: out T)";
        }
        else if ((attributes & GenericParameterAttributes.Contravariant) != 0)
        {
            variance = "Контраваріантний (Contravariant: in T)";
        }

        Console.WriteLine($"Інтерфейс {genericInterface.Name}:");
        Console.WriteLine($"  Параметр типу: {typeParam.Name}");
        Console.WriteLine($"  Атрибути:      {attributes}");
        Console.WriteLine($"  Варіантність:  {variance}");
        Console.WriteLine();
    }
}
