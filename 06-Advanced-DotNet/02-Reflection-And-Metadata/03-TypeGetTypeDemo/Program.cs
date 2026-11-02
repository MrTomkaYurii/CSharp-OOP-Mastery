using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 03. ДИНАМІЧНИЙ ПОШУК ТИПУ ЗА РЯДКОВИМ ІМ'ЯМ: Type.GetType(string)");
Console.WriteLine(" Як знайти клас, коли його назва приходить із конфігурації або плагіна");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Пошук стандартних системних типів (CoreLib)
// -----------------------------------------------------------------------
// Для базових типів .NET (System.String, System.Int32, System.DateTime) достатньо вказати
// лише простір імен та назву класу — CLR автоматично знайде їх у системній збірці.
Console.WriteLine("--- Крок 1: Пошук системного типу за рядком 'System.String' ---");
Type? stringType = Type.GetType("System.String");
Console.WriteLine($"  Знайдено системний тип: {stringType?.FullName}\n");

// -----------------------------------------------------------------------
// Крок 2: Пошук типу в іншій збірці через AssemblyQualifiedName
// -----------------------------------------------------------------------
// Якщо клас лежить в іншій бібліотеці (наприклад, Shared.dll), простого імені "Shared.Common.Car" недостатньо.
// Потрібна повна адреса (AssemblyQualifiedName): "ПростірІмен.Клас, НазваЗбірки, Версія...".
Console.WriteLine("--- Крок 2: Пошук типу із зовнішньої збірки Shared.dll ---");
string carQualifiedName = typeof(Car).AssemblyQualifiedName!;
Console.WriteLine($"  Повна адреса типу (AssemblyQualifiedName):\n  {carQualifiedName}\n");

Type? loadedCarType = Type.GetType(carQualifiedName);
Console.WriteLine($"  Тип успішно знайдено за рядком: {loadedCarType?.FullName}\n");

// -----------------------------------------------------------------------
// Крок 3: Безпечний пошук неіснуючого типу (throwOnError: false, ignoreCase: true)
// -----------------------------------------------------------------------
// Параметр throwOnError: false каже CLR повернути null замість викидання TypeLoadException,
// а ignoreCase: true дозволяє не залежати від регістру літер.
Console.WriteLine("--- Крок 3: Безпечна перевірка неіснуючого типу ---");
Type? notFound = Type.GetType("NonExistent.PluginType", throwOnError: false, ignoreCase: true);
Console.WriteLine($"  Результат пошуку 'NonExistent.PluginType': {(notFound == null ? "null (програма не впала з помилкою)" : notFound.Name)}\n");

// -----------------------------------------------------------------------
// Крок 4: Завантаження зовнішнього .dll-файлу з диска (Assembly.LoadFrom)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 4: Завантаження збірки з файлу на диску ---");
string sharedDllPath = Path.Combine(AppContext.BaseDirectory, "Shared.dll");
if (File.Exists(sharedDllPath))
{
    Assembly loadedAssembly = Assembly.LoadFrom(sharedDllPath);
    Type? sportsCarType = loadedAssembly.GetType("Shared.Common.SportsCar");
    Console.WriteLine($"  Файл збірки:   {Path.GetFileName(sharedDllPath)}");
    Console.WriteLine($"  Знайдено клас: {sportsCarType?.FullName}");
}

Console.WriteLine("\nВисновок: Type.GetType(string) є основою плагінних систем, де імена класів зчитуються з JSON/XML.");
