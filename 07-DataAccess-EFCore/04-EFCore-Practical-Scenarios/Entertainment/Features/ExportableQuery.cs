using Entertainment.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Запит з експортом результатів
    /// </summary>
    public class ExportableQuery : IQueryService
    {
        private readonly EntertainmentDbContext _context;
        private readonly IExportService _exportService;
        private readonly ILogger<ExportableQuery> _logger;

        public string QueryName => "Експорт фільмів до CSV";
        public string QueryKey => "export-movies";

        public ExportableQuery(
            EntertainmentDbContext context,
            IExportService exportService,
            ILogger<ExportableQuery> logger)
        {
            _context = context;
            _exportService = exportService;
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Підготовка даних для експорту");

            var movies = _context.Movies
                .Select(m => new
                {
                    m.Name,
                    m.Release,
                    m.WorldwideBoxOfficeGross
                });

            var moviesList = await movies.ToListAsync(cancellationToken);
            var filename = $"movies_export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            await _exportService.ExportToCsvAsync(moviesList, filename);

            AnsiConsole.MarkupLine(
                "[green]✓[/] Експортовано [yellow]{0}[/] фільмів до файлу [cyan]{1}[/]",
                moviesList.Count,
                filename);
        }
    }
}
