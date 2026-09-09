# Наскрізна функціональність: Кешування, Експорт, Профайлінг та TUI-відображення

Реальні корпоративні рішення вимагають реалізації наскрізних функцій (Cross-Cutting Concerns), які не є частиною бізнес-домену, але забезпечують надійність та зручність користувача.

---

## 1. Патерн Декоратор для кешування (`CachedMoviesQuery`)

Замість внесення логіки кешування в сам сервіс запитів, використовується патерн Декоратор або проксі:

```csharp
public class CachedMoviesQuery : IQueryService
{
    private readonly IQueryService _inner;
    private readonly ICacheService _cache;

    public CachedMoviesQuery(IQueryService inner, ICacheService cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<List<MovieDto>> GetMoviesAsync()
    {
        const string cacheKey = "all_movies";
        var cached = _cache.Get<List<MovieDto>>(cacheKey);
        if (cached != null)
        {
            return cached;
        }

        var result = await _inner.GetMoviesAsync();
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }
}
```

---

## 2. Моніторинг продуктивності (`PerformanceMonitor`)

Вимірювання тривалості виконання запитів з логуванням попереджень при перевищенні порогового часу:

```csharp
public async Task<T> MeasureAsync<T>(string operationName, Func<Task<T>> operation)
{
    var sw = Stopwatch.StartNew();
    try
    {
        return await operation();
    }
    finally
    {
        sw.Stop();
        if (sw.ElapsedMilliseconds > 200)
        {
            _logger.LogWarning("Повільний запит [{Operation}] тривав {Elapsed}ms", operationName, sw.ElapsedMilliseconds);
        }
        else
        {
            _logger.LogDebug("Запит [{Operation}] виконано за {Elapsed}ms", operationName, sw.ElapsedMilliseconds);
        }
    }
}
```

---

## 3. Сервіс експорту результатів (`ExportService`)

Дозволяє зберігати вибірки з бази даних у форматах JSON або CSV:

```csharp
public async Task ExportToJsonAsync<T>(string filePath, IEnumerable<T> data)
{
    var options = new JsonSerializerOptions { WriteIndented = true };
    await using var fs = File.Create(filePath);
    await JsonSerializer.SerializeAsync(fs, data, options);
}
```

---

## 4. Двопанельний інтерфейс у `Spectre.Console`

Клас `Output.cs` будує двопанельну розмітку термінала (`Layout`):
- **Верхня панель**: інтерактивне навігаційне меню та статус сервісів.
- **Нижня панель**: детальні таблиці (`Table`) з результатами виконання запитів до бази даних, підсвічуванням полів та статистикою вибірки.
