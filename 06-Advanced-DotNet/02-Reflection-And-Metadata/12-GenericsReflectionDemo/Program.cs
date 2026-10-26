using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація дженериків у рефлексії (Generics & Reflection) ===\n");

// 1. Відкриті (Open) та закриті (Closed) дженерик-типи
Type openListType = typeof(List<>);
Type closedListType = typeof(List<Car>);

Console.WriteLine($"Відкритий дженерик List<>:");
Console.WriteLine($"  IsGenericType: {openListType.IsGenericType}");
Console.WriteLine($"  IsGenericTypeDefinition: {openListType.IsGenericTypeDefinition}");

Console.WriteLine($"\nЗакритий дженерик List<Car>:");
Console.WriteLine($"  IsGenericType: {closedListType.IsGenericType}");
Console.WriteLine($"  IsGenericTypeDefinition: {closedListType.IsGenericTypeDefinition}");
Console.WriteLine($"  Аргумент типу: {closedListType.GetGenericArguments()[0].Name}");

// 2. Динамічне конструювання закритого типу в runtime (MakeGenericType)
Console.WriteLine("\n--- Динамічне створення Dictionary<string, Car> через MakeGenericType ---");
Type openDictType = typeof(Dictionary<,>);
Type constructedDictType = openDictType.MakeGenericType(typeof(string), typeof(Car));

// Створення екземпляра
object dictInstance = Activator.CreateInstance(constructedDictType)!;
Console.WriteLine($"Створено об'єкт: {dictInstance.GetType().FullName}");

// Додавання елемента через виклик методу Add
MethodInfo addMethod = constructedDictType.GetMethod("Add")!;
addMethod.Invoke(dictInstance, new object[] { "CAR-01", new Car("Red", 90) });

MethodInfo countProp = constructedDictType.GetProperty("Count")!.GetGetMethod()!;
Console.WriteLine($"Кількість елементів у створеному Dictionary: {countProp.Invoke(dictInstance, null)}");

// 3. Динамічний виклик відкритого дженерик-методу (MakeGenericMethod)
Console.WriteLine("\n--- Виклик відкритого дженерик-методу через MakeGenericMethod ---");
var processor = new DataProcessor();
MethodInfo genericMethod = typeof(DataProcessor).GetMethod("ProcessItem")!;

// Перетворюємо відкритий метод ProcessItem<T> у закритий ProcessItem<Car>
MethodInfo closedMethod = genericMethod.MakeGenericMethod(typeof(Car));
closedMethod.Invoke(processor, new object[] { new Car("Gold", 130) });

// 4. Інспекція обмежень дженериків (Generic Constraints)
Console.WriteLine("\n--- Інспекція обмежень (Constraints) параметрів типів ---");
Type repositoryType = typeof(IRepository<>);
Type typeParam = repositoryType.GetGenericArguments()[0];

Console.WriteLine($"Параметр типу: {typeParam.Name}");
Console.WriteLine($"Атрибути параметра: {typeParam.GenericParameterAttributes}");
foreach (Type constraint in typeParam.GetGenericParameterConstraints())
{
    Console.WriteLine($"  Обмеження (Constraint): {constraint.Name}");
}

public class DataProcessor
{
    public void ProcessItem<T>(T item)
    {
        Console.WriteLine($"[ProcessItem<{typeof(T).Name}>] Оброблено елемент типу {typeof(T).Name}: {item}");
    }
}

public interface IRepository<T> where T : class, ICar, new()
{
}
