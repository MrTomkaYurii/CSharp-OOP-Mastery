namespace EFCoreCleanArchitectureDemo.Domain;

/// <summary>
/// Доменна сутність автора книги (таблиця Authors у БД).
/// </summary>
public sealed class Author
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Country { get; set; }

    // Навігаційна властивість EF Core (зв'язок 1-до-багатьох)
    public ICollection<Book> Books { get; set; } = new List<Book>();
}

/// <summary>
/// Доменна сутність книги (таблиця Books у БД).
/// Не повинна напряму передаватися в UI-шар (для цього використовується BookCatalogDto).
/// </summary>
public sealed class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public decimal BasePrice { get; set; }
    public int PublishedYear { get; set; }

    // Зовнішній ключ та навігаційна властивість до Author
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}
