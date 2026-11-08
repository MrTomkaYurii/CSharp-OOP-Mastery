using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoreApp.BusinessLogic.Models;
using StoreApp.DataAccess.Entities;
using StoreApp.DataAccess.Repositories;

namespace StoreApp.BusinessLogic.Services;

/// <summary>
/// Сервіс бізнес-логіки керування каталогом товарів:
/// вибірка DTO, контроль критичних залишків, автоматичне поповнення складу та переоцінка.
/// </summary>
public sealed class CatalogService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly StoreSettings _settings;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(
        IProductRepository productRepository,
        IMapper mapper,
        IOptions<StoreSettings> options,
        ILogger<CatalogService> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductDto>> GetCatalogAsync(CancellationToken ct = default)
    {
        IReadOnlyList<Product> products = await _productRepository.GetAllWithCategoryReadOnlyAsync(ct);
        return _mapper.Map<IReadOnlyList<ProductDto>>(products);
    }

    public async Task<IReadOnlyList<ProductDto>> GetLowStockAlertsAsync(CancellationToken ct = default)
    {
        IReadOnlyList<Product> lowStock = await _productRepository.GetLowStockProductsReadOnlyAsync(ct);
        return _mapper.Map<IReadOnlyList<ProductDto>>(lowStock);
    }

    /// <summary>
    /// Автоматично поповнює складські залишки для позицій, що впали нижче порогу <see cref="Product.ReorderLevel"/>.
    /// </summary>
    public async Task<int> ReplenishLowStockProductsAsync(CancellationToken ct = default)
    {
        IReadOnlyList<Product> lowStockPreview = await _productRepository.GetLowStockProductsReadOnlyAsync(ct);
        if (lowStockPreview.Count == 0)
        {
            return 0;
        }

        IReadOnlyList<Product> trackedProducts = await _productRepository.GetBySkusTrackedAsync(
            lowStockPreview.Select(p => p.Sku), ct);

        foreach (Product product in trackedProducts)
        {
            int oldQty = product.StockQuantity;
            product.StockQuantity += _settings.DefaultReplenishQuantity;

            _logger.LogInformation(
                " Поповнено склад для {Sku} ({Name}): {OldQty} -> {NewQty} од.",
                product.Sku, product.Name, oldQty, product.StockQuantity);
        }

        await _productRepository.SaveChangesAsync(ct);
        return trackedProducts.Count;
    }
}

