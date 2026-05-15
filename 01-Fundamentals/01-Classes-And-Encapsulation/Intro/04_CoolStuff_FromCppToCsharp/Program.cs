// ============================================================================
//  INTRO 04 — Цікаві речі C#, яких бракує у класичному C++
// ----------------------------------------------------------------------------
//  Коротка вітрина того, заради чого варто радіти переходу:
//   * LINQ — запити до колекцій одним рядком (Where / Select / OrderBy / GroupBy...).
//   * Готові колекції: List<T>, Dictionary<K,V>, HashSet<T> зі зручними ініціалізаторами.
//   * records — незмінні типи з рівністю за значенням і копіюванням через "with".
//   * Зіставлення зі зразком, кортежі, деконструкція.
//   * string.Split / string.Join, інтерполяція з форматуванням.
//   * null-безпека: ?.  ??  ??=
// ============================================================================

namespace Intro;

// record — одразу отримуємо конструктор, властивості, ToString, рівність за значенням.
internal record Person(string Name, int Age, string City);

internal static class Program
{
    private static readonly Person[] People =
    [
        new("Аня",    20, "Львів"),
        new("Богдан",  22, "Київ"),
        new("Оля",     19, "Львів"),
        new("Тарас",   25, "Київ"),
        new("Марія",   21, "Одеса"),
    ];

    private static void Main()
    {
        Section("1. LINQ — запити до даних");
        LinqDemo();

        Section("2. Колекції");
        CollectionsDemo();

        Section("3. records: рівність за значенням і 'with'");
        RecordsDemo();

        Section("4. Зіставлення зі зразком + деконструкція");
        PatternsDemo();

        Section("5. Робота з рядками");
        StringsDemo();

        Section("6. null-безпека");
        NullSafetyDemo();
    }

    // ------------------------------------------------------------------------
    private static void LinqDemo()
    {
        // "Знайти імена повнолітніх зі Львова, за абеткою" — одним ланцюжком:
        var lvivAdults = People
            .Where(p => p.City == "Львів" && p.Age >= 18)
            .OrderBy(p => p.Name)
            .Select(p => p.Name);
        Console.WriteLine($"Львів'яни 18+: {string.Join(", ", lvivAdults)}");

        // Агрегати:
        Console.WriteLine($"Всього людей: {People.Count()}");
        Console.WriteLine($"Середній вік: {People.Average(p => p.Age):F1}");
        Console.WriteLine($"Наймолодша: {People.MinBy(p => p.Age)!.Name}");
        Console.WriteLine($"Є хтось з Одеси? {People.Any(p => p.City == "Одеса")}");

        // Групування за містом:
        foreach (var group in People.GroupBy(p => p.City).OrderBy(g => g.Key))
            Console.WriteLine($"  {group.Key}: {group.Count()} осіб ({string.Join(", ", group.Select(p => p.Name))})");

        // Числовий приклад: сума квадратів парних чисел 1..10
        int sumSqEven = Enumerable.Range(1, 10).Where(n => n % 2 == 0).Sum(n => n * n);
        Console.WriteLine($"Сума квадратів парних 1..10 = {sumSqEven}");
    }

    // ------------------------------------------------------------------------
    private static void CollectionsDemo()
    {
        // List<T> — динамічний масив (аналог std::vector).
        var list = new List<int> { 3, 1, 4, 1, 5 };
        list.Add(9);
        list.Sort();
        Console.WriteLine($"List: [{string.Join(", ", list)}], count = {list.Count}");

        // Dictionary<K,V> — асоціативний масив (аналог std::unordered_map).
        var phone = new Dictionary<string, string>
        {
            ["Аня"] = "111",
            ["Богдан"] = "222",
        };
        phone["Оля"] = "333";
        if (phone.TryGetValue("Аня", out string? number))
            Console.WriteLine($"Телефон Ані: {number}");
        Console.WriteLine($"Є запис для 'Тарас'? {phone.ContainsKey("Тарас")}");

        // HashSet<T> — множина унікальних значень.
        var seen = new HashSet<int>();
        int[] stream = { 1, 2, 2, 3, 3, 3, 4 };
        foreach (int x in stream) seen.Add(x);
        Console.WriteLine($"Унікальні: [{string.Join(", ", seen.Order())}]");
    }

    // ------------------------------------------------------------------------
    private static void RecordsDemo()
    {
        var a = new Person("Аня", 20, "Львів");
        var b = new Person("Аня", 20, "Львів");

        // Рівність за ЗНАЧЕННЯМ (для class було б порівняння посилань):
        Console.WriteLine($"a == b -> {a == b}");                 // True
        Console.WriteLine($"a -> {a}");                           // згенерований ToString

        // 'with' — копія зі зміненими полями (оригінал незмінний):
        var older = a with { Age = a.Age + 1 };
        Console.WriteLine($"було: {a.Age}, стало у копії: {older.Age}");

        // Деконструкція record у змінні:
        var (name, age, city) = older;
        Console.WriteLine($"деконструкція: {name} / {age} / {city}");
    }

    // ------------------------------------------------------------------------
    private static void PatternsDemo()
    {
        foreach (var p in People)
            Console.WriteLine($"{p.Name,-8} -> {Category(p)}");

        // Зразок за властивостями + реляційні зразки + кортежний зразок.
        static string Category(Person p) => p switch
        {
            { Age: < 20 } => "підліток",
            { Age: >= 20 and < 24, City: "Львів" } => "молодь зі Львова",
            { Age: >= 20 and < 24 } => "молодь",
            { Age: >= 24 } => "доросла людина",
            _ => "інша категорія",
        };
    }

    // ------------------------------------------------------------------------
    private static void StringsDemo()
    {
        string csv = "яблуко;банан;вишня;яблуко";

        string[] parts = csv.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        Console.WriteLine($"частин: {parts.Length}");

        string joined = string.Join(" + ", parts.Distinct());
        Console.WriteLine($"унікальні: {joined}");

        // Інтерполяція з вирівнюванням і форматом — зручна табличка:
        Console.WriteLine($"{"Товар",-10}{"Ціна",8}");
        Console.WriteLine($"{"кава",-10}{49.5,8:F2}");
        Console.WriteLine($"{"чай",-10}{120,8:F2}");

        Console.WriteLine($"ВЕЛИКИ({"мала".ToUpper()}), задом-наперед: {new string("abcde".Reverse().ToArray())}");
        Console.WriteLine($"'{"  пробіли  "}'.Trim() -> '{"  пробіли  ".Trim()}'");
        Console.WriteLine($"StartsWith 'C#': {"C# rules".StartsWith("C#")}");
    }

    // ------------------------------------------------------------------------
    private static void NullSafetyDemo()
    {
        string? maybeName = null;

        // ?? — підстановка за замовчуванням
        string display = maybeName ?? "(без імені)";
        Console.WriteLine(display);

        // ??= — присвоїти, лише якщо зараз null
        maybeName ??= "Присвоєно";
        Console.WriteLine(maybeName);

        // ?. — не впаде на null, поверне null
        string? city = FindCity("Богдан");
        Console.WriteLine($"місто Богдана: {city ?? "?"}, довжина назви: {city?.Length.ToString() ?? "n/a"}");
        Console.WriteLine($"місто 'Ніхто': {FindCity("Ніхто") ?? "не знайдено"}");

        static string? FindCity(string name) =>
            People.FirstOrDefault(p => p.Name == name)?.City;
    }

    // ------------------------------------------------------------------------
    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ".PadRight(70, '='));
    }
}
