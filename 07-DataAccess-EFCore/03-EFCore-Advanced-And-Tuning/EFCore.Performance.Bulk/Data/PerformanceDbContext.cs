using EFCore.Performance.Bulk.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Performance.Bulk.Data;

public class PerformanceDbContext : DbContext
{
    private readonly AuditSaveChangesInterceptor _auditInterceptor = new();

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlite("Data Source=performance_bulk_demo.db")
                .AddInterceptors(_auditInterceptor);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderNumber).HasMaxLength(30).IsRequired();
            entity.Property(o => o.CustomerName).HasMaxLength(100).IsRequired();
            entity.Property(o => o.Status).HasMaxLength(30).IsRequired();
            entity.Property(o => o.TotalAmount).HasPrecision(18, 2);

            // Глобальний фільтр запитів для підтримки Soft Delete:
            // Будь-який запит автоматично отримує умову WHERE IsDeleted = 0
            entity.HasQueryFilter(o => !o.IsDeleted);

            entity.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Sku).HasMaxLength(50).IsRequired();
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
        });
    }
}
