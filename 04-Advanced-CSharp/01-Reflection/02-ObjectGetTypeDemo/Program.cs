using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 02. ВИЗНАЧЕННЯ РЕАЛЬНОГО ТИПУ ОБ'ЄКТА В ПАМ'ЯТІ: object.GetType()");
Console.WriteLine(" Як рефлексія бачить справжній тип об'єкта крізь інтерфейси та поліморфізм");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Поліморфна змінна проти реального об'єкта в купі (Heap)
// -----------------------------------------------------------------------
// Змінна оголошена як базовий інтерфейс ICar, але в пам'яті ми створюємо похідний клас SportsCar.
// Метод GetType() читає заголовок об'єкта (MethodTable pointer) безпосередньо в купі (Heap),
// тому він ЗАВЖДИ повертає точний реальний тип створеного екземпляра!
Console.WriteLine("--- Крок 1: Отримання реального типу через поліморфне посилання ICar ---");
ICar vehicle = new SportsCar("Yellow", 280, true);

Type compileTimeType = typeof(ICar);
Type runtimeType = vehicle.GetType();

Console.WriteLine($"  Оголошений тип змінної (typeof):   {compileTimeType.FullName}");
Console.WriteLine($"  Фактичний тип у пам'яті (GetType): {runtimeType.FullName}\n");

// -----------------------------------------------------------------------
// Крок 2: Точний збіг типів проти перевірки спорідненості в ієрархії
// -----------------------------------------------------------------------
// Ієрархія успадкування: Object -> Car -> SportsCar (реалізує ICar).
Console.WriteLine("--- Крок 2: Порівняння точного типу та перевірка ієрархії успадкування ---");
Console.WriteLine($"  Чи точний тип == SportsCar?                 {runtimeType == typeof(SportsCar)}");
Console.WriteLine($"  Чи точний тип == Car?                       {runtimeType == typeof(Car)} (бо це нащадок, а не сам Car)");
Console.WriteLine($"  Чи є нащадком класу Car (IsSubclassOf)?     {runtimeType.IsSubclassOf(typeof(Car))}");
Console.WriteLine($"  Чи сумісний з інтерфейсом ICar?             {typeof(ICar).IsAssignableFrom(runtimeType)}\n");

// -----------------------------------------------------------------------
// Крок 3: Збереження інформації про тип при упаковці (Boxing) у object
// -----------------------------------------------------------------------
// Навіть якщо покласти число int у безособову змінну типу object, CLR пам'ятає його справжній тип.
Console.WriteLine("--- Крок 3: Розпізнавання типу всередині змінної object ---");
object mysteryBox = 42;
Console.WriteLine($"  Значення в object: {mysteryBox}");
Console.WriteLine($"  Реальний тип всередині коробки: {mysteryBox.GetType().FullName}\n");

Console.WriteLine("Висновок: object.GetType() повертає точний тип живого екземпляра під час виконання програми.");
