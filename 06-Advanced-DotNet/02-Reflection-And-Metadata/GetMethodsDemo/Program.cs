using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація MethodInfo (Методи, перевантаження та виклик) ===\n");

var car = new Car("Black", 40);
Type carType = typeof(Car);

// 1. Отримання тільки оголошених у цьому класі методів (без методів System.Object)
Console.WriteLine("--- Власні відкриті методи класу Car (DeclaredOnly) ---");
MethodInfo[] declaredMethods = carType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
foreach (MethodInfo method in declaredMethods)
{
    Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))})");
}

// 2. Динамічний виклик публічного екземплярного методу з параметрами
Console.WriteLine("\n--- Виклик методу Accelerate(35) через MethodInfo.Invoke ---");
MethodInfo? accelerateMethod = carType.GetMethod("Accelerate", new[] { typeof(int) });
if (accelerateMethod != null)
{
    Console.WriteLine($"Швидкість до виклику:  {car.Speed}");
    accelerateMethod.Invoke(car, new object[] { 35 });
    Console.WriteLine($"Швидкість після Accelerate: {car.Speed}");
}

// 3. Виклик статичного методу (перший параметр Invoke передається як null)
Console.WriteLine("\n--- Виклик статичного методу GetTotalProduced ---");
MethodInfo? staticMethod = carType.GetMethod("GetTotalProduced", BindingFlags.Public | BindingFlags.Static);
if (staticMethod != null)
{
    object? total = staticMethod.Invoke(null, null);
    Console.WriteLine($"Результат статичного методу: {total}");
}

// 4. Пошук та виклик приватного методу ResetOdometer
Console.WriteLine("\n--- Виклик приватного методу ResetOdometer ---");
MethodInfo? resetMethod = carType.GetMethod("ResetOdometer", BindingFlags.NonPublic | BindingFlags.Instance);
if (resetMethod != null)
{
    resetMethod.Invoke(car, null);
    Console.WriteLine("Приватний метод ResetOdometer успішно викликано через рефлексію!");
}
