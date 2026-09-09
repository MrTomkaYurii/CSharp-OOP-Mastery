using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entertainment.Features
{
    public class PerformanceMonitor : IPerformanceMonitor
    {
        private readonly ILogger<PerformanceMonitor> _logger;

        public PerformanceMonitor(ILogger<PerformanceMonitor> logger)
        {
            _logger = logger;
        }

        public IDisposable BeginOperation(string operationName)
        {
            return new PerformanceOperation(operationName, _logger);
        }

        private class PerformanceOperation : IDisposable
        {
            private readonly string _operationName;
            private readonly ILogger _logger;
            private readonly Stopwatch _stopwatch;

            public PerformanceOperation(string operationName, ILogger logger)
            {
                _operationName = operationName;
                _logger = logger;
                _stopwatch = Stopwatch.StartNew();
                _logger.LogDebug("Початок операції: {OperationName}", _operationName);
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _logger.LogInformation(
                    "Операція {OperationName} виконана за {ElapsedMs}мс",
                    _operationName,
                    _stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
