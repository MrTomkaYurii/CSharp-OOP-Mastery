using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 10. ДОСЛІДЖЕННЯ КОНСТРУКТОРІВ ТА СТВОРЕННЯ ОБ'ЄКТІВ: ConstructorInfo");
Console.WriteLine(" Як знайти потрібний конструктор класу і створити об'єкт через Invoke");
Console.WriteLine("==========================================================================\n");

Type carType = typeof(Car);

// -----------------------------------------------------------------------
// Крок 1: Отримання всіх відкритих (public) конструкторів класу
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Відкриті конструктори класу Car (GetConstructors) ---");
ConstructorInfo[] publicCtors = carType.GetConstructors();
foreach (ConstructorInfo ctor in publicCtors)
{
    PrintConstructorSignature(ctor);
}

// -----------------------------------------------------------------------
// Крок 2: Отримання ВСІХ конструкторів, включно із закритими (private)
// -----------------------------------------------------------------------
Console.WriteLine("\n--- Крок 2: Усі конструктори класу Car (включно з private) ---");
ConstructorInfo[] allCtors = carType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
foreach (ConstructorInfo ctor in allCtors)
{
    PrintConstructorSignature(ctor);
}

// -----------------------------------------------------------------------
// Крок 3: Пошук конкретного конструктора за типами параметрів та створення об'єкта
// -----------------------------------------------------------------------
// Шукаємо конструктор Car(string color, int speed) і викликаємо його через Invoke.
Console.WriteLine("\n--- Крок 3: Створення об'єкта через ConstructorInfo.Invoke ---");
ConstructorInfo? paramCtor = carType.GetConstructor(new[] { typeof(string), typeof(int) });
if (paramCtor != null)
{
    var carInstance = (Car)paramCtor.Invoke(new object[] { "Blue", 120 });
    Console.WriteLine($"  Створено автомобіль: Колір = {carInstance.Color}, Швидкість = {carInstance.Speed} км/год");
}

// -----------------------------------------------------------------------
// Крок 4: Виклик ПРИВАТНОГО конструктора (як це роблять ORM при читанні з БД)
// -----------------------------------------------------------------------
Console.WriteLine("\n--- Крок 4: Створення об'єкта через закритий private-конструктор ---");
ConstructorInfo? privateCtor = carType.GetConstructor(
    BindingFlags.NonPublic | BindingFlags.Instance,
    binder: null,
    types: new[] { typeof(string), typeof(double), typeof(int) },
    modifiers: null);

if (privateCtor != null)
{
    var secretCar = (Car)privateCtor.Invoke(new object[] { "SPEC-777", 260.0, 500 });
    Console.WriteLine($"  Створено спец-авто через private конструктор! Номер: {secretCar.LicensePlate}, Макс. швидкість: {secretCar.MaxSpeed}");
}

Console.WriteLine("\nВисновок: ConstructorInfo — це серце будь-якого Dependency Injection контейнера!");

static void PrintConstructorSignature(ConstructorInfo ctor)
{
    string visibility = ctor.IsPublic ? "public " : (ctor.IsPrivate ? "private" : "protected");
    string paramList = string.Join(", ", ctor.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
    Console.WriteLine($"  [{visibility}] {ctor.DeclaringType?.Name}({paramList})");
}
