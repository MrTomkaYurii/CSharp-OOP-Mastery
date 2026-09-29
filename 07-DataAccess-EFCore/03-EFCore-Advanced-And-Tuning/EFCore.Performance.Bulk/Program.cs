using System.Text;
using EFCore.Performance.Bulk.Data;
using EFCore.Performance.Bulk.Models;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  EF CORE 10: ШВИДКОДІЯ, МАСОВІ ОПЕРАЦІЇ ТА ОПТИМІЗАЦІЯ ЗАПИТІВ");
Console.WriteLine("=================================================================\n");

using var db = new PerformanceDbContext();
await db.Database.EnsureDeletedAsync();
await db.Database.EnsureCreatedAsync();

// -------------------------------------------------------------
// 1. АУДИТОРСЬКИЙ ІНТЕРЦЕПТОР (SaveChangesInterceptor)
// -------------------------------------------------------------
Console.WriteLine("--- 1. Демонстрація SaveChangesInterceptor (CreatedAt / UpdatedAt) ---");
var order1 = new Order
{
    OrderNumber = "ORD-2026-001",
    CustomerName = "Дмитро Мельник",
    TotalAmount = 3500.00m,
    Status = "Pending",
    Items =
    [
        new OrderItem { Sku = "KB-01", Quantity = 1, UnitPrice = 1500.00m },
        new OrderItem { Sku = "MS-02", Quantity = 2, UnitPrice = 1000.00m }
    ]
};

var order2 = new Order
{
    OrderNumber = "ORD-2026-002",
    CustomerName = "Юлія Бондаренко",
    TotalAmount = 8900.00m,
    Status = "Pending",
    Items =
    [
        new OrderItem { Sku = "MN-4K", Quantity = 1, UnitPrice = 8900.00m }
    ]
};

var order3 = new Order
{
    OrderNumber = "ORD-2026-003",
    CustomerName = "Сергій Кравчук",
    TotalAmount = 1200.00m,
    Status = "Cancelled"
};

db.Orders.AddRange(order1, order2, order3);
await db.SaveChangesAsync();

Console.WriteLine($"Замовлення збережено. Автоматично заповнені поля аудиту через інтерцептор:");
Console.WriteLine($"  [{order1.OrderNumber}] Створено: {order1.CreatedAt:yyyy-MM-dd HH:mm:ss.fff} UTC | Оновлено: {order1.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture) ?? "null"}");

// -------------------------------------------------------------
// 2. МАСОВЕ ОНОВЛЕННЯ БЕЗ ЗАВАНТАЖЕННЯ В ПАМ'ЯТЬ (ExecuteUpdateAsync)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 2. Масове оновлення через ExecuteUpdateAsync (Прямий SQL UPDATE) ---");

// Оновлюємо всі замовлення зі статусом 'Pending' одразу в базі даних без Change Tracker
int updatedCount = await db.Orders
    .Where(o => o.Status == "Pending")
    .ExecuteUpdateAsync(setter => setter
        .SetProperty(o => o.Status, "Processing")
        .SetProperty(o => o.UpdatedAt, DateTime.UtcNow));

Console.WriteLine($"Оновлено рядків у БД за один запит ExecuteUpdateAsync: {updatedCount}");

// -------------------------------------------------------------
// 3. GLOBAL QUERY FILTERS ТА SOFT DELETE
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 3. Глобальні фільтри запитів (Global Query Filters & Soft Delete) ---");

// Виконуємо Soft Delete для order1
order1.IsDeleted = true;
order1.DeletedAt = DateTime.UtcNow;
await db.SaveChangesAsync();

int visibleCount = await db.Orders.CountAsync();
int totalIncludingDeleted = await db.Orders.IgnoreQueryFilters().CountAsync();

Console.WriteLine($"Кількість видимих замовлень (HasQueryFilter: IsDeleted == false): {visibleCount}");
Console.WriteLine($"Кількість замовлень з IgnoreQueryFilters() (включно з видаленими):  {totalIncludingDeleted}");

// -------------------------------------------------------------
// 4. ОПТИМІЗАЦІЯ ЗАПИТІВ (AsNoTracking, AsSplitQuery, Compiled Query)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 4. Оптимізація вибірки (AsNoTracking, AsSplitQuery, Compiled Query) ---");

// AsNoTrackingWithIdentityResolution для Read-only запитів зі зв'язками
var readOnlyOrders = await db.Orders
    .AsNoTrackingWithIdentityResolution()
    .Include(o => o.Items)
    .AsSplitQuery() // Запобігає декартовому добутку (Cartesian Explosion) при зв'язках один-до-багатьох
    .ToListAsync();

Console.WriteLine($"Вичитано {readOnlyOrders.Count} замовлень за допомогою AsSplitQuery та AsNoTracking.");

// Компілювання запиту (Compiled Query) для критичних затримок
var getOrderByNumberCompiled = EF.CompileAsyncQuery(
    (PerformanceDbContext context, string orderNum) =>
        context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderNumber == orderNum));

var foundOrder = await getOrderByNumberCompiled(db, "ORD-2026-002");
Console.WriteLine($"Результат Compiled Query для 'ORD-2026-002': {foundOrder?.CustomerName}, Сума: {foundOrder?.TotalAmount} грн");

// -------------------------------------------------------------
// 5. МАСОВЕ ВИДАЛЕННЯ БЕЗ ЗАВАНТАЖЕННЯ В ПАМ'ЯТЬ (ExecuteDeleteAsync)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 5. Масове видалення через ExecuteDeleteAsync (Прямий SQL DELETE) ---");

int deletedCount = await db.Orders
    .IgnoreQueryFilters()
    .Where(o => o.Status == "Cancelled")
    .ExecuteDeleteAsync();

Console.WriteLine($"Видалено скасованих замовлень одним запитом ExecuteDeleteAsync: {deletedCount}");

Console.WriteLine("\n=================================================================");
Console.WriteLine("  EFCore.Performance.Bulk успішно завершив виконання!");
Console.WriteLine("=================================================================");
