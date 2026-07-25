using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPBadExample
{
    // Конкретна реалізація логування у файл
    public class FileLogger
    {
        public void LogToFile(string message)
        {
            Console.WriteLine($"[FILE LOG] Запис у файл: {message}");
            Console.WriteLine($"[FILE LOG] Шлях: C:\\logs\\app_{DateTime.Now:yyyyMMdd}.log");
        }
    }
}
