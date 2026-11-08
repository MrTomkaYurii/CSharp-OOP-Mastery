using AutoMapper;
using EFCoreCleanArchitectureDemo.Domain;

namespace EFCoreCleanArchitectureDemo.Application;

/// <summary>
/// Data Transfer Object (DTO) для відображення книги у клієнтському інтерфейсі.
/// Приховує внутрішню структуру БД та містить уже сплющене (Flattened) ім'я автора.
/// </summary>
public sealed class BookCatalogDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }
}

/// <summary>
/// Профіль AutoMapper: описує правила трансформації доменної сутності EF Core (Book) у DTO (BookCatalogDto).
/// </summary>
public sealed class BookstoreMappingProfile : Profile
{
    public BookstoreMappingProfile()
    {
        CreateMap<Book, BookCatalogDto>()
            .ForMember(
                dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : "Невідомий автор"));
    }
}

/// <summary>
/// SOLID - OCP (Open/Closed Principle): Контракт розрахунку ціни зі знижкою.
/// Дозволяє додавати нові правила ціноутворення без зміни коду сервісу каталогу.
/// </summary>
public interface IPricingStrategy
{
    string StrategyName { get; }
    decimal CalculateFinalPrice(Book book);
}

/// <summary>
/// Реалізація OCP: Академічна знижка 15% на технічну літературу.
/// </summary>
public sealed class AcademicDiscountPricingStrategy : IPricingStrategy
{
    public string StrategyName => "Академічна знижка (-15% на Architecture & .NET)";

    public decimal CalculateFinalPrice(Book book)
    {
        if (book.Category is "Architecture" or ".NET")
        {
            return Math.Round(book.BasePrice * 0.85m, 2);
        }

        return book.BasePrice;
    }
}
