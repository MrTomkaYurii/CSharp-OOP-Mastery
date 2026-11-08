using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 04. ДОСЛІДЖЕННЯ ВЛАСТИВОСТЕЙ ТА ХАРАКТЕРИСТИК ТИПУ (Type Properties)");
Console.WriteLine(" Як програмно відрізнити клас від структури, інтерфейсу чи абстрактного типу");
Console.WriteLine("==========================================================================\n");

// По черзі передаємо в наш універсальний інспектор 4 різні види типів:
// 1. Звичайний клас (Car)
// 2. Похідний клас (SportsCar)
// 3. Інтерфейс (ICar)
// 4. Значущий примітивний тип / структуру (int -> System.Int32)
InspectType(typeof(Car));
InspectType(typeof(SportsCar));
InspectType(typeof(ICar));
InspectType(typeof(int));

Console.WriteLine("Висновок: властивості IsClass, IsInterface, IsAbstract дозволяють DI-контейнерам");
Console.WriteLine("та серіалізаторам перевіряти, чи можна створити екземпляр типу через 'new'.");

static void InspectType(Type t)
{
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.WriteLine($" Паспорт типу: {t.Name} (Простір імен: {t.Namespace})");
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.WriteLine($"  Повне ім'я:      {t.FullName}");
    Console.WriteLine($"  Збірка:          {t.Assembly.GetName().Name}");
    Console.WriteLine($"  Базовий тип:     {t.BaseType?.Name ?? "немає (це інтерфейс або System.Object)"}");

    // 1. Категорія типу в системі CTS
    Console.WriteLine($"  Категорія:       IsClass={t.IsClass}, IsValueType={t.IsValueType}, IsInterface={t.IsInterface}, IsEnum={t.IsEnum}");

    // 2. Модифікатори класу (увага: інтерфейси також мають IsAbstract = true!)
    Console.WriteLine($"  Модифікатори:    IsAbstract={t.IsAbstract}, IsSealed={t.IsSealed}, IsPublic={t.IsPublic}");

    // 3. Перевірка реалізованих інтерфейсів
    Type[] interfaces = t.GetInterfaces();
    if (interfaces.Length > 0)
    {
        Console.WriteLine($"  Інтерфейси:      {string.Join(", ", interfaces.Select(i => i.Name))}");
    }

    // 4. Огляд власних публічних членів (DeclaredOnly відсікає успадковані методи ToString/Equals)
    MemberInfo[] members = t.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    Console.WriteLine($"  Власні публічні члени ({members.Length} шт.):");
    foreach (var group in members.GroupBy(m => m.MemberType))
    {
        Console.WriteLine($"    - {group.Key,-12}: {group.Count()} шт. ({string.Join(", ", group.Select(m => m.Name))})");
    }
    Console.WriteLine();
}
