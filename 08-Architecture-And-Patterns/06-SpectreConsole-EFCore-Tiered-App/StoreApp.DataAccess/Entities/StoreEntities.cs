namespace StoreApp.DataAccess.Entities;

/// <summary>
/// Категорія товарів у каталозі магазину електроніки та комплектуючих.
/// </summary>
public sealed class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

/// <summary>
/// Сутність товару на складі магазину.
/// </summary>
public sealed class Product
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; } = 5;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}

/// <summary>
/// Замовлення покупця.
/// </summary>
public sealed class CustomerOrder
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public decimal SubtotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }

    public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
}

/// <summary>
/// Позиція (рядок) у замовленні покупця.
/// </summary>
public sealed class OrderLine
{
    public int Id { get; set; }
    public int CustomerOrderId { get; set; }
    public CustomerOrder CustomerOrder { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPriceAtPurchase { get; set; }
}
