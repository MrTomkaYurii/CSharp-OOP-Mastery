using AutoMapper;
using EFCoreCleanArchitectureDemo.Application;
using EFCoreCleanArchitectureDemo.Domain;
using EFCoreCleanArchitectureDemo.Infrastructure;
using Spectre.Console;

namespace EFCoreCleanArchitectureDemo.Presentation;

/// <summary>
/// Сервіс бізнес-логіки каталогу.
/// Координує роботу репозиторію EF Core, трансформацію через AutoMapper та розрахунок цін за стратегією OCP.
/// </summary>
public sealed class CatalogApplicationService
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPricingStrategy _pricingStrategy;

    public CatalogApplicationService(
        IBookRepository repository,
        IMapper mapper,
        IPricingStrategy pricingStrategy)
    {
        _repository = repository;
        _mapper = mapper;
        _pricingStrategy = pricingStrategy;
    }

    public string ActivePricingStrategyName => _pricingStrategy.StrategyName;

    public async Task<IReadOnlyList<BookCatalogDto>> GetCatalogDtosAsync()
    {
        await _repository.SeedInitialDataIfEmptyAsync();
        IReadOnlyList<Book> entities = await _repository.GetAllWithAuthorsAsync();

        // Трансформуємо сутності БД (Book) у безпечні DTO (BookCatalogDto) через AutoMapper
        var dtos = _mapper.Map<List<BookCatalogDto>>(entities);

        for (int i = 0; i < entities.Count; i++)
        {
            dtos[i].FinalPrice = _pricingStrategy.CalculateFinalPrice(entities[i]);
        }

        return dtos;
    }
}

/// <summary>
/// UI-компонент на базі Spectre.Console (SRP: відповідає виключно за візуалізацію даних у терміналі).
/// </summary>
public sealed class SpectreCatalogRenderer
{
    public void RenderCatalog(IReadOnlyList<BookCatalogDto> books, string pricingName)
    {
        var headerPanel = new Panel(
            new Markup($"[bold yellow]Інтеграційний приклад:[/] EF Core (SQLite) + SOLID + AutoMapper (DTO) + DI + Spectre.Console\n[grey]Активна стратегія цін (OCP): {pricingName}[/]"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0, 1, 0)
        };
        AnsiConsole.Write(headerPanel);
        AnsiConsole.WriteLine();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[bold cyan]ID[/]").RightAligned())
            .AddColumn("[bold white]Назва книги[/]")
            .AddColumn("[bold green]Автор (AutoMapper Flattening)[/]")
            .AddColumn("[bold magenta]Категорія[/]")
            .AddColumn(new TableColumn("[grey]Базова ціна[/]").RightAligned())
            .AddColumn(new TableColumn("[bold yellow]Ціна зі знижкою[/]").RightAligned());

        foreach (BookCatalogDto book in books)
        {
            string priceBadge = book.FinalPrice < book.BasePrice
                ? $"[bold green]{book.FinalPrice:N2} грн[/]"
                : $"[white]{book.FinalPrice:N2} грн[/]";

            table.AddRow(
                book.Id.ToString(),
                book.Title,
                book.AuthorName,
                book.Category,
                $"{book.BasePrice:N2} грн",
                priceBadge);
        }

        AnsiConsole.Write(table);
    }
}
