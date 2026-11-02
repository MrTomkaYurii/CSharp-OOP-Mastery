using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using SerializationBasicsJsonDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 19. СЕРІАЛІЗАЦІЯ ТА ДЕСЕРІАЛІЗАЦІЯ: БАЗОВЕ РОЗУМІННЯ ФОРМАТУ JSON");
Console.WriteLine(" Структура JSON-документа, типи даних та роль атрибутів");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Що таке серіалізація та анатомія формату JSON
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Для чого потрібна серіалізація та що таке JSON ---");
Console.WriteLine("1. Об'єкт C# живе в оперативній пам'яті (RAM) у вигляді адрес і вказівників.");
Console.WriteLine("2. Щоб передати об'єкт через Інтернет або зберегти у файл, його перетворюють у текст — це СЕРІАЛІЗАЦІЯ.");
Console.WriteLine("3. Зворотне перетворення тексту у живий об'єкт C# у пам'яті — це ДЕСЕРІАЛІЗАЦІЯ.");
Console.WriteLine("4. JSON (JavaScript Object Notation) — найпопулярніший легковаговий текстовий формат типу 'ключ: значення'.\n");

// -----------------------------------------------------------------------
// Крок 2: Створення об'єкта C# та перетворення у JSON
// -----------------------------------------------------------------------
var student = new StudentCard
{
    Id = 1024,
    FullName = "Ірина Мельник",
    AverageScore = 94.5,
    HasScholarship = true,
    Courses = new List<string> { "ООП на C#", "Алгоритми", "Бази даних" },
    TemporaryPinCode = "9988-SECRET"
};

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
};

string jsonText = JsonSerializer.Serialize(student, options);

Console.WriteLine("--- Крок 2: Як об'єкт C# виглядає у форматі JSON ---");
Console.WriteLine(jsonText);
Console.WriteLine("\nЗверніть увагу:");
Console.WriteLine("  - Фігурні дужки { } позначають об'єкт.");
Console.WriteLine("  - Квадратні дужки [ ] позначають масив/колекцію.");
Console.WriteLine("  - Атрибут [JsonPropertyName] змінив назви властивостей (наприклад, FullName -> \"full_name\").");
Console.WriteLine("  - Атрибут [JsonIgnore] приховав секретну властивість TemporaryPinCode.\n");

// -----------------------------------------------------------------------
// Крок 3: Розбір анатомії JSON-вузлів (JsonDocument)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 3: Дослідження внутрішньої структури JSON-документа (JsonElement) ---");
using (JsonDocument doc = JsonDocument.Parse(jsonText))
{
    JsonElement root = doc.RootElement;
    Console.WriteLine($"Кореневий вузол має тип: {root.ValueKind}");

    foreach (JsonProperty prop in root.EnumerateObject())
    {
        Console.WriteLine($"  Ключ JSON: \"{prop.Name}\",-25 | Тип значення: {prop.Value.ValueKind,-10} | Вміст: {prop.Value}");
    }
}
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 4: Десеріалізація JSON-рядка назад в об'єкт C#
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 4: Десеріалізація з тексту JSON назад у екземпляр класу StudentCard ---");
StudentCard? restored = JsonSerializer.Deserialize<StudentCard>(jsonText, options);

if (restored != null)
{
    Console.WriteLine($"  Відновлено Id:           {restored.Id}");
    Console.WriteLine($"  Відновлено FullName:     {restored.FullName}");
    Console.WriteLine($"  Відновлено AverageScore: {restored.AverageScore}");
    Console.WriteLine($"  Відновлено Курси:        {string.Join(", ", restored.Courses)}");
    Console.WriteLine($"  TemporaryPinCode:        '{restored.TemporaryPinCode}' (порожньо, бо поле ігнорувалося)");
}
