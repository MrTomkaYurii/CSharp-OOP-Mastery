using EFCore.AdvancedMapping.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.AdvancedMapping.Data;

public class AdvancedMappingDbContext : DbContext
{
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<CreditCardPayment> CreditCardPayments => Set<CreditCardPayment>();
    public DbSet<CryptoPayment> CryptoPayments => Set<CryptoPayment>();
    public DbSet<BankTransferPayment> BankTransferPayments => Set<BankTransferPayment>();

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CatalogProduct> Products => Set<CatalogProduct>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=advanced_mapping_demo.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. TPH (Table-per-Hierarchy) налаштування для ієрархії Payment
        modelBuilder.Entity<Payment>()
            .ToTable("Payments")
            .HasDiscriminator<string>("PaymentMethodType")
            .HasValue<CreditCardPayment>("CreditCard")
            .HasValue<CryptoPayment>("Crypto")
            .HasValue<BankTransferPayment>("BankTransfer");

        // 2. Owned Entity Types (Value Objects)
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.Property(c => c.FullName).HasMaxLength(120).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(100).IsRequired();

            entity.OwnsOne(c => c.ShippingAddress, addr =>
            {
                addr.Property(a => a.Street).HasColumnName("Shipping_Street").HasMaxLength(150);
                addr.Property(a => a.City).HasColumnName("Shipping_City").HasMaxLength(80);
                addr.Property(a => a.PostalCode).HasColumnName("Shipping_PostalCode").HasMaxLength(20);
                addr.Property(a => a.Country).HasColumnName("Shipping_Country").HasMaxLength(60);
            });
        });

        // 3. JSON Columns (EF Core 8/9/10)
        modelBuilder.Entity<CatalogProduct>(entity =>
        {
            entity.ToTable("CatalogProducts");
            entity.Property(p => p.Sku).HasMaxLength(50).IsRequired();
            entity.Property(p => p.Name).HasMaxLength(150).IsRequired();

            // Зберігання об'єкта як JSON-документа всередині реляційного стовпця
            entity.OwnsOne(p => p.Specifications, spec =>
            {
                spec.ToJson();
            });
        });
    }
}
