using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entertainment.Features
{
    /// <summary>
    /// Сервіс для експорту даних (демонстрація додаткового функціоналу)
    /// </summary>
    public interface IExportService
    {
        Task ExportToCsvAsync<T>(IEnumerable<T> data, string filename);
    }
}
