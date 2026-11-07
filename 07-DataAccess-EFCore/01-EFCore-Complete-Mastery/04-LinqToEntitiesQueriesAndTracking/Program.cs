using System.Text;
using Microsoft.EntityFrameworkCore;

namespace LinqToEntitiesQueriesAndTracking;

// ============================================================================
// ПРОЄКТ 04: Глибокий LINQ to Entities: Відстеження, Проєкції, Групування та Join
// ============================================================================
// Цей приклад розкриває механізм перетворення виразів IQueryable<T> у SQL-запити:
// 1. Різниця між IEnumerable<T> (виконання в пам'яті клієнта) та IQueryable<T> (трансляція дерева виразів у SQL).
// 2. Фільтрація (Where, EF.Functions.Like), Сортування (OrderBy, ThenByDescending), Пагінація (Skip, Take).
// 3. Проєкція даних у легковагові DTO / рекорди через Select (завантаження лише потрібних стовпців).
// 4. Групування (GroupBy) з агрегатними функціями БД (Count, Sum, Average, Max).
// 5. Явні з'єднання таблиць (Inner Join, Left Join у EF Core 10) та підзапити (Any, All).
// 6. Порівняння режимів відстеження при читанні:
//    - Tracking (за замовчуванням)
//    - AsNoTracking() (максимальна швидкодія для читання, без Identity Resolution)
//    - AsNoTrackingWithIdentityResolution() (без ChangeTracker, але зі збереженням єдиного екземпляра на ключ).
// ============================================================================

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Course> Courses { get; set; } = new();
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationHours { get; set; }
    public double Rating { get; set; }
    public int StudentsEnrolled { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

/// <summary>
/// Легковаговий DTO (Data Transfer Object) для проєкції Select.
/// </summary>
public record CourseSummaryDto(string CourseTitle, string CategoryName, decimal Price, decimal PricePerHour);

public class AcademyQueriesDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=academy_linq.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .Property(c => c.Price)
            .HasColumnType("decimal(18,2)");
    }
}

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" 04. Глибокий LINQ to Entities: Відстеження, Проєкції, Групування та Join");
        Console.WriteLine("================================================================================\n");

        await SeedAcademyAsync();

        await using var context = new AcademyQueriesDbContext();

        // --------------------------------------------------------------------
        // 1. Фільтрація (EF.Functions.Like), Сортування та Пагінація + ToQueryString()
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 1: Фільтрація (Where + EF.Functions.Like), OrderBy та Пагінація (Skip/Take) ---");
        IQueryable<Course> query = context.Courses
            .AsNoTracking()
            .Where(c => c.Rating >= 4.7 && EF.Functions.Like(c.Title, "%C#%"))
            .OrderByDescending(c => c.Rating)
            .ThenBy(c => c.Price)
            .Skip(0)
            .Take(5);

        // ToQueryString() дозволяє побачити точний SQL-запит, який EF Core відправить у СУБД
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("Згенерований SQL:\n" + query.ToQueryString());
        Console.ResetColor();

        var topCSharpCourses = await query.ToListAsync();
        foreach (var c in topCSharpCourses)
        {
            Console.WriteLine($"   * {c.Title} | Рейтинг: {c.Rating:F1} | Ціна: {c.Price} грн | Студентів: {c.StudentsEnrolled}");
        }

        // --------------------------------------------------------------------
        // 2. Проєкція у DTO через Select (уникнення надлишкового SELECT *)
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 2: Серверна проєкція Select у рекорд CourseSummaryDto ---");
        var projectedCourses = await context.Courses
            .AsNoTracking()
            .OrderBy(c => c.Price)
            .Select(c => new CourseSummaryDto(
                c.Title,
                c.Category.Name,
                c.Price,
                Math.Round(c.Price / c.DurationHours, 2)))
            .ToListAsync();

        foreach (var dto in projectedCourses)
        {
            Console.WriteLine($"   [{dto.CategoryName,-18}] {dto.CourseTitle,-38} | {dto.Price,6} грн ({dto.PricePerHour,6} грн/год)");
        }

        // --------------------------------------------------------------------
        // 3. Групування (GroupBy) та серверна агрегація (Count, Sum, Average)
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 3: Аналітичне групування GroupBy на стороні SQL-сервера ---");
        var categoryStats = await context.Courses
            .GroupBy(c => c.Category.Name)
            .Select(g => new
            {
                CategoryName = g.Key,
                CoursesCount = g.Count(),
                TotalStudents = g.Sum(c => c.StudentsEnrolled),
                AveragePrice = g.Average(c => (double)c.Price),
                MaxRating = g.Max(c => c.Rating)
            })
            .OrderByDescending(s => s.TotalStudents)
            .ToListAsync();

        foreach (var stat in categoryStats)
        {
            Console.WriteLine(
                $"   Категорія: {stat.CategoryName,-18} | Курсів: {stat.CoursesCount} | " +
                $"Студентів: {stat.TotalStudents,5} | Сер. ціна: {stat.AveragePrice,7:F0} грн | Макс. рейтинг: {stat.MaxRating:F1}");
        }

        // --------------------------------------------------------------------
        // 4. Порівняння Tracking vs AsNoTracking vs AsNoTrackingWithIdentityResolution
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 4: Різниця між AsNoTracking() та AsNoTrackingWithIdentityResolution() ---");

        // Завантажуємо всі курси разом із категорією у режимі звичайного AsNoTracking()
        var coursesNoTracking = await context.Courses
            .Include(c => c.Category)
            .AsNoTracking()
            .Where(c => c.CategoryId == 1)
            .ToListAsync();

        bool sameCategoryInstanceNoTracking = ReferenceEquals(
            coursesNoTracking[0].Category,
            coursesNoTracking[1].Category);

        // А тепер з AsNoTrackingWithIdentityResolution()
        var coursesWithIdentityResolution = await context.Courses
            .Include(c => c.Category)
            .AsNoTrackingWithIdentityResolution()
            .Where(c => c.CategoryId == 1)
            .ToListAsync();

        bool sameCategoryInstanceIdentityRes = ReferenceEquals(
            coursesWithIdentityResolution[0].Category,
            coursesWithIdentityResolution[1].Category);

        Console.WriteLine($"   Кількість об'єктів у ChangeTracker зараз: {context.ChangeTracker.Entries().Count()}");
        Console.WriteLine($"   1) AsNoTracking(): чи вказують два курси однієї категорії на один і той самий об'єкт Category в купі? -> {sameCategoryInstanceNoTracking} (створюються дублікати)");
        Console.WriteLine($"   2) AsNoTrackingWithIdentityResolution(): чи вказують на єдиний об'єкт Category? -> {sameCategoryInstanceIdentityRes} (дедуплікація без накладних витрат ChangeTracker)");
    }

    private static async Task SeedAcademyAsync()
    {
        await using var context = new AcademyQueriesDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var backend = new Category { Name = "Backend .NET" };
        var architecture = new Category { Name = "Software Architecture" };
        var databases = new Category { Name = "Databases & ORM" };

        context.Courses.AddRange(
            new Course { Title = "C# 14 та .NET 10: Фундаментальний ООП", Category = backend, Price = 3200m, DurationHours = 45, Rating = 4.9, StudentsEnrolled = 1450 },
            new Course { Title = "Асинхронне програмування в C# та Багатопоточність", Category = backend, Price = 2800m, DurationHours = 32, Rating = 4.8, StudentsEnrolled = 980 },
            new Course { Title = "Чиста Архітектура та патерни GoF у C#", Category = architecture, Price = 4100m, DurationHours = 50, Rating = 4.9, StudentsEnrolled = 820 },
            new Course { Title = "Мікросервіси та Domain-Driven Design", Category = architecture, Price = 4800m, DurationHours = 60, Rating = 4.6, StudentsEnrolled = 540 },
            new Course { Title = "Entity Framework Core 10: Повне керівництво", Category = databases, Price = 3500m, DurationHours = 40, Rating = 5.0, StudentsEnrolled = 1620 },
            new Course { Title = "Оптимізація SQL-запитів та індексів", Category = databases, Price = 2900m, DurationHours = 28, Rating = 4.7, StudentsEnrolled = 1100 }
        );

        await context.SaveChangesAsync();
    }
}
