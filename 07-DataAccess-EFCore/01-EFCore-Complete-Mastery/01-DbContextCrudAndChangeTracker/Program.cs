using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DbContextCrudAndChangeTracker;

// ============================================================================
// ПРОЄКТ 01: Основи DbContext, CRUD, Логування SQL та ChangeTracker
// ============================================================================
// Цей приклад охоплює базові розділи Metanit по EF Core:
// 1. Роль DbContext та DbSet<T>, життєвий цикл контексту даних.
// 2. Управління БД: EnsureDeletedAsync() та EnsureCreatedAsync().
// 3. Логування згенерованих SQL-команд через LogTo() та EnableSensitiveDataLogging().
// 4. Повний цикл CRUD: Create (Add/AddRange), Read (ToListAsync/FindAsync), Update, Delete (Remove).
// 5. Глибоке дослідження ChangeTracker: стани EntityState (Detached, Added, Unchanged, Modified, Deleted),
//    перегляд OriginalValue та CurrentValue окремих властивостей сутності через Entry().
// ============================================================================

/// <summary>
/// Сутність книги у каталозі бібліотеки.
/// За конвенцією EF Core властивість Id автоматично стає первинним ключем (PRIMARY KEY AUTOINCREMENT).
/// </summary>
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int PublishedYear { get; set; }

    public override string ToString() =>
        $"[{Id}] «{Title}» — {Author} ({PublishedYear} р.), Ціна: {Price:C}";
}

/// <summary>
/// Контекст бази даних — головний клас EF Core, який реалізує патерни
/// Unit of Work (одиниця роботи) та Repository (через властивості DbSet).
/// </summary>
public class LibraryDbContext : DbContext
{
    private readonly Action<string>? _sqlLogger;

    public DbSet<Book> Books => Set<Book>();

