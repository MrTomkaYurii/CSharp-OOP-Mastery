using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація пізнього зв'язування (Late Binding & Activator) ===\n");

// 1. Динамічне завантаження типу через його AssemblyQualifiedName або збірку
Assembly sharedAssembly = typeof(Car).Assembly;
Type carType = sharedAssembly.GetType("Shared.Common.Car")!;

Console.WriteLine($"Отримано тип у runtime: {carType.FullName}");

// 2. Створення екземпляра через Activator.CreateInstance (конструктор за замовчуванням)
object defaultCar = Activator.CreateInstance(carType)!;
Console.WriteLine($"Створено об'єкт за замовчуванням: {defaultCar}");

// 3. Створення екземпляра з передачею аргументів у конструктор
object customCar = Activator.CreateInstance(carType, "Metallic Red", 110)!;
Console.WriteLine($"Створено параметризований об'єкт: Колір={carType.GetField("Color")?.GetValue(customCar)}, Швидкість={carType.GetProperty("Speed")?.GetValue(customCar)}");

// 4. Динамічний виклик методу без параметрів (IsMoving)
MethodInfo isMovingMethod = carType.GetMethod("IsMoving")!;
bool isMoving = (bool)isMovingMethod.Invoke(customCar, null)!;
Console.WriteLine($"\nРезультат IsMoving(): {isMoving}");

// 5. Динамічний виклик методу з параметрами (CalculateMPG)
MethodInfo calcMpgMethod = carType.GetMethod("CalculateMPG")!;
object[] mpgArgs = new object[] { 1000, 1450, 15.0 }; // startMiles, endMiles, gallons
double mpg = (double)calcMpgMethod.Invoke(customCar, mpgArgs)!;
Console.WriteLine($"Результат CalculateMPG(1000, 1450, 15.0): {mpg:F2} миль на галон");

// 6. Безпечна обробка винятків при виклику (TargetInvocationException)
Console.WriteLine("\n--- Обробка помилок при динамічному виклику ---");
try
{
    // Виклик методу з некоректними типами аргументів
    calcMpgMethod.Invoke(customCar, new object[] { "не число", 200, 10 });
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Спіймано очікуваний ArgumentException: {ex.Message}");
}
catch (TargetInvocationException ex)
{
    Console.WriteLine($"Помилка всередині викликаного методу: {ex.InnerException?.Message}");
}
