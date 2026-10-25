using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація інтерфейсів у рефлексії (Interfaces & Mapping) ===\n");

Type carType = typeof(Car);

// 1. Отримання всіх реалізованих інтерфейсів
Console.WriteLine($"--- Інтерфейси, реалізовані класом {carType.Name} ---");
Type[] interfaces = carType.GetInterfaces();
foreach (Type iface in interfaces)
{
    Console.WriteLine($"  - {iface.FullName}");
}

// 2. Перевірка реалізації інтерфейсу (IsAssignableFrom)
Type targetInterface = typeof(ICar);
bool implementsICar = targetInterface.IsAssignableFrom(carType);
Console.WriteLine($"\nКлас {carType.Name} реалізує {targetInterface.Name}: {implementsICar}");

// 3. Карта інтерфейсу (GetInterfaceMap)
// Показує точну відповідність методів інтерфейсу та їхніх реалізацій у класі
Console.WriteLine($"\n--- Карта зіставлення методів для {targetInterface.Name} (Interface Mapping) ---");
InterfaceMapping map = carType.GetInterfaceMap(targetInterface);

for (int i = 0; i < map.InterfaceMethods.Length; i++)
{
    MethodInfo ifaceMethod = map.InterfaceMethods[i];
    MethodInfo targetMethod = map.TargetMethods[i];
    Console.WriteLine($"  Інтерфейс: {ifaceMethod.Name}  ===>  Реалізація: {targetMethod.DeclaringType?.Name}.{targetMethod.Name}");
}
