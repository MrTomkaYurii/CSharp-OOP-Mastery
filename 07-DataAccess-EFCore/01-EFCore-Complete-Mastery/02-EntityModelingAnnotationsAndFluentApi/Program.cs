using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityModelingAnnotationsAndFluentApi;

// ============================================================================
// ПРОЄКТ 02: Моделювання сутностей: Data Annotations проти Fluent API
// ============================================================================
// Цей приклад демонструє всі способи налаштування моделі даних у EF Core:
// 1. Конвенції (за угодою), Атрибути (Data Annotations) та Fluent API (OnModelCreating).
// 2. Винесення конфігурацій у окремі класи IEntityTypeConfiguration<T>.
// 3. Перейменування таблиць і стовпців (ToTable, HasColumnName), типи стовпців.
// 4. Первинні ключі (HasKey) та складені ключі (Composite Primary Key).
// 5. Альтернативні ключі (HasAlternateKey), Унікальні індекси (HasIndex().IsUnique()).
// 6. Обмеження перевірки БД (HasCheckConstraint), значення за замовчуванням (HasDefaultValue).
// 7. Ігнорування розрахункових властивостей ([NotMapped] та builder.Ignore()).
// 8. Початкове заповнення БД даними (Data Seeding через HasData).
// ============================================================================

/// <summary>
/// Сутність Товару, де базові обмеження задані через Data Annotations,
/// а складніші правила (CheckConstraint, індекси, значення за замовчуванням) — через Fluent API.
/// </summary>
[Table("CatalogProducts")]
public class Product
{
    [Key]
    [Column("ProductId")]
    public int Id { get; set; }

    /// <summary>
    /// Унікальний артикул товару (SKU), який ми зробимо Alternate Key та індексом у Fluent API.
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Розрахункова властивість C#, яка не повинна створювати стовпець у таблиці БД.
    /// </summary>
    [NotMapped]
    public decimal TotalInventoryValue => UnitPrice * StockQuantity;
}

/// <summary>
/// Сутність Запису на складі зі СКЛАДЕНИМ первинним ключем (WarehouseCode + ProductId).
/// Складені ключі у EF Core налаштовуються через Fluent API (або атрибут [PrimaryKey] починаючи з EF Core 7).
/// </summary>
public class WarehouseBinItem
{
    public string WarehouseCode { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string AisleLocation { get; set; } = string.Empty;
    public int BinCapacity { get; set; }

    // Тимчасова діагностична нотатка — проігноруємо її через Fluent API (builder.Ignore)
    public string? TemporaryInspectorNote { get; set; }
}

/// <summary>
/// Виділений клас конфігурації Fluent API для сутності Product (рекомендований патерн для чистих проєктів).
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Налаштування таблиці та SQL-обмежень CHECK
        builder.ToTable("CatalogProducts", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_CatalogProducts_Price_Positive", "UnitPrice >= 0");
            tableBuilder.HasCheckConstraint("CK_CatalogProducts_Stock_NonNegative", "StockQuantity >= 0");
        });

        // Альтернативний ключ (створює UNIQUE constraint і може бути ціллю для Foreign Key)
        builder.HasAlternateKey(p => p.Sku);

        // Складений індекс для швидкого пошуку активних товарів за назвою
        builder.HasIndex(p => new { p.IsActive, p.Name })
            .HasDatabaseName("IX_CatalogProducts_IsActive_Name");

        // Точне налаштування властивостей
        builder.Property(p => p.UnitPrice)
            .HasColumnType("decimal(18, 2)")
            .HasDefaultValue(0.0m);

        builder.Property(p => p.StockQuantity)
            .HasDefaultValue(0);

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        // Початкова ініціалізація даних (Seeding через HasData)
        builder.HasData(
            new Product { Id = 1, Sku = "LAP-PRO-16", Name = "Ноутбук Розробника 16\"", UnitPrice = 62000m, StockQuantity = 8, IsActive = true },
            new Product { Id = 2, Sku = "MON-4K-27", Name = "Монітор 27\" 4K IPS", UnitPrice = 18500m, StockQuantity = 15, IsActive = true },
            new Product { Id = 3, Sku = "KBD-MECH-UA", Name = "Механічна клавіатура з укр. розкладкою", UnitPrice = 4200m, StockQuantity = 30, IsActive = true }
        );
    }
}

