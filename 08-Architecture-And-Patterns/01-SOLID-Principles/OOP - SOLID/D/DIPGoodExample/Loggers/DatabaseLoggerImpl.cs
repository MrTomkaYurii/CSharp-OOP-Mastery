using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Loggers
{

    // Реалізація 3: Логування в базу даних
    public class DatabaseLoggerImpl : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[DB LOGGER] INSERT INTO logs (message, timestamp) VALUES ('{message}', '{DateTime.Now}')");
        }
    }
}
