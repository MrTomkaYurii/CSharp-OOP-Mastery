using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Loggers
{
    // Реалізація 2: Логування в консоль
    public class ConsoleLoggerImpl : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[CONSOLE LOGGER] [{DateTime.Now:HH:mm:ss}] {message}");
        }
    }

}
