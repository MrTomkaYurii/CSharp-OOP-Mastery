using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using StoreApp.BusinessLogic.Models;
using StoreApp.BusinessLogic.Services;
using StoreApp.DataAccess.Context;
using StoreApp.DataAccess.Repositories;

namespace StoreApp.ConsoleUI;

// ============================================================================
// ПРОЄКТ 03: Презентаційний консольний шар (Generic Host + Spectre.Console)
// ============================================================================
// Об'єднує всі вивчені технології Глави 08 у тришаровій архітектурі:
// • Шар даних (StoreApp.DataAccess): EF Core SQLite + Репозиторії + Транзакції
// • Шар бізнес-логіки (StoreApp.BusinessLogic): Сервіси + DTO + AutoMapper + IOptions<StoreSettings>
// • Презентаційний шар (StoreApp.ConsoleUI): Generic Host + DI Scopes + багатий TUI Spectre.Console
// ============================================================================

internal static class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.SetBasePath(AppContext.BaseDirectory);
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "[HH:mm:ss] ";
        });

        // 1. Реєстрація та валідація конфігурації StoreSettings (Options Pattern)
        builder.Services
            .AddOptions<StoreSettings>()
            .Bind(builder.Configuration.GetSection(StoreSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // 2. Реєстрація контексту бази даних EF Core (SQLite)
        builder.Services.AddDbContext<StoreDbContext>((sp, dbOptions) =>
        {
            StoreSettings settings = sp.GetRequiredService<IOptions<StoreSettings>>().Value;
            string dbPath = Path.Combine(AppContext.BaseDirectory, settings.DatabaseFileName);
            dbOptions.UseSqlite($"Data Source={dbPath}");
        });

        // 3. Реєстрація профілів AutoMapper
        builder.Services.AddAutoMapper(typeof(StoreMappingProfile).Assembly);

        // 4. Реєстрація репозиторіїв (Data Access Layer) та сервісів (Business Logic Layer)
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        builder.Services.AddScoped<CatalogService>();
        builder.Services.AddScoped<OrderProcessingService>();
        builder.Services.AddScoped<AnalyticsService>();

        builder.Services.AddScoped<StoreConsoleDashboard>();

        using IHost host = builder.Build();
        await host.StartAsync();

        // Виконуємо демонстраційний сценарій у межах ізольованого IServiceScope
        await using (AsyncServiceScope scope = host.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            await StoreDatabaseInitializer.InitializeAndSeedAsync(dbContext);

            var dashboard = scope.ServiceProvider.GetRequiredService<StoreConsoleDashboard>();
            await dashboard.RunShowcaseScenarioAsync();
        }

        await host.StopAsync();
    }
}

/// <summary>
/// Презентаційний координатор інтерфейсу на базі <see cref="AnsiConsole"/> (Spectre.Console).
/// Працює виключно з моделями DTO та бізнес-сервісами, не маючи прямої залежності від сутностей БД.
/// </summary>
public sealed class StoreConsoleDashboard
{
    private readonly CatalogService _catalogService;
    private readonly OrderProcessingService _orderService;
    private readonly AnalyticsService _analyticsService;
    private readonly StoreSettings _settings;

    public StoreConsoleDashboard(
        CatalogService catalogService,
        OrderProcessingService orderService,
        AnalyticsService analyticsService,
        IOptions<StoreSettings> options)
    {
        _catalogService = catalogService;
        _orderService = orderService;
        _analyticsService = analyticsService;
        _settings = options.Value;
    }

    public async Task RunShowcaseScenarioAsync(CancellationToken ct = default)
    {
        RenderBanner();

        // КРОК 1: Відображення поточного каталогу товарів та критичних залишків
        AnsiConsole.Write(new Rule("[bold yellow]Крок 1. Початковий стан каталогу товарів (DTO через AutoMapper)[/]").LeftJustified());
        IReadOnlyList<ProductDto> initialCatalog = await _catalogService.GetCatalogAsync(ct);
        RenderCatalogTable(initialCatalog);

        // КРОК 2: Автоматичне поповнення критичних залишків на складі
        AnsiConsole.Write(new Rule("[bold cyan]Крок 2. Контроль мінімальних залишків та поповнення складу[/]").LeftJustified());
        int replenishedCount = await _catalogService.ReplenishLowStockProductsAsync(ct);
        AnsiConsole.MarkupLine($"[green]✔ Автоматично поповнено позицій із низьким залишком:[/] [bold white]{replenishedCount}[/] (по +{_settings.DefaultReplenishQuantity} од.)\n");

        // КРОК 3: Оформлення нових замовлень у транзакціях SQLite
        AnsiConsole.Write(new Rule("[bold green]Крок 3. Оформлення клієнтських замовлень (EF Core Transaction + VIP-знижка)[/]").LeftJustified());

        OrderReceiptDto receipt1 = await _orderService.PlaceOrderAsync(
            customerName: "Олена Косач (Студія Дизайну)",
            requestedItems: new[]
            {
                new OrderItemRequestDto("MON-4K-27", Quantity: 2),
                new OrderItemRequestDto("KBD-MECH-UA", Quantity: 2)
            },
            ct);

        RenderOrderReceiptPanel(receipt1);

        OrderReceiptDto receipt2 = await _orderService.PlaceOrderAsync(
            customerName: "Михайло Грушевський (Аналітичний Центр)",
            requestedItems: new[]
            {
                new OrderItemRequestDto("RAM-DDR5-64", Quantity: 2),
                new OrderItemRequestDto("SSD-NVME-2TB", Quantity: 2)
            },
            ct);

        RenderOrderReceiptPanel(receipt2);

        // КРОК 4: Зведена фінансова аналітика (BarChart + BreakdownChart)
        AnsiConsole.Write(new Rule("[bold magenta]Крок 4. Аналітична панель продажів та залишків за категоріями[/]").LeftJustified());
        IReadOnlyList<CategoryAnalyticsDto> analytics = await _analyticsService.GetCategoryAnalyticsAsync(ct);
        RenderAnalyticsWidgets(analytics);
    }

