using EFCore.Seeding.Bogus.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Seeding.Bogus.Data;

public class BookstoreDbContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();

    public BookstoreDbContext() { }

    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=bookstore_bogus_seeding.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FirstName).HasMaxLength(60).IsRequired();
            entity.Property(a => a.LastName).HasMaxLength(60).IsRequired();
            entity.Property(a => a.Email).HasMaxLength(100).IsRequired();
            entity.HasIndex(a => a.Email).IsUnique();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).HasMaxLength(200).IsRequired();
            entity.Property(b => b.Isbn).HasMaxLength(20).IsRequired();
            entity.Property(b => b.Price).HasPrecision(18, 2);

            entity.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.ReviewerName).HasMaxLength(80).IsRequired();
            entity.Property(r => r.Comment).HasMaxLength(1000);

            entity.HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
