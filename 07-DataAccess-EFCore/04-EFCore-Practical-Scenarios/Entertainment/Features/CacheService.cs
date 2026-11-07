using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entertainment.Features
{
    public class CacheService : ICacheService
    {
        private readonly Dictionary<string, object> _cache = new();
        private readonly ILogger<CacheService> _logger;

        public CacheService(ILogger<CacheService> logger)
        {
            _logger = logger;
        }

        public void Set<T>(string key, T value)
        {
            _cache[key] = value!;
            _logger.LogDebug("Додано до кешу: {Key}", key);
        }

        public T? Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                _logger.LogDebug("Знайдено в кеші: {Key}", key);
                return (T)value;
            }

            _logger.LogDebug("Не знайдено в кеші: {Key}", key);
            return default;
        }

        public void Clear()
        {
            _cache.Clear();
            _logger.LogInformation("Кеш очищено");
        }
    }
}
