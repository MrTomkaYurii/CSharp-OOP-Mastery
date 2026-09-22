namespace DI.WebHost.MinimalApi.Services;

public record ProductDto(int Id, string Title, decimal Price, string Category);

public interface ICatalogService
{
    IReadOnlyList<ProductDto> GetProducts();
    ProductDto? GetById(int id);
}

public class InMemoryCatalogService : ICatalogService
{
    private readonly List<ProductDto> _products =
    [
        new(1, "Ноутбук Dell XPS 15", 74999.00m, "Комп'ютери"),
        new(2, "Монітор LG UltraFine 4K", 21500.00m, "Периферія"),
        new(3, "Механічна клавіатура Keychron K2", 4200.00m, "Аксесуари"),
        new(4, "Бездротова миша Logitech MX Master 3S", 3999.00m, "Аксесуари")
    ];

    public IReadOnlyList<ProductDto> GetProducts() => _products;

    public ProductDto? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
}
