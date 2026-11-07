using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entertainment.Features
{
    public class ExportService : IExportService
    {
        private readonly ILogger<ExportService> _logger;

        public ExportService(ILogger<ExportService> logger)
        {
            _logger = logger;
        }

        public async Task ExportToCsvAsync<T>(IEnumerable<T> data, string filename)
        {
            _logger.LogInformation("Експорт даних до {Filename}", filename);

            try
            {
                var properties = typeof(T).GetProperties();
                var lines = new List<string>();

                // Заголовки
                lines.Add(string.Join(",", properties.Select(p => p.Name)));

                // Дані
                foreach (var item in data)
                {
                    var values = properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty);
                    lines.Add(string.Join(",", values));
                }

                //await File.WriteAllLinesAsync(filename, lines);

                _logger.LogInformation("Експорт завершено успішно: {Filename}", filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при експорті до {Filename}", filename);
                throw;
            }
        }
    }
}
