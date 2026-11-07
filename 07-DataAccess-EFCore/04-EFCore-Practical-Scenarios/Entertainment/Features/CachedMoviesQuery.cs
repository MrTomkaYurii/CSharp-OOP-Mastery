using Entertainment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quieries.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.Features
{
    /// <summary>
    /// Приклад запиту з використанням кешування та моніторингу продуктивності
    /// </summary>
    public class CachedMoviesQuery : IQueryService
    {
        private readonly EntertainmentDbContext _context;
        private readonly ICacheService _cache;
        private readonly IPerformanceMonitor _performanceMonitor;
        private readonly ILogger<CachedMoviesQuery> _logger;
        private readonly IConfiguration _configuration;

        public string QueryName => "Кешовані фільми (з моніторингом)";
        public string QueryKey => "cached-movies";

        public CachedMoviesQuery(
            EntertainmentDbContext context,
            ICacheService cache,
            IPerformanceMonitor performanceMonitor,
            ILogger<CachedMoviesQuery> logger,
            IConfiguration configuration)
        {
            _context = context;
            _cache = cache;
            _performanceMonitor = performanceMonitor;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var _ = _performanceMonitor.BeginOperation("CachedMoviesQuery");

            const string cacheKey = "all_movies";

            // Спроба отримати з кешу
            var movies = _cache.Get<List<Movie>>(cacheKey);

            if (movies == null)
            {
                _logger.LogInformation("Дані не знайдено в кеші, виконуємо запит до БД");

                movies = await _context.Movies
                    .OrderByDescending(x => x.WorldwideBoxOfficeGross)
                    .ToListAsync(cancellationToken);

                _cache.Set(cacheKey, movies);
            }
            else
            {
                _logger.LogInformation("Дані отримано з кешу");
            }

            // Отримуємо налаштування з конфігурації
            var showBoxOffice = _configuration.GetValue<bool>("Features:ShowBoxOffice", true);

            // Створюємо IQueryable для Output.Results
            var moviesQuery = _context.Movies.OrderByDescending(x => x.WorldwideBoxOfficeGross);

            // Виводимо результати
            var table = new Table()
                .RoundedBorder()
                .BorderColor(Color.Blue3_1)
                .AddColumn("Назва")
                .AddColumn("Дата виходу");

            if (showBoxOffice)
            {
                table.AddColumn("Каса ($)");
            }

            // Формуємо рядки таблиці
            foreach (var movie in movies)
            {
                var row = new List<string>
            {
                $"[red]{movie.Name}[/]",
                $"[fuchsia]{movie.Release:d}[/]"
            };

                if (showBoxOffice)
                {
                    row.Add($"[green]{movie.WorldwideBoxOfficeGross:C}[/]");
                }

                table.AddRow(row.ToArray());
            }

            AnsiConsole.MarkupLine("[yellow]📦 Статус: {0}[/]",
                movies != null && _cache.Get<List<Movie>>(cacheKey) != null ? "З кешу" : "З БД");

            Output.Results(moviesQuery, "Кешовані фільми (з моніторингом)", table);
        }
    }
}
