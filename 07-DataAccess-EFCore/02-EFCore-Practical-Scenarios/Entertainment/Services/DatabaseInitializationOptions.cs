using Entertainment;
using Entertainment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quieries.Interfaces;
using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.Configuration;

/// <summary>
/// Налаштування ініціалізації бази даних
/// Демонстрація Options Pattern для strongly-typed configuration
/// </summary>
public class DatabaseInitializationOptions
{
    public const string SectionName = "EntertainmentSettings:DatabaseInitialization";

    /// <summary>
    /// Чи автоматично застосовувати міграції при старті
    /// </summary>
    public bool AutoMigrate { get; set; } = true;

    /// <summary>
    /// Чи видаляти базу даних при зупинці додатку
    /// </summary>
    public bool DeleteOnShutdown { get; set; } = false;
}

/// <summary>
/// Налаштування функціональності додатку
/// </summary>
public class FeaturesOptions
{
    public const string SectionName = "Features";

    /// <summary>
    /// Відображати касові збори фільмів
    /// </summary>
    public bool ShowBoxOffice { get; set; } = true;

    /// <summary>
    /// Максимальна кількість результатів у таблиці
    /// </summary>
    public int MaxResultsPerPage { get; set; } = 50;

    /// <summary>
    /// Увімкнути кешування результатів
    /// </summary>
    public bool EnableCaching { get; set; } = true;

    /// <summary>
    /// Час життя кешу в секундах
    /// </summary>
    public int CacheExpirationSeconds { get; set; } = 300;
}

/// <summary>
/// Validator для DatabaseInitializationOptions
/// </summary>
public class DatabaseInitializationOptionsValidator : IValidateOptions<DatabaseInitializationOptions>
{
    public ValidateOptionsResult Validate(string? name, DatabaseInitializationOptions options)
    {
        if (options.DeleteOnShutdown && !options.AutoMigrate)
        {
            return ValidateOptionsResult.Fail(
                "Якщо DeleteOnShutdown=true, то AutoMigrate також повинен бути true");
        }

        return ValidateOptionsResult.Success;
    }
}

/// <summary>
/// Приклад використання Options в сервісі
/// </summary>
public class ConfigurableQueryService : IQueryService
{
    private readonly EntertainmentDbContext _context;
    private readonly IOptionsMonitor<FeaturesOptions> _featuresOptions;
    private readonly ILogger<ConfigurableQueryService> _logger;

    public string QueryName => "Конфігурований запит (Options Pattern)";
    public string QueryKey => "configurable-query";

    public ConfigurableQueryService(
        EntertainmentDbContext context,
        IOptionsMonitor<FeaturesOptions> featuresOptions,
        ILogger<ConfigurableQueryService> logger)
    {
        _context = context;
        _featuresOptions = featuresOptions;
        _logger = logger;

        // Підписка на зміни конфігурації в runtime
        _featuresOptions.OnChange(options =>
        {
            _logger.LogInformation("Конфігурація змінилась: MaxResults={MaxResults}, Caching={Caching}",
                options.MaxResultsPerPage,
                options.EnableCaching);
        });
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Отримуємо поточні налаштування
        var options = _featuresOptions.CurrentValue;

        _logger.LogInformation(
            "Виконання запиту з налаштуваннями: MaxResults={MaxResults}, ShowBoxOffice={ShowBoxOffice}",
            options.MaxResultsPerPage,
            options.ShowBoxOffice);

        var movies = _context.Movies
            .OrderByDescending(x => x.WorldwideBoxOfficeGross)
            .Take(options.MaxResultsPerPage);

        var moviesList = await movies.ToListAsync(cancellationToken);

        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Blue)
            .AddColumn("Назва")
            .AddColumn("Дата");

        if (options.ShowBoxOffice)
        {
            table.AddColumn("Каса");
        }

        foreach (var movie in moviesList)
        {
            var row = new List<string>
            {
                $"[red]{movie.Name}[/]",
                $"[fuchsia]{movie.Release:d}[/]"
            };

            if (options.ShowBoxOffice)
            {
                row.Add($"[green]{movie.WorldwideBoxOfficeGross:C}[/]");
            }

            table.AddRow(row.ToArray());
        }

        AnsiConsole.MarkupLine(
            "[yellow]ℹ️ Налаштування:[/] MaxResults={0}, Caching={1}",
            options.MaxResultsPerPage,
            options.EnableCaching ? "Увімкнено" : "Вимкнено");

        Output.Results(movies, "Конфігурований запит (Options Pattern)", table);
    }
}