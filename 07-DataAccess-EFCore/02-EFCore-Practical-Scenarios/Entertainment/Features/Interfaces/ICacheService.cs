using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entertainment.Features
{
    /// <summary>
    /// Сервіс для кешування результатів запитів (демонстрація Singleton)
    /// </summary>
    public interface ICacheService
    {
        void Set<T>(string key, T value);
        T? Get<T>(string key);
        void Clear();
    }
}
