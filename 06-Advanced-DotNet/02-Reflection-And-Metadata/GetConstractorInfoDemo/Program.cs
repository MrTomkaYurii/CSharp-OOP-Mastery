using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація ConstructorInfo (Аналіз та виклик конструкторів) ===\n");

Type carType = typeof(Car);

// 1. Отримання тільки публічних конструкторів
Console.WriteLine("--- Публічні конструктори Car ---");
ConstructorInfo[] publicCtors = carType.GetConstructors();
foreach (ConstructorInfo ctor in publicCtors)
{
    PrintConstructor(ctor);
}

// 2. Отримання ВСІХ конструкторів (включаючи приватні)
Console.WriteLine("\n--- Усі конструктори (включно з приватними) ---");
ConstructorInfo[] allCtors = carType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
foreach (ConstructorInfo ctor in allCtors)
{
    PrintConstructor(ctor);
}

// 3. Динамічне створення екземпляра через публічний конструктор
Console.WriteLine("\n--- Створення об'єкта через ConstructorInfo.Invoke ---");
ConstructorInfo paramCtor = carType.GetConstructor(new[] { typeof(string), typeof(int) })!;
object carInstance = paramCtor.Invoke(new object[] { "Blue", 120 });
Console.WriteLine($"Створено об'єкт: {carInstance}, Color: {((Car)carInstance).Color}, Speed: {((Car)carInstance).Speed}");

// 4. Створення екземпляра через ПРИВАТНИЙ конструктор (обхід інкапсуляції)
Console.WriteLine("\n--- Виклик ПРИВАТНОГО конструктора ---");
ConstructorInfo? privateCtor = carType.GetConstructor(
    BindingFlags.NonPublic | BindingFlags.Instance,
    binder: null,
    types: new[] { typeof(string), typeof(double), typeof(int) },
    modifiers: null);

if (privateCtor != null)
{
    object privateCar = privateCtor.Invoke(new object[] { "SPEC-777", 260.0, 500 });
    Console.WriteLine($"Успішно створено через приватний ctor! LicensePlate: {((Car)privateCar).LicensePlate}");
}

static void PrintConstructor(ConstructorInfo ctor)
{
    string visibility = ctor.IsPublic ? "public" : (ctor.IsPrivate ? "private" : "protected/internal");
    var parameters = ctor.GetParameters();
    string paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
    Console.WriteLine($"  [{visibility}] {ctor.DeclaringType?.Name}({paramList})");
}
