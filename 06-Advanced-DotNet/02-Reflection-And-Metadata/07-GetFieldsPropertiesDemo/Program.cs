using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 07. ДОСЛІДЖЕННЯ ПОЛІВ І ВЛАСТИВОСТЕЙ (FieldInfo, PropertyInfo та BindingFlags)");
Console.WriteLine(" Як читати й змінювати стан об'єкта, включно із закритими private-полями");
Console.WriteLine("==========================================================================\n");

var car = new Car("Silver", 80);
Type carType = typeof(Car);

// -----------------------------------------------------------------------
// Крок 1: Читання відкритих (public) полів екземпляра
// -----------------------------------------------------------------------
// За замовчуванням GetFields() повертає ТІЛЬКИ public-поля.
Console.WriteLine("--- Крок 1: Відкриті поля класу Car (GetFields) ---");
FieldInfo[] publicFields = carType.GetFields();
foreach (FieldInfo field in publicFields)
{
    Console.WriteLine($"  [public] {field.FieldType.Name} {field.Name} = {field.GetValue(car)}");
}

// -----------------------------------------------------------------------
// Крок 2: Доступ до закритих (private) полів через прапорці BindingFlags
// -----------------------------------------------------------------------
// Важливо: комбінація BindingFlags.NonPublic | BindingFlags.Instance каже CLR:
// "Покажи непублічні поля, які належать конкретному екземпляру об'єкта (а не static)".
Console.WriteLine("\n--- Крок 2: Приватні поля екземпляра (BindingFlags.NonPublic | BindingFlags.Instance) ---");
FieldInfo[] privateFields = carType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
foreach (FieldInfo field in privateFields)
{
    Console.WriteLine($"  [private] {field.FieldType.Name,-8} {field.Name,-15} = {field.GetValue(car)}");
}

// -----------------------------------------------------------------------
// Крок 3: Модифікація приватного поля в обхід інкапсуляції (SetValue)
// -----------------------------------------------------------------------
// У звичайному C#-коді рядок car._licensePlate = "..." викликає помилку компіляції.
// Але для рефлексії модифікатор private не є перешкодою під час виконання!
Console.WriteLine("\n--- Крок 3: Зміна приватного поля _licensePlate через FieldInfo.SetValue ---");
FieldInfo? plateField = carType.GetField("_licensePlate", BindingFlags.NonPublic | BindingFlags.Instance);
if (plateField != null)
{
    Console.WriteLine($"  Номер авто ДО втручання:    {car.LicensePlate}");
    plateField.SetValue(car, "KA 7777 OO");
    Console.WriteLine($"  Номер авто ПІСЛЯ SetValue:  {car.LicensePlate}");
}

// -----------------------------------------------------------------------
// Крок 4: Інспекція властивостей (PropertyInfo)
// -----------------------------------------------------------------------
// Властивість у C# — це пара методів get_Name() та set_Name().
// Властивості CanRead та CanWrite підказують, чи є у властивості геттер і сеттер.
Console.WriteLine("\n--- Крок 4: Публічні властивості класу Car (GetProperties) ---");
PropertyInfo[] properties = carType.GetProperties();
foreach (PropertyInfo prop in properties)
{
    object? value = prop.CanRead ? prop.GetValue(car) : "<недоступно>";
    Console.WriteLine($"  Property: {prop.Name,-14} ({prop.PropertyType.Name,-6}) | CanRead={prop.CanRead,-5} | CanWrite={prop.CanWrite,-5} | Значення = {value}");
}

Console.WriteLine("\nВисновок: PropertyInfo є основою всіх ORM (Entity Framework) та серіалізаторів (JSON/XML).");
