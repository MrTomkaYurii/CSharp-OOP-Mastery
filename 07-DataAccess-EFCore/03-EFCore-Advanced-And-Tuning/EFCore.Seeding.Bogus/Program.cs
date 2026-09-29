using System.Text;
using EFCore.Seeding.Bogus.Data;
using EFCore.Seeding.Bogus.Seeding;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  EF CORE 10: СИДИНГ ТА ГЕНЕРАЦІЯ РЕЛЯЦІЙНИХ ДАНИХ (BOGUS)");
Console.WriteLine("=================================================================\n");

using var context = new BookstoreDbContext();
Console.WriteLine("1. Скидання та створення схеми бази даних SQLite...");
await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();

Console.WriteLine("\n2. Виконання первинного сідингу (Bogus Data Generation):");
await BookstoreDataSeeder.SeedAsync(context, authorCount: 10);

Console.WriteLine("\n3. Перевірка ідемпотентності (повторний виклик сідера):");
await BookstoreDataSeeder.SeedAsync(context, authorCount: 10);

Console.WriteLine("\n4. Вибірка згенерованих даних з бази:");
var topAuthors = await context.Authors
    .Include(a => a.Books)
        .ThenInclude(b => b.Reviews)
    .Take(3)
    .ToListAsync();

foreach (var author in topAuthors)
{
    Console.WriteLine($"\n  Автор: {author.FirstName} {author.LastName} ({author.Email})");
    Console.WriteLine($"  Біографія: {author.Biography[..Math.Min(90, author.Biography.Length)]}...");
    Console.WriteLine($"  Кількість опублікованих книг: {author.Books.Count}");

    foreach (var book in author.Books.Take(2))
    {
        double avgRating = book.Reviews.Count > 0 ? book.Reviews.Average(r => r.Rating) : 0;
        Console.WriteLine($"    -> Книга: \"{book.Title}\" | Ціна: {book.Price} грн | Відгуків: {book.Reviews.Count} | Сер. оцінка: {avgRating:F1}/5");
    }
}

var stats = new
{
    TotalAuthors = await context.Authors.CountAsync(),
    TotalBooks = await context.Books.CountAsync(),
    TotalReviews = await context.Reviews.CountAsync(),
    AvgPrice = await context.Books.AverageAsync(b => b.Price)
};

Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine($"Загальна статистика згенерованого каталогу:");
Console.WriteLine($"  Всього авторів: {stats.TotalAuthors}");
Console.WriteLine($"  Всього книг:    {stats.TotalBooks}");
Console.WriteLine($"  Всього відгуків:{stats.TotalReviews}");
Console.WriteLine($"  Середня ціна:   {stats.AvgPrice:F2} грн");
Console.WriteLine("=================================================================");
Console.WriteLine("  EFCore.Seeding.Bogus успішно завершив виконання!");
Console.WriteLine("=================================================================");
