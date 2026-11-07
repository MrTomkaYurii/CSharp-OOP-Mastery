using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.Services;

/// <summary>
/// Інтерфейс сервісу меню
/// </summary>
public interface IMenuService
{
    Task RunAsync(CancellationToken cancellationToken = default);
}