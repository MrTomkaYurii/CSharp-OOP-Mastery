using JsonSerializationDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 21. СЕРІАЛІЗАЦІЯ В JSON ЧЕРЕЗ РЕФЛЕКСІЮ (ВЛАСНИЙ РУШІЙ)");
Console.WriteLine(" Як Type.GetProperties() та PropertyInfo.GetValue() будують JSON-рядок");
Console.WriteLine("==========================================================================\n");

// 1. Створюємо об'єкт у пам'яті
var laptop = new ProductItem
{
    Id = 501,
    Name = "Ноутбук ProBook 16\"",
    Price = 1249.99,
    IsAvailable = true,
    Tags = new[] { "Електроніка", "Ноутбуки", "Топ продажів" },
    WholesaleSupplierCost = 890.00m // Це поле позначене [JsonExclude]
};

Console.WriteLine("--- Крок 1: Покрокова траса роботи рефлексивного JSON-серіалізатора ---");
string jsonResult = ReflectionJsonSerializer.Serialize(laptop, traceSteps: true);

Console.WriteLine("\n--- Крок 2: Згенерований JSON-документ ---");
Console.WriteLine(jsonResult);

// 2. Перевірка універсальності: той самий серіалізатор працює для будь-якого іншого класу
Console.WriteLine("\n--- Крок 3: Перевірка універсальності на анонімному об'єкті ---");
var orderSummary = new
{
    OrderNumber = "ORD-2026-99",
    ItemsCount = 3,
    IsPaid = false
};

Console.WriteLine(ReflectionJsonSerializer.Serialize(orderSummary, traceSteps: false));
