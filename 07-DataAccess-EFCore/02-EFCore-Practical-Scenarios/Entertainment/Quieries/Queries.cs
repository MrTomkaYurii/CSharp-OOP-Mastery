using Entertainment;
using Entertainment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quieries.Interfaces;
using Spectre.Console;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.Services;

// ============================================
// 1. Найбільш касові фільми
// ============================================
public class HighestGrossingMoviesQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<HighestGrossingMoviesQuery> _logger;

    public string QueryName => "Найбільш касові фільми";
    public string QueryKey => "highest-grossing";

    public HighestGrossingMoviesQuery(
        EntertainmentDbContext context,
        ILogger<HighestGrossingMoviesQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Найбільш касові фільми");

        var movies = _context.Movies
            .OrderByDescending(x => x.WorldwideBoxOfficeGross);

        var sum = await movies.SumAsync(x => x.WorldwideBoxOfficeGross, cancellationToken);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumn("Назва")
            .AddColumn(new TableColumn("Каса ($)").Footer($"[purple]{sum:C}[/]"))
            .AddRows(
                await movies.ToListAsync(cancellationToken),
                m => $"[red]{m.Name}[/]",
                m => $"[green]{m.WorldwideBoxOfficeGross:C}[/]"
            );

        Output.Results(movies, "Найбільш касові фільми", table);
    }
}

// ============================================
// 2. Продукції за рейтингами
// ============================================
public class ProductionsByRatingsQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<ProductionsByRatingsQuery> _logger;

    public string QueryName => "Продукції за рейтингами";
    public string QueryKey => "productions-by-ratings";

    public ProductionsByRatingsQuery(
        EntertainmentDbContext context,
        ILogger<ProductionsByRatingsQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Продукції за рейтингами");

        var highestRated = _context.Productions
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                avg = x.Ratings.Average(r => (double?)r.Stars) ?? 0,
                type = x.GetType().Name
            })
            .OrderByDescending(x => x.avg);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumns("Назва", "Тип", "Рейтинг", "Зірки")
            .AddRows(
                await highestRated.ToListAsync(cancellationToken),
                m => $"[red]{m.name}[/]",
                m => m.type,
                m => $"[green]{m.avg:0.00}[/]",
                m => string.Join("", Enumerable.Repeat("★", (int)Math.Floor(m.avg)))
            );

        Output.Results(highestRated, "Продукції за рейтингами", table);
    }
}

// ============================================
// 3. Актори з кількома персонажами
// ============================================
public class ActorsMultipleCharactersQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<ActorsMultipleCharactersQuery> _logger;

    public string QueryName => "Актори з кількома персонажами";
    public string QueryKey => "actors-multiple-characters";

    public ActorsMultipleCharactersQuery(
        EntertainmentDbContext context,
        ILogger<ActorsMultipleCharactersQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Актори з кількома персонажами");

        var multipleCharacters = _context.Actors
            .Where(a => a.Characters.Count > 1)
            .Select(a => new
            {
                a.Name,
                Characters = a.Characters
                    .Select(x => new
                    {
                        Name = x.Name,
                        ProductionType = x.Production.GetType().Name
                    })
                    .OrderBy(x => x.Name)
                    .ToList()
            });

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumns("Актор", "Персонажі")
            .AddRows(
                await multipleCharacters.ToListAsync(cancellationToken),
                m => $"[red]{m.Name}[/]",
                m => string.Join(", ", m.Characters.Select(c =>
                    $"[green]{c.Name}[/] [fuchsia]({c.ProductionType})[/]"))
            );

        Output.Results(multipleCharacters, "Актори з кількома персонажами", table);
    }
}

// ============================================
// 4. Рейтинги за джерелами
// ============================================
public class RatingsBySourceQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<RatingsBySourceQuery> _logger;

    public string QueryName => "Рейтинги за джерелами";
    public string QueryKey => "ratings-by-source";

    public RatingsBySourceQuery(
        EntertainmentDbContext context,
        ILogger<RatingsBySourceQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Рейтинги за джерелами");

        var sources = _context.Ratings
            .GroupBy(x => x.Source)
            .Select(x => new { Source = x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumns("Джерело", "Кількість")
            .AddRows(
                await sources.ToListAsync(cancellationToken),
                m => $"[red]{m.Source}[/]",
                m => m.Count
            );

        Output.Results(sources, "Рейтинги за джерелами", table);
    }
}

// ============================================
// 5. Серіали з найменшою кількістю епізодів
// ============================================
public class SeriesFewestEpisodesQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<SeriesFewestEpisodesQuery> _logger;

    public string QueryName => "Серіали з найменшою кількістю епізодів";
    public string QueryKey => "series-fewest-episodes";

    public SeriesFewestEpisodesQuery(
        EntertainmentDbContext context,
        ILogger<SeriesFewestEpisodesQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Серіали з найменшою кількістю епізодів");

        var leastEpisodes = _context.Series
            .OrderBy(x => x.NumberOfEpisodes)
            .Select(x => new { x.Name, x.NumberOfEpisodes, x.Release })
            .Take(1);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumns("Назва", "Кількість епізодів", "Дата виходу")
            .AddRows(
                await leastEpisodes.ToListAsync(cancellationToken),
                m => $"[red]{m.Name}[/]",
                m => m.NumberOfEpisodes,
                m => $"[fuchsia]{m.Release:d}[/]"
            );

        Output.Results(leastEpisodes, "Серіали з найменшою кількістю епізодів", table);
    }
}

