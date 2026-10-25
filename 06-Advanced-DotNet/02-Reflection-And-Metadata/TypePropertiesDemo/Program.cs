using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація характеристик і прапорців Type ===\n");

// Досліджуємо типи різних категорій
InspectType(typeof(Car));
InspectType(typeof(SportsCar));
InspectType(typeof(ICar));
InspectType(typeof(int));

static void InspectType(Type t)
{
    Console.WriteLine($"--------------------------------------------------");
    Console.WriteLine($"Інспекція типу: {t.Name} (Namespace: {t.Namespace})");
    Console.WriteLine($"--------------------------------------------------");
    Console.WriteLine($"Повне ім'я:      {t.FullName}");
    Console.WriteLine($"Збірка:          {t.Assembly.GetName().Name}");
    Console.WriteLine($"Базовий тип:     {t.BaseType?.Name ?? "немає (корінь або інтерфейс)"}");

    // Категорійні прапорці
    Console.WriteLine($"Категорія:       IsClass={t.IsClass}, IsValueType={t.IsValueType}, IsInterface={t.IsInterface}, IsEnum={t.IsEnum}");
    Console.WriteLine($"Модифікатори:    IsAbstract={t.IsAbstract}, IsSealed={t.IsSealed}, IsPublic={t.IsPublic}");
    Console.WriteLine($"Дженерики:       IsGenericType={t.IsGenericType}");

    // Інтерфейси
    Type[] interfaces = t.GetInterfaces();
    if (interfaces.Length > 0)
    {
        Console.WriteLine($"Інтерфейси:      {string.Join(", ", interfaces.Select(i => i.Name))}");
    }

    // Підрахунок членів за типами
    MemberInfo[] members = t.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    Console.WriteLine($"Власні публічні члени ({members.Length}):");
    foreach (var group in members.GroupBy(m => m.MemberType))
    {
        Console.WriteLine($"  - {group.Key}: {group.Count()} шт. ({string.Join(", ", group.Select(m => m.Name))})");
    }
    Console.WriteLine();
}
