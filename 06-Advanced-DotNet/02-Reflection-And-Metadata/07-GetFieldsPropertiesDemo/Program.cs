using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація FieldInfo та PropertyInfo (Поля і властивості) ===\n");

var car = new Car("Silver", 80);
Type carType = typeof(Car);

// 1. Інспекція публічних полів
Console.WriteLine("--- Публічні поля ---");
FieldInfo[] publicFields = carType.GetFields();
foreach (FieldInfo field in publicFields)
{
    Console.WriteLine($"  Field: {field.Name}, Type: {field.FieldType.Name}, Value: {field.GetValue(car)}");
}

// 2. Доступ до приватних полів та зміна значення (обхід інкапсуляції)
Console.WriteLine("\n--- Приватні поля (через BindingFlags.NonPublic | BindingFlags.Instance) ---");
FieldInfo[] privateFields = carType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
foreach (FieldInfo field in privateFields)
{
    Console.WriteLine($"  [private] {field.FieldType.Name} {field.Name} = {field.GetValue(car)}");
}

// Зміна значення закритого приватного поля _licensePlate
FieldInfo? plateField = carType.GetField("_licensePlate", BindingFlags.NonPublic | BindingFlags.Instance);
if (plateField != null)
{
    Console.WriteLine($"\nСтаре значення _licensePlate: {plateField.GetValue(car)}");
    plateField.SetValue(car, "HACKED-99");
    Console.WriteLine($"Нове значення після SetValue:  {car.LicensePlate}");
}

// 3. Інспекція властивостей та перевірка геттерів/сеттерів
Console.WriteLine("\n--- Властивості (PropertyInfo) ---");
PropertyInfo[] properties = carType.GetProperties();
foreach (PropertyInfo prop in properties)
{
    bool canRead = prop.CanRead;
    bool canWrite = prop.CanWrite;
    object? val = canRead ? prop.GetValue(car) : "<недоступно для читання>";
    Console.WriteLine($"  Property: {prop.Name} ({prop.PropertyType.Name}) | CanRead={canRead}, CanWrite={canWrite} | Значення={val}");
}
