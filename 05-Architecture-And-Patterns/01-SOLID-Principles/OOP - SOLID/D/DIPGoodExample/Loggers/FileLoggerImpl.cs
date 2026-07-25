using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Loggers
{
    // Реалізація 1: Логування у файл
    public class FileLoggerImpl : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[FILE LOGGER] {message}");
            Console.WriteLine($"[FILE LOGGER] -> C:\\logs\\app_{DateTime.Now:yyyyMMdd}.log");
        }
    }
}
