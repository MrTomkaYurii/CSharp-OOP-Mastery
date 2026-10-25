using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація object.GetType() (Runtime Type Discovery) ===\n");

// 1. Поліморфне посилання на інтерфейс
ICar vehicle = new SportsCar("Yellow", 280, true);

// GetType() викликається віртуально і повертає фактичний тип створеного об'єкта в пам'яті
Type runtimeType = vehicle.GetType();
Console.WriteLine($"Тип змінної (compile-time): {nameof(ICar)}");
Console.WriteLine($"Фактичний тип об'єкта (runtime): {runtimeType.FullName}");

// 2. Порівняння точного типу та перевірка сумісності типів
Console.WriteLine("\n--- Перевірка типів ---");
Console.WriteLine($"Точний тип є SportsCar: {runtimeType == typeof(SportsCar)}"); // true
Console.WriteLine($"Точний тип є Car:       {runtimeType == typeof(Car)}");       // false (точний тип інший)
Console.WriteLine($"Чи є екземпляром Car (IsSubclassOf): {runtimeType.IsSubclassOf(typeof(Car))}"); // true
Console.WriteLine($"Чи реалізує ICar (IsAssignableFrom): {typeof(ICar).IsAssignableFrom(runtimeType)}"); // true

// 3. Робота з object
object mysteryBox = 42;
Console.WriteLine($"\nТип у boxing-обгортці mysteryBox: {mysteryBox.GetType().FullName}");
