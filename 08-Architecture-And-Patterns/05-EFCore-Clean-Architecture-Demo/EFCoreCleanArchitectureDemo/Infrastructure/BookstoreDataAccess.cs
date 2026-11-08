using EFCoreCleanArchitectureDemo.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCleanArchitectureDemo.Infrastructure;

/// <summary>
/// Контекст бази даних Entity Framework Core (SQLite In-Memory).
/// Відповідає за відображення класів Author і Book на реляційні таблиці.
/// </summary>
public sealed class BookstoreDbContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();

    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FullName).IsRequired().HasMaxLength(120);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.HasOne(b => b.Author)
                  .WithMany(a => a.Books)
                  .HasForeignKey(b => b.AuthorId);
        });
    }
}

/// <summary>
/// SOLID - DIP (Dependency Inversion Principle): Абстракція репозиторію.
/// Бізнес-логіка залежить від цього інтерфейсу, а не від BookstoreDbContext напряму.
/// </summary>
public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAllWithAuthorsAsync();
    Task SeedInitialDataIfEmptyAsync();
}

/// <summary>
/// SOLID - SRP (Single Responsibility Principle): Реалізація репозиторію на базі EF Core.
/// Єдина відповідальність цього класу — виконання SQL-запитів через BookstoreDbContext.
/// </summary>
public sealed class EfCoreBookRepository : IBookRepository
{
    private readonly BookstoreDbContext _dbContext;

    public EfCoreBookRepository(BookstoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Book>> GetAllWithAuthorsAsync()
    {
        // AsNoTracking() прискорює читання, а Include() завантажує пов'язаного автора (JOIN)
        return await _dbContext.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .OrderBy(b => b.Id)
            .ToListAsync();
    }

    public async Task SeedInitialDataIfEmptyAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
        if (await _dbContext.Books.AnyAsync())
        {
            return;
        }

        var martin = new Author { FullName = "Роберт Мартін (Uncle Bob)", Country = "USA" };
        var richter = new Author { FullName = "Джеффрі Ріхтер", Country = "USA" };
        var fowler = new Author { FullName = "Мартін Фаулер", Country = "UK" };

        _dbContext.Books.AddRange(
            new Book { Title = "Clean Architecture", Category = "Architecture", BasePrice = 1200m, PublishedYear = 2017, Author = martin },
            new Book { Title = "CLR via C#", Category = ".NET", BasePrice = 1500m, PublishedYear = 2012, Author = richter },
            new Book { Title = "Patterns of Enterprise Application Architecture", Category = "Architecture", BasePrice = 1400m, PublishedYear = 2002, Author = fowler },
            new Book { Title = "Refactoring (2nd Edition)", Category = "Engineering", BasePrice = 1100m, PublishedYear = 2018, Author = fowler }
        );

        await _dbContext.SaveChangesAsync();
    }
}
