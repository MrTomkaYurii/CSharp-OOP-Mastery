namespace EFCore.AdvancedMapping.Models;

public class ProductSpecifications
{
    public double WeightKg { get; set; }
    public string Color { get; set; } = string.Empty;
    public List<string> FeatureTags { get; set; } = [];
}

public class CatalogProduct
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    // JSON-стовпець у базі даних
    public ProductSpecifications Specifications { get; set; } = new();
}
