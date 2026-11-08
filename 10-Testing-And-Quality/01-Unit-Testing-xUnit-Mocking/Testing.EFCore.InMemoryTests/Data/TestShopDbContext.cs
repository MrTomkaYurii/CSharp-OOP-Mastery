using Microsoft.EntityFrameworkCore;
using Testing.Core.Domain.Data;

namespace Testing.EFCore.InMemoryTests.Data;

public class TestShopDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public TestShopDbContext(DbContextOptions<TestShopDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(120).IsRequired();
            entity.HasIndex(c => c.Email).IsUnique();
        });
    }
}