    public LibraryDbContext(Action<string>? sqlLogger = null)
    {
        _sqlLogger = sqlLogger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Підключення до локального файлу SQLite
        optionsBuilder.UseSqlite("Data Source=library_basics.db");

        // Якщо передано делегат логування — перехоплюємо всі SQL-команди EF Core
        if (_sqlLogger is not null)
        {
            optionsBuilder
                .LogTo(_sqlLogger, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging(); // Дозволяє бачити конкретні значення параметрів (@p0, @p1) у логах
        }
    }
}

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" 01. Основи DbContext, CRUD-операції, Логування SQL та механізм ChangeTracker");
        Console.WriteLine("================================================================================\n");

        // Список для збереження останніх виконаних SQL-команд
        var executedSqlCommands = new List<string>();
        void LogSql(string logMessage)
        {
            executedSqlCommands.Add(logMessage);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"   [SQL LOG]: {logMessage.Replace(Environment.NewLine, " ")}");
            Console.ResetColor();
        }

        // --------------------------------------------------------------------
        // 1. Ініціалізація бази даних (EnsureDeleted + EnsureCreated)
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 1: Перестворення локальної бази даних SQLite ---");
        await using (var initContext = new LibraryDbContext(LogSql))
        {
            await initContext.Database.EnsureDeletedAsync();
            bool created = await initContext.Database.EnsureCreatedAsync();
            Console.WriteLine($"Базу даних створено з нуля: {created}\n");
        }

        // --------------------------------------------------------------------
        // 2. Додавання даних (CREATE) та відстеження станів EntityState
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 2: Створення сутностей та перехід станів Detached -> Added -> Unchanged ---");
        await using (var context = new LibraryDbContext(LogSql))
        {
            var kobzar = new Book
            {
                Title = "Кобзар",
                Author = "Тарас Шевченко",
                Price = 450.00m,
                PublishedYear = 1840
            };

            // До виклику Add об'єкт не відомий контексту -> стан Detached
            PrintEntityState(context, kobzar, "Щойно створений об'єкт C# (до context.Books.Add)");

            // Реєструємо об'єкт у контексті для вставки
            context.Books.Add(kobzar);
            PrintEntityState(context, kobzar, "Після виклику context.Books.Add(kobzar)");

            // Додамо ще дві книги пакетно через AddRange
            var additionalBooks = new[]
            {
                new Book { Title = "Лісова пісня", Author = "Леся Українка", Price = 320.00m, PublishedYear = 1911 },
                new Book { Title = "Захар Беркут", Author = "Іван Франко", Price = 290.00m, PublishedYear = 1883 }
            };
            context.Books.AddRange(additionalBooks);

            // Зберігаємо всі зміни однією транзакцією
            int affectedRows = await context.SaveChangesAsync();
            Console.WriteLine($"Виконано SaveChangesAsync(). Записано рядків у БД: {affectedRows}");

            // Після збереження EF Core автоматично підставив згенерований БД Id і перевів стан у Unchanged
            PrintEntityState(context, kobzar, $"Після SaveChangesAsync() (присвоєно Id={kobzar.Id})");
            Console.WriteLine();
        }

        // --------------------------------------------------------------------
        // 3. Читання (READ), FindAsync (кеш контексту) та Модифікація (UPDATE)
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 3: Читання, кешування First-Level Cache (FindAsync) та модифікація (Modified) ---");
        await using (var context = new LibraryDbContext(LogSql))
        {
            // Завантажуємо книгу з БД за первинним ключем
            var book = await context.Books.FindAsync(1);
            if (book is not null)
            {
                Console.WriteLine($"Знайдено книгу в БД: {book}");

                // Повторний виклик FindAsync(1) у тому ж контексті НЕ виконує SQL-запит!
                // Він миттєво повертає вже відстежуваний екземпляр з пам'яті ChangeTracker (Identity Map).
                int sqlCountBeforeSecondFind = executedSqlCommands.Count;
                var sameBookRef = await context.Books.FindAsync(1);
                int sqlCountAfterSecondFind = executedSqlCommands.Count;

                Console.WriteLine($"Повторний FindAsync(1): чи той самий об'єкт у пам'яті? {ReferenceEquals(book, sameBookRef)}");
                Console.WriteLine($"Кількість виконаних SQL-запитів під час повторного FindAsync(1): {sqlCountAfterSecondFind - sqlCountBeforeSecondFind}");

                // Змінюємо лише одну властивість — Price
                book.Price = 525.00m;

                // Досліджуємо Entry(book): які властивості змінені, їхні OriginalValue та CurrentValue
                var entry = context.Entry(book);
                PrintEntityState(context, book, "Після зміни властивості book.Price = 525.00m");

                foreach (var prop in entry.Properties)
                {
                    Console.WriteLine(
                        $"   Властивість {prop.Metadata.Name,-15} | IsModified: {prop.IsModified,-5} | " +
                        $"OriginalValue: {prop.OriginalValue,-15} | CurrentValue: {prop.CurrentValue}");
                }

                // EF Core згенерує UPDATE лише для зміненого стовпця Price!
                await context.SaveChangesAsync();
                Console.WriteLine("Зміни збережено у БД.\n");
            }
        }

        // --------------------------------------------------------------------
        // 4. Видалення сутності (DELETE) та огляд ChangeTracker.DebugView
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 4: Видалення сутності (Deleted) та інспекція ChangeTracker.DebugView ---");
        await using (var context = new LibraryDbContext(LogSql))
        {
            var allBooks = await context.Books.ToListAsync();
            Console.WriteLine($"Кількість книг у каталозі перед видаленням: {allBooks.Count}");

            var bookToRemove = allBooks.Last();
            context.Books.Remove(bookToRemove);

            PrintEntityState(context, bookToRemove, $"Після context.Books.Remove(«{bookToRemove.Title}»)");

            Console.WriteLine("\nПовний знімок ChangeTracker.DebugView.ShortView перед SaveChangesAsync():");
            Console.WriteLine(context.ChangeTracker.DebugView.ShortView);

            await context.SaveChangesAsync();

            var remainingBooks = await context.Books.AsNoTracking().ToListAsync();
            Console.WriteLine($"Книги у каталозі після видалення ({remainingBooks.Count} шт.):");
            foreach (var b in remainingBooks)
            {
                Console.WriteLine($"   * {b}");
            }
        }
    }

    private static void PrintEntityState(DbContext context, Book book, string stageDescription)
    {
        EntityState state = context.Entry(book).State;
        Console.WriteLine($"-> {stageDescription}: State = [{state}]");
    }
}
