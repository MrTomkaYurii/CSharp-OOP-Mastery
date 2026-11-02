using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 06. ІНСПЕКЦІЯ ІНТЕРФЕЙСІВ ТА КАРТИ МЕТОДІВ (GetInterfaces & GetInterfaceMap)");
Console.WriteLine(" Як перевірити, які контракти підтримує клас і які методи їх реалізують");
Console.WriteLine("==========================================================================\n");

Type carType = typeof(Car);

// -----------------------------------------------------------------------
// Крок 1: Отримання списку всіх інтерфейсів, які реалізує клас
// -----------------------------------------------------------------------
Console.WriteLine($"--- Крок 1: Усі інтерфейси, реалізовані класом '{carType.Name}' ---");
Type[] interfaces = carType.GetInterfaces();
foreach (Type iface in interfaces)
{
    Console.WriteLine($"  - {iface.FullName}");
}

// -----------------------------------------------------------------------
// Крок 2: Перевірка сумісності через IsAssignableFrom
// -----------------------------------------------------------------------
// Вираз targetInterface.IsAssignableFrom(carType) читається так:
// "Чи можна у змінну типу ICar присвоїти об'єкт типу Car?"
Console.WriteLine("\n--- Крок 2: Перевірка реалізації контракту через IsAssignableFrom ---");
Type targetInterface = typeof(ICar);
bool implementsICar = targetInterface.IsAssignableFrom(carType);
Console.WriteLine($"  Чи можна присвоїти Car у змінну ICar? -> {implementsICar}");

// -----------------------------------------------------------------------
// Крок 3: Побудова карти інтерфейсу (InterfaceMapping)
// -----------------------------------------------------------------------
// GetInterfaceMap показує точний зв'язок між оголошенням методу в інтерфейсі
// та конкретним методом у класі, який виконує цю роботу.
Console.WriteLine($"\n--- Крок 3: Карта зіставлення методів (GetInterfaceMap для {targetInterface.Name}) ---");
InterfaceMapping map = carType.GetInterfaceMap(targetInterface);

for (int i = 0; i < map.InterfaceMethods.Length; i++)
{
    MethodInfo ifaceMethod = map.InterfaceMethods[i];
    MethodInfo targetMethod = map.TargetMethods[i];
    Console.WriteLine($"  Метод контракту: {ifaceMethod.DeclaringType?.Name}.{ifaceMethod.Name}()  ===>  Метод класу: {targetMethod.DeclaringType?.Name}.{targetMethod.Name}()");
}

Console.WriteLine("\nВисновок: IsAssignableFrom та GetInterfaces — головні інструменти перевірки сумісності типів.");
