namespace JsonSerializationDemo;

/// <summary>
/// Доменна модель товару інтернет-магазину для демонстрації рефлексивної серіалізації в JSON.
/// </summary>
public class ProductItem
{
    [JsonField("product_id")]
    public int Id { get; set; }

    [JsonField("title")]
    public string Name { get; set; } = string.Empty;

    [JsonField("price_usd")]
    public double Price { get; set; }

    [JsonField("in_stock")]
    public bool IsAvailable { get; set; }

    [JsonField("tags")]
    public string[] Tags { get; set; } = Array.Empty<string>();

    // Службова інформація складу, яка не повинна потрапляти у публічний JSON
    [JsonExclude]
    public decimal WholesaleSupplierCost { get; set; }
}
