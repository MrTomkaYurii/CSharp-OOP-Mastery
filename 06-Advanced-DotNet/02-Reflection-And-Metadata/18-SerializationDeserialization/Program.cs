using SerializationDeserialization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація рушія серіалізації/десеріалізації на базі рефлексії ===\n");

// 1. Початковий об'єкт
var originalUser = new User
{
    Name = "Тарас Шевченко",
    Age = 47,
    Rating = 9.85,
    IsActive = true,
    SecretApiKey = "TOP-SECRET-NEVER-SERIALIZE"
};

Console.WriteLine("--- Оригінальний об'єкт у пам'яті ---");
Console.WriteLine($"Name: {originalUser.Name}, Age: {originalUser.Age}, Rating: {originalUser.Rating}, Secret: {originalUser.SecretApiKey}");

// 2. Серіалізація в XML через рефлексію
Console.WriteLine("\n--- Результат серіалізації в XML (з урахуванням [XmlIgnoreProperty] та [XmlElementName]) ---");
string xml = XmlConverter.Serialize(originalUser);
Console.WriteLine(xml);

// 3. Десеріалізація назад в об'єкт User
Console.WriteLine("--- Десеріалізація XML назад у новий екземпляр User ---");
User restoredUser = XmlConverter.Deserialize<User>(xml);
Console.WriteLine($"Restored Name:   {restoredUser.Name}");
Console.WriteLine($"Restored Age:    {restoredUser.Age}");
Console.WriteLine($"Restored Rating: {restoredUser.Rating}");
Console.WriteLine($"Restored Active: {restoredUser.IsActive}");
Console.WriteLine($"Secret (було проігноровано): {restoredUser.SecretApiKey}");