// ============================================
// 6. Відфільтровані продукції (тільки фільми)
// ============================================
public class FilteredProductionsQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<FilteredProductionsQuery> _logger;

    public string QueryName => "Відфільтровані продукції (фільми)";
    public string QueryKey => "filtered-productions";

    public FilteredProductionsQuery(
        EntertainmentDbContext context,
        ILogger<FilteredProductionsQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Відфільтровані продукції");

        var movies = _context.Productions
            .OfType<Movie>();

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumns("Назва", "Дата виходу")
            .AddRows(
                await movies.ToListAsync(cancellationToken),
                m => $"[red]{m.Name}[/]",
                m => $"[fuchsia]{m.Release:d}[/]"
            );

        Output.Results(movies, "Відфільтровані продукції (фільми)", table);
    }
}

// ============================================
// 7. Актори, які грають самих себе
// ============================================
public class ActorsPlayingThemselvesQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<ActorsPlayingThemselvesQuery> _logger;

    public string QueryName => "Актори, які грають самих себе";
    public string QueryKey => "actors-playing-themselves";

    public ActorsPlayingThemselvesQuery(
        EntertainmentDbContext context,
        ILogger<ActorsPlayingThemselvesQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Актори, які грають самих себе");

        var actorsPlayingThemselves = _context.Characters
            .Where(c => c.Name == c.Actor.Name)
            .Select(c => new
            {
                character = c.Name,
                actor = c.Actor.Name
            });

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumns("Персонаж", "Актор")
            .AddRows(
                await actorsPlayingThemselves.ToListAsync(cancellationToken),
                m => $"[red]{m.character}[/]",
                m => $"[green]{m.actor}[/]"
            );

        Output.Results(actorsPlayingThemselves, "Актори, які грають самих себе", table);
    }
}

// ============================================
// 8. Продукції з "The" в назві
// ============================================
public class ProductionsWithTheQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<ProductionsWithTheQuery> _logger;

    public string QueryName => "Продукції з \"The\" в назві";
    public string QueryKey => "productions-with-the";

    public ProductionsWithTheQuery(
        EntertainmentDbContext context,
        ILogger<ProductionsWithTheQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Продукції з The в назві");

        var productions = _context.Productions
            .Where(x => x.Name.Contains("The"));

        var count = await productions.CountAsync(cancellationToken);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumn(new TableColumn("Назва").Footer($"[purple]Всього: {count}[/]"))
            .AddRows(
                await productions.ToListAsync(cancellationToken),
                m => $"[red]{m.Name}[/]"
            );

        Output.Results(productions, "Продукції з \"The\" в назві", table);
    }
}

// ============================================
// 9. Літні фільми (квітень-липень)
// ============================================
public class SummerMoviesQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<SummerMoviesQuery> _logger;

    public string QueryName => "Літні фільми (квітень-липень)";
    public string QueryKey => "summer-movies";

    public SummerMoviesQuery(
        EntertainmentDbContext context,
        ILogger<SummerMoviesQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Літні фільми");

        var summerMovies = _context.Movies
            .Where(x => x.Release.Month >= 4 && x.Release.Month < 8);

        var count = await summerMovies.CountAsync(cancellationToken);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumn("Назва")
            .AddColumn(new TableColumn("Дата виходу").Footer($"[purple]Всього: {count}[/]"))
            .AddRows(
                await summerMovies.ToListAsync(cancellationToken),
                m => $"[red]{m.Name}[/]",
                m => $"[fuchsia]{m.Release:d}[/]"
            );

        Output.Results(summerMovies, "Літні фільми (квітень-липень)", table);
    }
}

// ============================================
// 10. Найвищий рейтинг
// ============================================
public class HighestRatingQuery : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly ILogger<HighestRatingQuery> _logger;

    public string QueryName => "Найвищий рейтинг";
    public string QueryKey => "highest-rating";

    public HighestRatingQuery(
        EntertainmentDbContext context,
        ILogger<HighestRatingQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Виконання запиту: Найвищий рейтинг");

        var highestRating = _context.Ratings
            .OrderByDescending(x => x.Production.Release)
            .ThenByDescending(x => x.Stars)
            .Select(r => new
            {
                stars = r.Stars,
                source = r.Source,
                production = r.Production.Name
            })
            .Take(1);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Yellow)
            .AddColumns("Джерело", "Зірки", "Продукція")
            .AddRows(
                await highestRating.ToListAsync(cancellationToken),
                m => $"[red]{m.source}[/]",
                m => string.Join("", Enumerable.Repeat("★", m.stars)),
                m => m.production
            );

        Output.Results(highestRating, "Найвищий рейтинг", table);
    }
}