using System.ComponentModel.DataAnnotations;
using AutoMapper;
using StoreApp.DataAccess.Entities;

namespace StoreApp.BusinessLogic.Models;

/// <summary>
/// Строго типізовані налаштування бізнес-правил магазину, що завантажуються з appsettings.json через <c>IOptions&lt;StoreSettings&gt;</c>.
/// </summary>
public sealed class StoreSettings
{
    public const string SectionName = "StoreSettings";

    [Required]
    public string StoreName { get; set; } = "TechMastery UA Store";

    [Required]
    public string DatabaseFileName { get; set; } = "techmastery_store.db";

    [Range(1000, 500000)]
    public decimal VipDiscountThreshold { get; set; } = 30000m;

    [Range(0, 50)]
    public decimal VipDiscountPercent { get; set; } = 10.0m;

    [Range(1, 100)]
    public int DefaultReplenishQuantity { get; set; } = 10;
}

/// <summary>
/// DTO картки товару для передачі у презентаційний шар (Spectre.Console UI) без експонування EF-навігаційних властивостей.
/// </summary>
public sealed class ProductDto
{
    public int Id { get; init; }
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int StockQuantity { get; init; }
    public int ReorderLevel { get; init; }
    public bool NeedsRestock => StockQuantity <= ReorderLevel;
    public decimal TotalInventoryValue => UnitPrice * StockQuantity;
}

/// <summary>
/// Вхідний запит на позицію замовлення.
/// </summary>
public sealed record OrderItemRequestDto(string Sku, int Quantity);

/// <summary>
/// DTO оформленого замовлення для відображення квитанції в консольному інтерфейсі.
/// </summary>
public sealed class OrderReceiptDto
{
    public string OrderNumber { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
    public decimal SubtotalAmount { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalAmount { get; init; }
    public IReadOnlyList<OrderLineDto> Lines { get; init; } = Array.Empty<OrderLineDto>();
}

/// <summary>
/// DTO рядка квитанції замовлення.
/// </summary>
public sealed class OrderLineDto
{
    public string ProductSku { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPriceAtPurchase { get; init; }
    public decimal LineTotal => Quantity * UnitPriceAtPurchase;
}

/// <summary>
/// Зведена аналітика за категорією для побудови діаграм BarChart та BreakdownChart у Spectre.Console.
/// </summary>
public sealed record CategoryAnalyticsDto(
    string CategoryName,
    int DistinctProductsCount,
    int TotalUnitsInStock,
    decimal InventoryValueUah,
    int UnitsSold,
    decimal RevenueUah);

/// <summary>
/// Профіль AutoMapper для трансформації доменних сутностей EF Core у плоскі моделі DTO.
/// </summary>
public sealed class StoreMappingProfile : Profile
{
    public StoreMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<OrderLine, OrderLineDto>()
            .ForMember(
                dest => dest.ProductSku,
                opt => opt.MapFrom(src => src.Product.Sku))
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Product.Category.Name));

        CreateMap<CustomerOrder, OrderReceiptDto>();
    }
}
