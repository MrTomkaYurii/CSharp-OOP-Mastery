namespace DependencyInjection.Models;

/// <summary>
/// Доменна модель товару в інтернет-магазині.
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public Product()
    {
    }

    public Product(int id, string name, decimal price, int stockQuantity)
    {
        Id = id;
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public override string ToString() => $"[Товар #{Id}] {Name} - {Price:C} (Залишок: {StockQuantity} шт.)";
}
