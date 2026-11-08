using Microsoft.EntityFrameworkCore;
using StoreApp.DataAccess.Entities;

namespace StoreApp.DataAccess.Context;

/// <summary>
/// Контекст бази даних SQLite для тришарового застосунку магазину.
/// Налаштовує схему таблиць, індекси та зовнішні ключі за допомогою Fluent API.
/// </summary>
public sealed class StoreDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CustomerOrder> CustomerOrders => Set<CustomerOrder>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Конфігурація Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).HasMaxLength(300);
            entity.HasIndex(c => c.Name).IsUnique();
        });

        // 2. Конфігурація Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Sku).IsRequired().HasMaxLength(32);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(180);
            entity.Property(p => p.UnitPrice).HasPrecision(18, 2);

            entity.HasIndex(p => p.Sku).IsUnique();

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 3. Конфігурація CustomerOrder
        modelBuilder.Entity<CustomerOrder>(entity =>
        {
            entity.ToTable("CustomerOrders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(40);
            entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(150);
            entity.Property(o => o.SubtotalAmount).HasPrecision(18, 2);
            entity.Property(o => o.DiscountAmount).HasPrecision(18, 2);
            entity.Property(o => o.FinalAmount).HasPrecision(18, 2);

            entity.HasIndex(o => o.OrderNumber).IsUnique();
        });

        // 4. Конфігурація OrderLine
        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.ToTable("OrderLines");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.UnitPriceAtPurchase).HasPrecision(18, 2);

            entity.HasOne(l => l.CustomerOrder)
                .WithMany(o => o.Lines)
                .HasForeignKey(l => l.CustomerOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.Product)
                .WithMany(p => p.OrderLines)
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

/// <summary>
/// Початковий ініціалізатор бази даних SQLite з реалістичним наповненням каталогу та історії замовлень.
/// </summary>
public static class StoreDatabaseInitializer
{
    public static async Task InitializeAndSeedAsync(StoreDbContext context, CancellationToken ct = default)
    {
        await context.Database.EnsureDeletedAsync(ct);
        await context.Database.EnsureCreatedAsync(ct);

        if (await context.Categories.AnyAsync(ct))
        {
            return;
        }

        var laptops = new Category
        {
            Name = "Ноутбуки та Робочі станції",
            Description = "Професійні мобільні системи для розробників та інженерів"
        };
        var components = new Category
        {
            Name = "Комплектуючі та Накопичувачі",
            Description = "Процесори, оперативна пам'ять та швидкісні NVMe SSD"
        };
        var peripherals = new Category
        {
            Name = "Монітори та Периферія",
            Description = "Механічні клавіатури, 4K-дисплеї та ергономічні маніпулятори"
        };

        context.Categories.AddRange(laptops, components, peripherals);
        await context.SaveChangesAsync(ct);

        var p1 = new Product
        {
            Sku = "LAP-PRO-16",
            Name = "Ноутбук DevBook Pro 16 (32GB / 1TB NVMe)",
            CategoryId = laptops.Id,
            UnitPrice = 64500m,
            StockQuantity = 8,
            ReorderLevel = 3
        };
        var p2 = new Product
        {
            Sku = "LAP-AIR-14",
            Name = "Ультрабук CloudBook Air 14 (16GB / 512GB)",
            CategoryId = laptops.Id,
            UnitPrice = 42000m,
            StockQuantity = 12,
            ReorderLevel = 4
        };
        var p3 = new Product
        {
            Sku = "SSD-NVME-2TB",
            Name = "Накопичувач TurboNVMe Gen4 2TB",
            CategoryId = components.Id,
            UnitPrice = 6800m,
            StockQuantity = 25,
            ReorderLevel = 8
        };
        var p4 = new Product
        {
            Sku = "RAM-DDR5-64",
            Name = "Комплект пам'яті DDR5-6000 64GB (2x32GB)",
            CategoryId = components.Id,
            UnitPrice = 9400m,
            StockQuantity = 4, // Низький залишок (менше за ReorderLevel = 6)
            ReorderLevel = 6
        };
        var p5 = new Product
        {
            Sku = "MON-4K-27",
            Name = "Монітор UltraSharp 27\" 4K IPS USB-C",
            CategoryId = peripherals.Id,
            UnitPrice = 21500m,
            StockQuantity = 10,
            ReorderLevel = 4
        };
        var p6 = new Product
        {
            Sku = "KBD-MECH-UA",
            Name = "Механічна клавіатура Tactile Pro (UA/EN)",
            CategoryId = peripherals.Id,
            UnitPrice = 4900m,
            StockQuantity = 3, // Низький залишок (менше за ReorderLevel = 5)
            ReorderLevel = 5
        };

        context.Products.AddRange(p1, p2, p3, p4, p5, p6);
        await context.SaveChangesAsync(ct);

        // Додаємо одне стартове історичне замовлення для початкової статистики
        var seedOrder = new CustomerOrder
        {
            OrderNumber = "ORD-2026-0001",
            CustomerName = "Іван Франко (ІТ-Лабораторія)",
            CreatedAtUtc = DateTime.UtcNow.AddDays(-2),
            SubtotalAmount = 71300m,
            DiscountAmount = 7130m,
            FinalAmount = 64170m,
            Lines = new List<OrderLine>
            {
                new() { ProductId = p1.Id, Quantity = 1, UnitPriceAtPurchase = p1.UnitPrice },
                new() { ProductId = p3.Id, Quantity = 1, UnitPriceAtPurchase = p3.UnitPrice }
            }
        };

        context.CustomerOrders.Add(seedOrder);
        await context.SaveChangesAsync(ct);
    }
}