/// <summary>
/// Сервіс бізнес-логіки оформлення замовлень:
/// перевіряє наявність товарів, розраховує прогресивну знижку згідно зі <see cref="StoreSettings"/>
/// та викликає транзакційне збереження в репозиторії.
/// </summary>
public sealed class OrderProcessingService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly StoreSettings _settings;
    private readonly ILogger<OrderProcessingService> _logger;

    public OrderProcessingService(
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IMapper mapper,
        IOptions<StoreSettings> options,
        ILogger<OrderProcessingService> logger)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<OrderReceiptDto> PlaceOrderAsync(
        string customerName,
        IReadOnlyList<OrderItemRequestDto> requestedItems,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Ім'я клієнта не може бути порожнім.", nameof(customerName));
        }

        if (requestedItems.Count == 0)
        {
            throw new InvalidOperationException("Замовлення має містити щонайменше одну товарну позицію.");
        }

        IReadOnlyList<Product> products = await _productRepository.GetBySkusTrackedAsync(
            requestedItems.Select(i => i.Sku), ct);

        var productsBySku = products.ToDictionary(p => p.Sku, StringComparer.OrdinalIgnoreCase);

        var orderLines = new List<OrderLine>();
        var deductions = new Dictionary<int, int>();
        decimal subtotal = 0m;

        foreach (OrderItemRequestDto req in requestedItems)
        {
            if (!productsBySku.TryGetValue(req.Sku, out Product? product))
            {
                throw new InvalidOperationException($"Товар з артикулом '{req.Sku}' не знайдено у каталозі.");
            }

            if (req.Quantity <= 0)
            {
                throw new InvalidOperationException($"Кількість для товару '{req.Sku}' має бути більшою за нуль.");
            }

            if (product.StockQuantity < req.Quantity)
            {
                throw new InvalidOperationException(
                    $"Недостатньо товару '{product.Name}' ({product.Sku}) на складі: запитано {req.Quantity}, доступно {product.StockQuantity}.");
            }

            decimal lineTotal = product.UnitPrice * req.Quantity;
            subtotal += lineTotal;

            orderLines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantity = req.Quantity,
                UnitPriceAtPurchase = product.UnitPrice
            });

            deductions[product.Id] = deductions.GetValueOrDefault(product.Id) + req.Quantity;
        }

        // Розрахунок знижки за бізнес-правилами з конфігурації StoreSettings
        decimal discount = subtotal >= _settings.VipDiscountThreshold
            ? Math.Round(subtotal * (_settings.VipDiscountPercent / 100m), 2)
            : 0m;

        decimal finalAmount = subtotal - discount;
        string orderNumber = $"ORD-{DateTime.UtcNow:yyyy}-{Random.Shared.Next(1000, 9999)}";

        var newOrder = new CustomerOrder
        {
            OrderNumber = orderNumber,
            CustomerName = customerName,
            CreatedAtUtc = DateTime.UtcNow,
            SubtotalAmount = subtotal,
            DiscountAmount = discount,
            FinalAmount = finalAmount,
            Lines = orderLines
        };

        await _orderRepository.CreateOrderInTransactionAsync(newOrder, deductions, ct);

        _logger.LogInformation(
            " Оформлено замовлення {OrderNumber} для «{CustomerName}» на суму {FinalAmount} грн (знижка: {Discount} грн).",
            orderNumber, customerName, finalAmount, discount);

        // Отримуємо збережене замовлення з навігаційними властивостями для мапінгу в квитанцію
        IReadOnlyList<CustomerOrder> allOrders = await _orderRepository.GetAllOrdersWithDetailsReadOnlyAsync(ct);
        CustomerOrder savedOrder = allOrders.First(o => o.OrderNumber == orderNumber);

        return _mapper.Map<OrderReceiptDto>(savedOrder);
    }
}

/// <summary>
/// Сервіс аналітики: агрегує складські запаси та обсяги продажів у розрізі категорій.
/// </summary>
public sealed class AnalyticsService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public AnalyticsService(
        IProductRepository productRepository,
        IOrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<CategoryAnalyticsDto>> GetCategoryAnalyticsAsync(CancellationToken ct = default)
    {
        IReadOnlyList<Product> products = await _productRepository.GetAllWithCategoryReadOnlyAsync(ct);
        IReadOnlyList<CustomerOrder> orders = await _orderRepository.GetAllOrdersWithDetailsReadOnlyAsync(ct);

        // Групуємо продані одиниці та виторг за категоріями (з урахуванням пропорційної знижки замовлення)
        var salesByCategory = orders
            .SelectMany(order =>
            {
                decimal ratio = order.SubtotalAmount > 0
                    ? order.FinalAmount / order.SubtotalAmount
                    : 1m;

                return order.Lines.Select(line => new
                {
                    CategoryName = line.Product.Category.Name,
                    line.Quantity,
                    EffectiveRevenue = Math.Round(line.Quantity * line.UnitPriceAtPurchase * ratio, 2)
                });
            })
            .GroupBy(x => x.CategoryName)
            .ToDictionary(
                g => g.Key,
                g => (UnitsSold: g.Sum(x => x.Quantity), Revenue: g.Sum(x => x.EffectiveRevenue)));

        return products
            .GroupBy(p => p.Category.Name)
            .Select(group =>
            {
                salesByCategory.TryGetValue(group.Key, out var sales);
                return new CategoryAnalyticsDto(
                    CategoryName: group.Key,
                    DistinctProductsCount: group.Count(),
                    TotalUnitsInStock: group.Sum(p => p.StockQuantity),
                    InventoryValueUah: group.Sum(p => p.UnitPrice * p.StockQuantity),
                    UnitsSold: sales.UnitsSold,
                    RevenueUah: sales.Revenue);
            })
            .OrderByDescending(a => a.RevenueUah)
            .ToList();
    }
}
