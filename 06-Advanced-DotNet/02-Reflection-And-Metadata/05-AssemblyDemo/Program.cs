using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація рефлексії збірок (Assembly Reflection) ===\n");

// 1. Отримання поточної виконуваної збірки
Assembly executingAssembly = Assembly.GetExecutingAssembly();
Console.WriteLine($"Поточна збірка: {executingAssembly.GetName().Name}, Версія: {executingAssembly.GetName().Version}");

// 2. Отримання зовнішньої збірки через тип із неї
Assembly sharedAssembly = typeof(Car).Assembly;
AssemblyName sharedName = sharedAssembly.GetName();
Console.WriteLine($"\nЗовнішня збірка (Shared): {sharedName.Name}");
Console.WriteLine($"Повне ім'я: {sharedName.FullName}");
Console.WriteLine($"Розташування: {sharedAssembly.Location}");

// 3. Перелік усіх типів та експортованих (публічних) типів
Console.WriteLine("\n--- Усі типи у збірці Shared (GetTypes) ---");
foreach (Type type in sharedAssembly.GetTypes())
{
    Console.WriteLine($"  [{type.Attributes}] {type.FullName}");
}

// 4. Пошук типів за певною умовою (наприклад, реалізують ICar)
Console.WriteLine("\n--- Пошук реалізацій ICar через рефлексію ---");
var carImplementations = sharedAssembly.GetTypes()
    .Where(t => typeof(ICar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (Type carType in carImplementations)
{
    Console.WriteLine($"  Знайдено реалізацію: {carType.Name}");
}

// 5. Атрибути збірки
Console.WriteLine("\n--- Атрибути збірки ---");
foreach (Attribute attr in sharedAssembly.GetCustomAttributes())
{
    Console.WriteLine($"  {attr.GetType().Name}");
}
