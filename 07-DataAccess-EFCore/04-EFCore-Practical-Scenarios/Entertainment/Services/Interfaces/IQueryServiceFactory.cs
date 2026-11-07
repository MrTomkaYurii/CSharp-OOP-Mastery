using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.Services
{
    /// <summary>
    /// Інтерфейс фабрики для створення сервісів запитів
    /// </summary>
    public interface IQueryServiceFactory
    {
        /// <summary>
        /// Отримує метадані всіх доступних сервісів запитів (без створення екземплярів)
        /// </summary>
        IEnumerable<QueryServiceMetadata> GetAllQueryMetadata();

        /// <summary>
        /// Створює scope і виконує запит
        /// </summary>
        Task ExecuteQueryAsync(string key, CancellationToken cancellationToken = default);
    }
}
