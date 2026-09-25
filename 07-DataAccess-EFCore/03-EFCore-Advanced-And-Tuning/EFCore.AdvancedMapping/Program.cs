using System.Text;
using EFCore.AdvancedMapping.Data;
using EFCore.AdvancedMapping.Models;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  EF CORE 10: ПОГЛИБЛЕНЕ МАПУВАННЯ (TPH, OWNED TYPES, JSON COLUMNS)");
Console.WriteLine("=================================================================\n");

using var db = new AdvancedMappingDbContext();
await db.Database.EnsureDeletedAsync();
await db.Database.EnsureCreatedAsync();

// -------------------------------------------------------------
// 1. ДЕМОНСТРАЦІЯ TPH (TABLE-PER-HIERARCHY) УСПАДКУВАННЯ
// -------------------------------------------------------------
Console.WriteLine("--- 1. Поліморфні сутності та стратегія TPH ---");
var card = new CreditCardPayment
{
    Amount = 4500.50m,
    CardMask = "**** **** **** 4812",
    CardHolder = "Олександр Коваленко"
};

var crypto = new CryptoPayment
{
    Amount = 18200.00m,
    CryptoCurrency = "USDT",
    TxHash = "0x7f83b1657ff1fc53b92dc18148a1d65dfc2d4b1fa3d677284addd200126d9069"
};

var bank = new BankTransferPayment
{
    Amount = 98000.00m,
    Iban = "UA213223130000026007233566124",
    SwiftCode = "WACBPLPW"
};

db.Payments.AddRange(card, crypto, bank);
await db.SaveChangesAsync();

Console.WriteLine("Всі платежі (поліморфний запит db.Payments):");
var allPayments = await db.Payments.ToListAsync();
foreach (var p in allPayments)
{
    Console.WriteLine($"  ID: {p.Id}, Сума: {p.Amount} грн, Тип: {p.GetType().Name}");
}

Console.WriteLine("\nЗапит тільки крипто-платежів через db.Payments.OfType<CryptoPayment>():");
var cryptos = await db.Payments.OfType<CryptoPayment>().ToListAsync();
foreach (var c in cryptos)
{
    Console.WriteLine($"  Crypto ID: {c.Id}, Currency: {c.CryptoCurrency}, Tx: {c.TxHash[..16]}...");
}

// -------------------------------------------------------------
// 2. ДЕМОНСТРАЦІЯ OWNED ENTITY TYPES (VALUE OBJECTS)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 2. Вбудовані об'єкти (Owned Types / Value Objects) ---");
var customer = new Customer
{
    FullName = "Марія Шевченко",
    Email = "maria.shevchenko@example.ua",
    ShippingAddress = new Address
    {
        Street = "вул. Хрещатик, 24, кв. 12",
        City = "Київ",
        PostalCode = "01001",
        Country = "Україна"
    }
};

db.Customers.Add(customer);
await db.SaveChangesAsync();

var loadedCustomer = await db.Customers.FirstAsync(c => c.Email == "maria.shevchenko@example.ua");
Console.WriteLine($"Клієнт: {loadedCustomer.FullName}");
Console.WriteLine($"  Адреса доставки (Owned Object): {loadedCustomer.ShippingAddress.Street}, {loadedCustomer.ShippingAddress.City}, {loadedCustomer.ShippingAddress.PostalCode}");

// -------------------------------------------------------------
// 3. ДЕМОНСТРАЦІЯ JSON-СТОВПЦІВ (JSON COLUMNS У .NET 8+)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 3. Робота з JSON-стовпцями (Complex JSON Mapping) ---");
var laptop = new CatalogProduct
{
    Sku = "NB-DELL-XPS15",
    Name = "Dell XPS 15 9530",
    Price = 85999.00m,
    Specifications = new ProductSpecifications
    {
        WeightKg = 1.86,
        Color = "Platinum Silver",
        FeatureTags = ["oled", "thunderbolt4", "intel-core-i9", "rtx4070"]
    }
};

var monitor = new CatalogProduct
{
    Sku = "MN-LG-32UN880",
    Name = "LG UltraFine Ergo 32\"",
    Price = 24500.00m,
    Specifications = new ProductSpecifications
    {
        WeightKg = 6.5,
        Color = "Matte Black",
        FeatureTags = ["ips", "4k", "ergo-arm", "usb-c-hub"]
    }
};

db.Products.AddRange(laptop, monitor);
await db.SaveChangesAsync();

Console.WriteLine("Пошук товарів за властивостями всередині JSON документа:");
var lightProducts = await db.Products
    .Where(p => p.Specifications.WeightKg < 3.0)
    .ToListAsync();

foreach (var p in lightProducts)
{
    Console.WriteLine($"  [Легкий пристрій]: {p.Name} - Вага: {p.Specifications.WeightKg} кг, Колір: {p.Specifications.Color}");
    Console.WriteLine($"    Теги з JSON: {string.Join(", ", p.Specifications.FeatureTags)}");
}

Console.WriteLine("\n=================================================================");
Console.WriteLine("  EFCore.AdvancedMapping успішно завершив виконання!");
Console.WriteLine("=================================================================");
