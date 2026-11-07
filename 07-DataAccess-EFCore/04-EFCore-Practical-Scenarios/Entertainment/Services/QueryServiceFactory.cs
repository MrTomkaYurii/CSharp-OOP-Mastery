using Microsoft.Extensions.DependencyInjection;
using Quieries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.Services;

/// <summary>
/// Метадані про запит (для відображення в меню без створення екземплярів)
/// </summary>
public class QueryServiceMetadata
{
    public required string QueryName { get; init; }
    public required string QueryKey { get; init; }
    public required Type ServiceType { get; init; }
}

/// <summary>
/// Фабрика для створення сервісів запитів через DI
/// </summary>
public class QueryServiceFactory : IQueryServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<QueryServiceMetadata> _queryMetadata;

    public QueryServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        // Отримуємо типи всіх зареєстрованих IQueryService один раз при ініціалізації
        _queryMetadata = GetRegisteredQueryServices();
    }

    /// <summary>
    /// Отримує метадані про всі зареєстровані запити
    /// </summary>
    private List<QueryServiceMetadata> GetRegisteredQueryServices()
    {
        var metadata = new List<QueryServiceMetadata>();

        // Створюємо тимчасовий scope ТІЛЬКИ для отримання метаданих
        using var scope = _serviceProvider.CreateScope();
        var services = scope.ServiceProvider.GetServices<IQueryService>();

        foreach (var service in services)
        {
            metadata.Add(new QueryServiceMetadata
            {
                QueryName = service.QueryName,
                QueryKey = service.QueryKey,
                ServiceType = service.GetType()
            });
        }

        return metadata;
    }

    /// <summary>
    /// Повертає метадані для відображення в меню (БЕЗ створення екземплярів)
    /// </summary>
    public IEnumerable<QueryServiceMetadata> GetAllQueryMetadata()
    {
        return _queryMetadata;
    }

    /// <summary>
    /// Створює НОВИЙ scope, виконує запит, і dispose scope після виконання
    /// </summary>
    public async Task ExecuteQueryAsync(string key, CancellationToken cancellationToken = default)
    {
        // Створюємо НОВИЙ scope для виконання запиту
        using var scope = _serviceProvider.CreateScope();

        // Отримуємо всі IQueryService з ЦЬОГО scope
        var services = scope.ServiceProvider.GetServices<IQueryService>();

        // Знаходимо потрібний сервіс
        var service = services.FirstOrDefault(q =>
            q.QueryKey.Equals(key, StringComparison.OrdinalIgnoreCase));

        if (service == null)
        {
            throw new InvalidOperationException($"Query service з ключем '{key}' не знайдено");
        }

        // Виконуємо запит
        await service.ExecuteAsync(cancellationToken);

        // Scope автоматично dispose тут (включно з DbContext)
    }
}