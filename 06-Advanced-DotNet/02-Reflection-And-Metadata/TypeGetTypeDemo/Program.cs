using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація Type.GetType() (Dynamic String-Based Resolution) ===\n");

// 1. Завантаження системного типу безпосередньо за іменем (типи з corelib знаходяться автоматично)
Type? stringType = Type.GetType("System.String");
Console.WriteLine($"Системний тип: {stringType?.FullName}");

// 2. Завантаження типу із зовнішньої збірки вимагає Assembly Qualified Name: "Namespace.Type, AssemblyName"
string carQualifiedName = typeof(Car).AssemblyQualifiedName!;
Console.WriteLine($"\nAssembly Qualified Name для Car:\n{carQualifiedName}");

Type? loadedCarType = Type.GetType(carQualifiedName);
Console.WriteLine($"\nТип успішно завантажено: {loadedCarType?.FullName}");

// 3. Спроба завантажити неіснуючий тип з обробкою помилки (throwOnError: false)
Type? notFound = Type.GetType("NonExistent.CustomType", throwOnError: false, ignoreCase: true);
Console.WriteLine($"Пошук неіснуючого типу (throwOnError=false): {(notFound == null ? "null (без винятку)" : notFound.Name)}");

// 4. Динамічне завантаження збірки за відносним шляхом через Assembly.LoadFrom
string sharedDllPath = Path.Combine(AppContext.BaseDirectory, "Shared.dll");
if (File.Exists(sharedDllPath))
{
    Assembly loadedAssembly = Assembly.LoadFrom(sharedDllPath);
    Type? sportsCarType = loadedAssembly.GetType("Shared.Common.SportsCar");
    Console.WriteLine($"\nЗавантажено з {Path.GetFileName(sharedDllPath)}: {sportsCarType?.FullName}");
}
