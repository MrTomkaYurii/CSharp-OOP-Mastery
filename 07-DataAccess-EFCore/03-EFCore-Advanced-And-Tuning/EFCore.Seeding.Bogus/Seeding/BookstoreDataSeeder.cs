using Bogus;
using EFCore.Seeding.Bogus.Data;
using EFCore.Seeding.Bogus.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Seeding.Bogus.Seeding;

/// <summary>
/// Сервіс генерації та наповнення бази даних реалістичними тестовими даними за допомогою Bogus.
/// </summary>
public class BookstoreDataSeeder
{
    public static async Task SeedAsync(BookstoreDbContext context, int authorCount = 20)
    {
        // 1. Перевірка ідемпотентності: якщо база вже заповнена, повторно не сідимо
        if (await context.Authors.AnyAsync())
        {
            Console.WriteLine("    [Seeder]: База даних вже містить записи. Пропуск сідингу.");
            return;
        }

        Console.WriteLine($"    [Seeder]: Генерація {authorCount} авторів та пов'язаних книг/відгуків через Bogus...");

        // Встановлюємо фіксований Seed для відтворюваності результатів генерації
        Randomizer.Seed = new Random(42);

        // Генератор відгуків
        var reviewFaker = new Faker<Review>("uk")
            .RuleFor(r => r.ReviewerName, f => f.Name.FullName())
            .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
            .RuleFor(r => r.Comment, f => f.Rant.Review("книга"))
            .RuleFor(r => r.CreatedAt, f => f.Date.Recent(60));

        // Генератор книг
        var bookFaker = new Faker<Book>("uk")
            .RuleFor(b => b.Title, f => $"{f.Company.CatchPhrase()} - Повний довідник")
            .RuleFor(b => b.Isbn, f => f.Commerce.Ean13())
            .RuleFor(b => b.Price, f => Math.Round(f.Random.Decimal(180m, 1600m), 2))
            .RuleFor(b => b.PublishedDate, f => f.Date.Past(8))
            .RuleFor(b => b.Reviews, f => reviewFaker.Generate(f.Random.Int(1, 6)));

        // Генератор авторів
        var authorFaker = new Faker<Author>("uk")
            .RuleFor(a => a.FirstName, f => f.Name.FirstName())
            .RuleFor(a => a.LastName, f => f.Name.LastName())
            .RuleFor(a => a.Email, (f, a) => f.Internet.Email(a.FirstName, a.LastName))
            .RuleFor(a => a.Biography, f => f.Lorem.Paragraph(2))
            .RuleFor(a => a.Books, f => bookFaker.Generate(f.Random.Int(2, 5)));

        var authors = authorFaker.Generate(authorCount);

        // Додаємо згенерований граф об'єктів у базу
        await context.Authors.AddRangeAsync(authors);
        await context.SaveChangesAsync();

        int totalBooks = await context.Books.CountAsync();
        int totalReviews = await context.Reviews.CountAsync();

        Console.WriteLine($"    [Seeder]: Успішно створено:");
        Console.WriteLine($"      - Авторів:  {authorCount}");
        Console.WriteLine($"      - Книг:     {totalBooks}");
        Console.WriteLine($"      - Відгуків: {totalReviews}");
    }
}
