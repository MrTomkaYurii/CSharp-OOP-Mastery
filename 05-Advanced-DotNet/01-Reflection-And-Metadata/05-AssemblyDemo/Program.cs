using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 05. ДОСЛІДЖЕННЯ ЗБІРОК ТА АВТОМАТИЧНИЙ ПОШУК ТИПІВ (System.Reflection.Assembly)");
Console.WriteLine(" Як просканувати .dll-бібліотеку та автоматично знайти всі реалізації інтерфейсу");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Отримання поточної виконуваної збірки (.exe / .dll)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Метадані поточної виконуваної збірки (GetExecutingAssembly) ---");
Assembly executingAssembly = Assembly.GetExecutingAssembly();
AssemblyName currentName = executingAssembly.GetName();
Console.WriteLine($"  Ім'я поточної збірки: {currentName.Name}");
Console.WriteLine($"  Версія збірки:        {currentName.Version}\n");

// -----------------------------------------------------------------------
// Крок 2: Дослідження підключеної бібліотеки класів (Shared.dll)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 2: Паспорт зовнішньої збірки Shared.dll ---");
Assembly sharedAssembly = typeof(Car).Assembly;
AssemblyName sharedName = sharedAssembly.GetName();
Console.WriteLine($"  Коротке ім'я:   {sharedName.Name}");
Console.WriteLine($"  Повне ім'я:     {sharedName.FullName}");
Console.WriteLine($"  Шлях на диску:  {sharedAssembly.Location}\n");

// -----------------------------------------------------------------------
// Крок 3: Перелік усіх типів, скомпільованих у збірці (GetTypes)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 3: Перелік усіх типів у збірці Shared.dll ---");
foreach (Type type in sharedAssembly.GetTypes())
{
    Console.WriteLine($"  - {type.FullName,-30} [ Модифікатори: {type.Attributes} ]");
}

// -----------------------------------------------------------------------
// Крок 4: Практичний сценарій — автоматичний пошук класів, що реалізують ICar
// -----------------------------------------------------------------------
// Саме так фреймворки (ASP.NET Core, MediatR, xUnit) автоматично знаходять усі ваші контролери або тести!
Console.WriteLine("\n--- Крок 4: Автоматичний пошук конкретних класів, що реалізують інтерфейс ICar ---");
var carImplementations = sharedAssembly.GetTypes()
    .Where(t => typeof(ICar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (Type implType in carImplementations)
{
    Console.WriteLine($"  [Знайдено плагін]: Клас '{implType.Name}' реалізує контракт 'ICar'");
}

// -----------------------------------------------------------------------
// Крок 5: Зчитування атрибутів рівня збірки ([assembly: ...])
// -----------------------------------------------------------------------
Console.WriteLine("\n--- Крок 5: Системні атрибути, вбудовані в маніфест збірки ---");
foreach (Attribute attr in sharedAssembly.GetCustomAttributes())
{
    Console.WriteLine($"  [{attr.GetType().Name}]");
}

Console.WriteLine("\nВисновок: сканування Assembly.GetTypes() дозволяє програмі знаходити нові класи автоматично.");
