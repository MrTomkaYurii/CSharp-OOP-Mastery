using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPBadExample
{
    // Конкретна реалізація бази даних MySQL
    public class MySqlDatabase
    {
        public void SaveToMySQL(string userId, string data)
        {
            Console.WriteLine($"[MYSQL] Підключення до MySQL Server (localhost:3306)...");
            Console.WriteLine($"[MYSQL] Використання бази даних: production_db");
            Console.WriteLine($"[MYSQL] INSERT INTO users (id, data) VALUES ('{userId}', '{data}')");
            Console.WriteLine($"[MYSQL] Запис збережено успішно");
        }
    }
}
