using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 08. ДОСЛІДЖЕННЯ ТА ДИНАМІЧНИЙ ВИКЛИК МЕТОДІВ (MethodInfo та Invoke)");
Console.WriteLine(" Як знайти метод за назвою і викликати його без прямого звернення в коді");
Console.WriteLine("==========================================================================\n");

var car = new Car("Black", 40);
Type carType = typeof(Car);

// -----------------------------------------------------------------------
// Крок 1: Перелік власних публічних методів класу (BindingFlags.DeclaredOnly)
// -----------------------------------------------------------------------
// Прапорець DeclaredOnly прибирає зі списку стандартні методи, успадковані від System.Object
// (ToString, Equals, GetHashCode, GetType), залишаючи лише методи самого класу Car.
Console.WriteLine("--- Крок 1: Власні публічні методи класу Car (DeclaredOnly) ---");
MethodInfo[] declaredMethods = carType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
foreach (MethodInfo method in declaredMethods)
{
    string parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
    Console.WriteLine($"  - {method.ReturnType.Name,-8} {method.Name}({parameters})");
}

// -----------------------------------------------------------------------
// Крок 2: Динамічний виклик екземплярного методу з аргументами (MethodInfo.Invoke)
// -----------------------------------------------------------------------
// Перший параметр Invoke — це цільовий об'єкт (car), на якому викликається метод.
// Другий параметр — масив аргументів object?[] (у нашому випадку { 35 }).
Console.WriteLine("\n--- Крок 2: Динамічний виклик car.Accelerate(35) через Invoke ---");
MethodInfo? accelerateMethod = carType.GetMethod("Accelerate", new[] { typeof(int) });
if (accelerateMethod != null)
{
    Console.WriteLine($"  Швидкість до виклику:       {car.Speed} км/год");
    accelerateMethod.Invoke(car, new object[] { 35 });
    Console.WriteLine($"  Швидкість після Accelerate: {car.Speed} км/год");
}

// -----------------------------------------------------------------------
// Крок 3: Виклик статичного методу (BindingFlags.Static)
// -----------------------------------------------------------------------
// Оскільки статичний метод не прив'язаний до жодного об'єкта, першим аргументом у Invoke передається null!
Console.WriteLine("\n--- Крок 3: Виклик статичного методу Car.GetTotalProduced() ---");
MethodInfo? staticMethod = carType.GetMethod("GetTotalProduced", BindingFlags.Public | BindingFlags.Static);
if (staticMethod != null)
{
    object? totalProduced = staticMethod.Invoke(obj: null, parameters: null);
    Console.WriteLine($"  Результат статичного виклику: випущено {totalProduced} авто");
}

// -----------------------------------------------------------------------
// Крок 4: Виклик закритого приватного методу (BindingFlags.NonPublic)
// -----------------------------------------------------------------------
Console.WriteLine("\n--- Крок 4: Виклик приватного методу ResetOdometer() ---");
MethodInfo? resetMethod = carType.GetMethod("ResetOdometer", BindingFlags.NonPublic | BindingFlags.Instance);
if (resetMethod != null)
{
    resetMethod.Invoke(car, parameters: null);
    Console.WriteLine("  Приватний метод ResetOdometer() успішно виконано через рефлексію!");
}

Console.WriteLine("\nВисновок: MethodInfo.Invoke дозволяє викликати будь-який метод динамічно (наприклад, у тестових фреймворках).");
