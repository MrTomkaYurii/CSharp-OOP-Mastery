using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 12. РЕФЛЕКСІЯ УЗАГАЛЬНЕНИХ ТИПІВ ТА МЕТОДІВ (Generics Reflection)");
Console.WriteLine(" Як у runtime сконструювати List<T> або Dictionary<TKey, TValue>, коли T невідомий наперед");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Відкритий шаблон (Open Generic) проти закритого типу (Closed Generic)
// -----------------------------------------------------------------------
// - List<> — це лише креслення (IsGenericTypeDefinition == true). Його НЕ МОЖНА створити через new.
// - List<Car> — це вже конкретний тип (IsGenericTypeDefinition == false), готовий до створення.
Console.WriteLine("--- Крок 1: Порівняння відкритого шаблону List<> та закритого типу List<Car> ---");
Type openListType = typeof(List<>);
Type closedListType = typeof(List<Car>);

Console.WriteLine($"  Відкритий шаблон List<>:  IsGenericType = {openListType.IsGenericType}, IsGenericTypeDefinition = {openListType.IsGenericTypeDefinition}");
Console.WriteLine($"  Закритий тип List<Car>:   IsGenericType = {closedListType.IsGenericType}, IsGenericTypeDefinition = {closedListType.IsGenericTypeDefinition}");
Console.WriteLine($"  Тип-аргумент всередині List<Car>: {closedListType.GetGenericArguments()[0].Name}\n");

// -----------------------------------------------------------------------
// Крок 2: Динамічне конструювання типу Dictionary<string, Car> через MakeGenericType
// -----------------------------------------------------------------------
// Уявіть, що тип ключа і значення ми дізналися лише під час роботи програми (наприклад, при читанні JSON).
// Метод MakeGenericType бере шаблон Dictionary<,> і підставляє в нього типи string та Car.
Console.WriteLine("--- Крок 2: Склеювання Dictionary<string, Car> у runtime через MakeGenericType ---");
Type openDictType = typeof(Dictionary<,>);
Type constructedDictType = openDictType.MakeGenericType(typeof(string), typeof(Car));

object dictInstance = Activator.CreateInstance(constructedDictType)!;
Console.WriteLine($"  Сконструйовано та створено об'єкт: {dictInstance.GetType().Name}<string, Car>");

MethodInfo addMethod = constructedDictType.GetMethod("Add")!;
addMethod.Invoke(dictInstance, new object[] { "CAR-01", new Car("Red", 90) });

PropertyInfo countProp = constructedDictType.GetProperty("Count")!;
Console.WriteLine($"  Кількість елементів після виклику Add: {countProp.GetValue(dictInstance)}\n");

// -----------------------------------------------------------------------
// Крок 3: Динамічний виклик узагальненого методу ProcessItem<T> через MakeGenericMethod
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 3: Підстановка типу в узагальнений метод через MakeGenericMethod ---");
var processor = new DataProcessor();
MethodInfo openGenericMethod = typeof(DataProcessor).GetMethod(nameof(DataProcessor.ProcessItem))!;

// Перетворюємо шаблонний метод ProcessItem<T> на конкретний ProcessItem<Car>
MethodInfo closedGenericMethod = openGenericMethod.MakeGenericMethod(typeof(Car));
closedGenericMethod.Invoke(processor, new object[] { new Car("Gold", 130) });

// -----------------------------------------------------------------------
// Крок 4: Перевірка обмежень дженерика (where T : class, ICar, new())
// -----------------------------------------------------------------------
Console.WriteLine("\n--- Крок 4: Інспекція обмежень (Generic Constraints) у IRepository<T> ---");
Type repositoryType = typeof(IRepository<>);
Type typeParam = repositoryType.GetGenericArguments()[0];

Console.WriteLine($"  Параметр типу:           {typeParam.Name}");
Console.WriteLine($"  Спеціальні обмеження:    {typeParam.GenericParameterAttributes}");
foreach (Type constraint in typeParam.GetGenericParameterConstraints())
{
    Console.WriteLine($"  Обов'язковий інтерфейс:  {constraint.Name}");
}

Console.WriteLine("\nВисновок: MakeGenericType та MakeGenericMethod дозволяють будувати універсальні бібліотеки.");

public class DataProcessor
{
    public void ProcessItem<T>(T item)
    {
        Console.WriteLine($"  [ProcessItem<{typeof(T).Name}>]: Успішно оброблено об'єкт типу {typeof(T).Name}");
    }
}

public interface IRepository<T> where T : class, ICar, new()
{
}