/// <summary>
/// Виділений клас конфігурації Fluent API для сутності зі складеним ключем WarehouseBinItem.
/// </summary>
public class WarehouseBinItemConfiguration : IEntityTypeConfiguration<WarehouseBinItem>
{
    public void Configure(EntityTypeBuilder<WarehouseBinItem> builder)
    {
        builder.ToTable("WarehouseBins");

        // Складений первинний ключ (Composite Primary Key) з двох властивостей
        builder.HasKey(w => new { w.WarehouseCode, w.ProductId });

        builder.Property(w => w.WarehouseCode)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(w => w.AisleLocation)
            .HasMaxLength(32)
            .HasDefaultValue("A-01");

        // Ігнорування властивості через Fluent API (аналог [NotMapped], але без засмічення доменного класу)
        builder.Ignore(w => w.TemporaryInspectorNote);

        builder.HasData(
            new WarehouseBinItem { WarehouseCode = "KYIV-01", ProductId = 1, AisleLocation = "RACK-A1", BinCapacity = 25 },
            new WarehouseBinItem { WarehouseCode = "LVIV-02", ProductId = 1, AisleLocation = "RACK-B4", BinCapacity = 15 },
            new WarehouseBinItem { WarehouseCode = "KYIV-01", ProductId = 2, AisleLocation = "RACK-C2", BinCapacity = 40 }
        );
    }
}

public class ModelingDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<WarehouseBinItem> WarehouseBins => Set<WarehouseBinItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=modeling_fluent.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Підключаємо всі класи IEntityTypeConfiguration<T> з поточної збірки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelingDbContext).Assembly);
    }
}

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" 02. Моделювання сутностей: Data Annotations проти Fluent API");
        Console.WriteLine("================================================================================\n");

        await using var context = new ModelingDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // --------------------------------------------------------------------
        // 1. Огляд згенерованої SQL-схеми таблиць (CreateScript)
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 1: Згенерований DDL-скрипт схеми бази даних (GenerateCreateScript) ---");
        string ddlScript = context.Database.GenerateCreateScript();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(ddlScript);
        Console.ResetColor();

        // --------------------------------------------------------------------
        // 2. Перевірка початкових даних (HasData) та ігнорованих властивостей ([NotMapped])
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 2: Початкові дані (HasData) та розрахункова властивість [NotMapped] ---");
        var products = await context.Products.AsNoTracking().ToListAsync();
        foreach (var p in products)
        {
            Console.WriteLine($"   [{p.Id}] SKU: {p.Sku,-12} | {p.Name,-40} | Ціна: {p.UnitPrice,8} грн | Запас: {p.StockQuantity,2} | Загальна вартість ([NotMapped]): {p.TotalInventoryValue,10} грн");
        }

        Console.WriteLine("\n--- КРОК 3: Пошук за складеним первинним ключем (FindAsync(WarehouseCode, ProductId)) ---");
        var bin = await context.WarehouseBins.FindAsync("KYIV-01", 1);
        if (bin is not null)
        {
            Console.WriteLine($"   Знайдено комірку складу: Код={bin.WarehouseCode}, ProductId={bin.ProductId}, Стелаж={bin.AisleLocation}, Місткість={bin.BinCapacity}");
        }

        // --------------------------------------------------------------------
        // 3. Перевірка дії SQL CHECK Constraint (спроба записати від'ємну ціну)
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 4: Перевірка обмеження цілісності CK_CatalogProducts_Price_Positive ---");
        try
        {
            context.Products.Add(new Product
            {
                Sku = "BAD-PRICE-01",
                Name = "Некоректний товар з від'ємною ціною",
                UnitPrice = -100m,
                StockQuantity = 5
            });
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"   [Очікуваний захист БД]: Спроба зберегти від'ємну ціну відхилена СУБД!");
            Console.WriteLine($"   Повідомлення SQLite: {ex.InnerException?.Message ?? ex.Message}");
            Console.ResetColor();
        }
    }
}
