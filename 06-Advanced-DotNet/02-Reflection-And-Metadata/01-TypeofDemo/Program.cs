using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація оператора typeof (Compile-Time Reflection) ===\n");

// 1. Отримання Type для конкретного класу під час компіляції (найшвидший і типобезпечний спосіб)
Type carType = typeof(Car);
Console.WriteLine($"Клас: {carType.FullName}");
Console.WriteLine($"Базовий тип: {carType.BaseType?.Name}");
Console.WriteLine($"Це тип значення: {carType.IsValueType}");

// 2. typeof для примітивних типів та системних структур
Type intType = typeof(int);
Type voidType = typeof(void);
Console.WriteLine($"\nСистемний тип int: {intType.FullName} (IsPrimitive: {intType.IsPrimitive})");
Console.WriteLine($"Системний тип void: {voidType.FullName}");

// 3. typeof для відкритих і закритих дженериків
Type openGeneric = typeof(List<>);
Type closedGeneric = typeof(List<Car>);

Console.WriteLine($"\nВідкритий дженерик List<>: {openGeneric.Name} (IsGenericTypeDefinition: {openGeneric.IsGenericTypeDefinition})");
Console.WriteLine($"Закритий дженерик List<Car>: {closedGeneric.Name} (IsGenericType: {closedGeneric.IsGenericType})");
Console.WriteLine($"Аргумент типу закритого дженерика: {closedGeneric.GetGenericArguments()[0].Name}");
