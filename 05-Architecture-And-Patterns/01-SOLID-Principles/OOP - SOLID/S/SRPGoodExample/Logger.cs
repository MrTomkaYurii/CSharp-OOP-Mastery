using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S.SRPGoodExample
{
    // 7. Логгер - відповідає ЛИШЕ за логування
    public class Logger
    {
        public void Log(string message)
        {
            Console.WriteLine($"\n[ЛОГ] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
        }
    }
}
