using JsonDeserializationDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 23. ДЕСЕРІАЛІЗАЦІЯ З JSON ЧЕРЕЗ РЕФЛЕКСІЮ (ВЛАСНИЙ РУШІЙ)");
Console.WriteLine(" Як Activator.CreateInstance та PropertyInfo.SetValue відновлюють об'єкт");
Console.WriteLine("==========================================================================\n");

// Вхідний JSON-рядок (наприклад, отриманий по мережі від клієнтського застосунку).
// Зверніть увагу: в JSON також передано невідоме поле "client_browser"
// та спробу підмінити захищене поле "IsAdminVerified": true.
string incomingJson = """
{
  "order_id": 9045,
  "customer_name": "Віктор Литвиненко",
  "total_amount": 3499.50,
  "is_express_delivery": true,
  "items": ["Монітор 27\"", "Механічна клавіатура", "Кабель DisplayPort"],
  "client_browser": "Firefox 132.0",
  "IsAdminVerified": true
}
""";

Console.WriteLine("--- Крок 1: Вхідний сирий JSON-рядок ---");
Console.WriteLine(incomingJson);

Console.WriteLine("\n--- Крок 2: Покрокова траса роботи рефлексивного JSON-десеріалізатора ---");
CustomerOrder order = ReflectionJsonDeserializer.Deserialize<CustomerOrder>(incomingJson, traceSteps: true);

Console.WriteLine("\n--- Крок 3: Перевірка відновленого C#-об'єкта CustomerOrder у пам'яті ---");
Console.WriteLine($"  OrderId:           {order.OrderId}");
Console.WriteLine($"  CustomerName:      {order.CustomerName}");
Console.WriteLine($"  TotalAmount:       {order.TotalAmount}");
Console.WriteLine($"  IsExpressDelivery: {order.IsExpressDelivery}");
Console.WriteLine($"  Items ({order.Items.Length}):         {string.Join(", ", order.Items)}");
Console.WriteLine($"  IsAdminVerified:   {order.IsAdminVerified} (залишилося false завдяки [JsonExclude])");
