using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entertainment.Features
{
    /// <summary>
    /// Сервіс для моніторингу продуктивності запитів
    /// </summary>
    public interface IPerformanceMonitor
    {
        IDisposable BeginOperation(string operationName);
    }
}
