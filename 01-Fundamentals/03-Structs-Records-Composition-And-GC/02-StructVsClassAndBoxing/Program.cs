namespace StructVsClassAndBoxing;

/// <summary>
/// Посилальний тип (Reference Type): об'єкт виділяється в Купі (Managed Heap),
/// а змінна зберігає лише 8-байтну адресу (посилання).
/// </summary>
public sealed class PointClass
{
    public int X { get; set; }
    public int Y { get; set; }

    public PointClass(int x, int y)
    {
        X = x;
        Y = y;
    }
}

/// <summary>
/// Значущий тип (Value Type): дані зберігаються безпосередньо на Стеку (або всередині масиву)
/// без виділення пам'яті в Купі та без навантаження на Garbage Collector.
/// Модифікатор readonly гарантує незмінність полів структури.
/// </summary>
public readonly struct GeoCoordinateStruct
{
    public double Latitude { get; }
    public double Longitude { get; }

    public GeoCoordinateStruct(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 02. ПОРІВНЯННЯ STRUCT ПРОТИ CLASS ТА МЕХАНІЗМ BOXING / UNBOXING");
        Console.WriteLine(" Як значущі типи (Value Types) копіюються за значенням та пакуються в object");
        Console.WriteLine("==========================================================================\n");

        // 1. Поведінка при копіюванні: Class (копія посилання) проти Struct (повна копія байтів)
        Console.WriteLine("--- 1. Присвоєння змінних: Class (посилання) vs Struct (копія значення) ---");
        var classA = new PointClass(10, 20);
        PointClass classB = classA; // Обидві змінні вказують на ОДИН об'єкт у Купі!
        classB.X = 999;
        Console.WriteLine($"  PointClass: classA.X = {classA.X}, classB.X = {classB.X} (змінився оригінал!)");

        var structA = new GeoCoordinateStruct(50.4501, 30.5234);
        GeoCoordinateStruct structB = structA; // Створено незалежну побайтову копію на Стеку!
        structB = new GeoCoordinateStruct(49.8397, 24.0297);
        Console.WriteLine($"  GeoStruct:  structA.Lat = {structA.Latitude}, structB.Lat = {structB.Latitude} (оригінал не змінено!)\n");

        // 2. Анатомія Boxing (пакування) та Unboxing (розпакування)
        Console.WriteLine("--- 2. Що таке Boxing (пакування структури в Купу) та Unboxing ---");
        int valueOnStack = 42;

        // BOXING: CLR виділяє новий об'єкт у Купі (Heap) і копіює туди значення 42 зі Стеку
        object boxedObjectOnHeap = valueOnStack;

        // UNBOXING: Перевірка типу в Купі та зворотне копіювання значення на Стек
        int unboxedValue = (int)boxedObjectOnHeap;

        Console.WriteLine($"  Значення на стеку: {valueOnStack}");
        Console.WriteLine($"  Упакований об'єкт у купі (boxed): {boxedObjectOnHeap}");
        Console.WriteLine($"  Розпаковане значення (unboxed):   {unboxedValue}");
        Console.WriteLine("Порада: уникайте передачі struct у методи, що приймають 'object', щоб не створювати зайве сміття для GC.");
    }
}
