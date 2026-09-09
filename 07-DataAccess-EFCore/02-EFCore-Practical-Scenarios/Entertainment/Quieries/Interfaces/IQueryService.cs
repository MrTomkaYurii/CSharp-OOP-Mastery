using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quieries.Interfaces
{
    /// <summary>
    /// Базовий інтерфейс для всіх сервісів запитів до бази даних
    /// </summary>
    public interface IQueryService
    {
        /// <summary>
        /// Назва запиту для відображення в меню
        /// </summary>
        string QueryName { get; }

        /// <summary>
        /// Унікальний ключ запиту
        /// </summary>
        string QueryKey { get; }

        /// <summary>
        /// Виконує запит та виводить результати
        /// </summary>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