    private void RenderBanner()
    {
        var headerPanel = new Panel(
            new Markup(
                $"[bold cyan]{Markup.Escape(_settings.StoreName)}[/]\n" +
                $"[grey]Тришарова архітектура:[/] [white]DataAccess (EF Core SQLite)[/] ➔ [white]BusinessLogic (Services, DTO, AutoMapper)[/] ➔ [white]ConsoleUI (Spectre.Console)[/]\n" +
                $"[grey]Поріг VIP-знижки ({_settings.VipDiscountPercent}%):[/] [yellow]{_settings.VipDiscountThreshold:N2} грн[/]"))
        {
            Border = BoxBorder.Double,
            Padding = new Padding(2, 1)
        };

        AnsiConsole.Write(headerPanel);
        AnsiConsole.WriteLine();
    }

    private static void RenderCatalogTable(IReadOnlyList<ProductDto> products)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold white] Актуальний каталог товарів на складі[/]")
            .AddColumn(new TableColumn("[bold]SKU[/]"))
            .AddColumn(new TableColumn("[bold]Назва товару[/]"))
            .AddColumn(new TableColumn("[bold]Категорія[/]"))
            .AddColumn(new TableColumn("[bold]Ціна (грн)[/]").RightAligned())
            .AddColumn(new TableColumn("[bold]Залишок[/]").RightAligned())
            .AddColumn(new TableColumn("[bold]Статус складу[/]"));

        foreach (ProductDto item in products)
        {
            string statusBadge = item.NeedsRestock
                ? $"[bold red]⚠ Потребує закупівлі (≤{item.ReorderLevel})[/]"
                : "[green]✔ Достатній запас[/]";

            table.AddRow(
                $"[cyan]{Markup.Escape(item.Sku)}[/]",
                Markup.Escape(item.Name),
                $"[grey]{Markup.Escape(item.CategoryName)}[/]",
                $"{item.UnitPrice:N2}",
                item.NeedsRestock ? $"[bold red]{item.StockQuantity}[/]" : $"[white]{item.StockQuantity}[/]",
                statusBadge);
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private static void RenderOrderReceiptPanel(OrderReceiptDto receipt)
    {
        var linesTable = new Table()
            .Border(TableBorder.Simple)
            .AddColumn("Артикул")
            .AddColumn("Товар")
            .AddColumn(new TableColumn("К-сть").RightAligned())
            .AddColumn(new TableColumn("Ціна").RightAligned())
            .AddColumn(new TableColumn("Сума").RightAligned());

        foreach (OrderLineDto line in receipt.Lines)
        {
            linesTable.AddRow(
                $"[cyan]{Markup.Escape(line.ProductSku)}[/]",
                Markup.Escape(line.ProductName),
                line.Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture),
                $"{line.UnitPriceAtPurchase:N2} грн",
                $"[bold]{line.LineTotal:N2} грн[/]");
        }

        string discountText = receipt.DiscountAmount > 0
            ? $"[green]-{receipt.DiscountAmount:N2} грн (VIP)[/]"
            : "[grey]0.00 грн[/]";

        var summaryMarkup = new Markup(
            $"[bold]Номер замовлення:[/] [yellow]{Markup.Escape(receipt.OrderNumber)}[/]  |  " +
            $"[bold]Покупець:[/] [white]{Markup.Escape(receipt.CustomerName)}[/]\n" +
            $"[grey]Сума без знижки:[/] {receipt.SubtotalAmount:N2} грн  |  " +
            $"[grey]Знижка:[/] {discountText}  |  " +
            $"[bold green]До сплати: {receipt.FinalAmount:N2} грн[/]");

        var panel = new Panel(new Rows(summaryMarkup, linesTable))
        {
            Header = new PanelHeader($"  Квитанція замовлення {Markup.Escape(receipt.OrderNumber)} "),
            Border = BoxBorder.Rounded
        };

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    private static void RenderAnalyticsWidgets(IReadOnlyList<CategoryAnalyticsDto> analytics)
    {
        Color[] palette = [Color.Aqua, Color.SpringGreen1, Color.Gold1, Color.MediumPurple1];

        var barChart = new BarChart()
            .Width(72)
            .Label("[bold white]Виторг від продажів за категоріями (грн)[/]")
            .CenterLabel();

        var breakdown = new BreakdownChart()
            .Width(72)
            .UseValueFormatter(v => $"{v:N1}%");

        decimal totalInventoryValue = Math.Max(1m, analytics.Sum(a => a.InventoryValueUah));

        for (int i = 0; i < analytics.Count; i++)
        {
            CategoryAnalyticsDto item = analytics[i];
            Color color = palette[i % palette.Length];

            barChart.AddItem(item.CategoryName, (double)Math.Round(item.RevenueUah, 0), color);
            double sharePercent = (double)Math.Round(item.InventoryValueUah / totalInventoryValue * 100m, 1);
            breakdown.AddItem(item.CategoryName, sharePercent, color);
        }

        AnsiConsole.Write(barChart);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold white]Структура вартості складських залишків за категоріями:[/]");
        AnsiConsole.Write(breakdown);
        AnsiConsole.WriteLine();
    }
}
