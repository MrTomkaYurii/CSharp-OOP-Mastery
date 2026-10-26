using CreateCustomAttributes;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація користувацьких атрибутів та рефлексивної валідації ===\n");

// 1. Тест невалідного користувача (неповнолітній, коротке ім'я, відсутній email)
var invalidUser = new User
{
    Name = "A",
    Age = 16,
    Email = ""
};

Console.WriteLine("--- Перевірка невалідного об'єкта ---");
if (!MinValueValidator.Validate(invalidUser, out List<string> errors))
{
    Console.WriteLine("❌ Валідація не пройдена! Виявлено помилки:");
    foreach (string error in errors)
    {
        Console.WriteLine($"  - {error}");
    }
}

// 2. Тест валідного користувача
var validUser = new User
{
    Name = "Олександр",
    Age = 25,
    Email = "alex@example.com"
};

Console.WriteLine("\n--- Перевірка валідного об'єкта ---");
if (MinValueValidator.Validate(validUser, out List<string> validErrors))
{
    Console.WriteLine("✅ Об'єкт успішно пройшов усі валідаційні перевірки!");
}

// 3. Інспекція метаданих атрибутів без інстанціювання (CustomAttributeData)
MinValueValidator.InspectRawAttributeData(typeof(User));
