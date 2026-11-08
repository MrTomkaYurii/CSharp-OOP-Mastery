using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 01. ОТРИМАННЯ МЕТАДАНИХ НА ЕТАПІ КОМПІЛЯЦІЇ: ОПЕРАТОР typeof(T)");
Console.WriteLine(" Як отримати паспорт типу (System.Type), коли клас відомий заздалегідь");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Отримання об'єкта Type для доменного класу
// -----------------------------------------------------------------------
// Оператор typeof(T) обчислюється компілятором Roslyn ще під час збірки (compile-time).
// Це найшвидший і найбезпечніший спосіб отримати метадані, бо не потребує створення екземпляра (new Car()).
Console.WriteLine("--- Крок 1: Базова інформація про клас Car через typeof(Car) ---");
Type carType = typeof(Car);

Console.WriteLine($"  Коротке ім'я (Name):         {carType.Name}");
Console.WriteLine($"  Повне ім'я (FullName):       {carType.FullName}");
Console.WriteLine($"  Простір імен (Namespace):    {carType.Namespace}");
Console.WriteLine($"  Базовий клас (BaseType):     {carType.BaseType?.Name}");
Console.WriteLine($"  Чи це структура (ValueType): {carType.IsValueType}\n");

// -----------------------------------------------------------------------
// Крок 2: Використання typeof для примітивних типів та системних структур
// -----------------------------------------------------------------------
// У системі типів .NET (CTS) навіть примітиви (int, bool, void) мають свій паспорт System.Type.
Console.WriteLine("--- Крок 2: Метадані примітивних типів (int, void) ---");
Type intType = typeof(int);
Type voidType = typeof(void);

Console.WriteLine($"  Псевдонім C# 'int' у CLR:    {intType.FullName} (IsPrimitive: {intType.IsPrimitive})");
Console.WriteLine($"  Ключове слово 'void' у CLR:  {voidType.FullName}\n");

// -----------------------------------------------------------------------
// Крок 3: Різниця між відкритим (Open) та закритим (Closed) дженериком
// -----------------------------------------------------------------------
// - Відкритий дженерик typeof(List<>) — це шаблон (креслення), у який ще не підставили конкретний тип T.
// - Закритий дженерик typeof(List<Car>) — це вже готовий повноцінний тип списку автомобілів.
Console.WriteLine("--- Крок 3: Відкриті та закриті узагальнені типи (Generics) ---");
Type openGeneric = typeof(List<>);
Type closedGeneric = typeof(List<Car>);

Console.WriteLine($"  Шаблон List<>:       Name = {openGeneric.Name,-10} | IsGenericTypeDefinition = {openGeneric.IsGenericTypeDefinition}");
Console.WriteLine($"  Готовий List<Car>:   Name = {closedGeneric.Name,-10} | IsGenericTypeDefinition = {closedGeneric.IsGenericTypeDefinition}");
Console.WriteLine($"  Підставлений тип T:  {closedGeneric.GetGenericArguments()[0].FullName}\n");

Console.WriteLine("Висновок: використовуйте typeof(T) завжди, коли ім'я типу відоме під час написання коду.");
