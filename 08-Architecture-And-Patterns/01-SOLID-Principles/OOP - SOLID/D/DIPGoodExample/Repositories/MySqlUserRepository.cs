using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Repositories
{
    // Реалізація 1: MySQL база даних
    public class MySqlUserRepository : IUserRepository
    {
        public void Save(string userId, string userData)
        {
            Console.WriteLine($"[MYSQL REPO] Підключення до MySQL (localhost:3306)");
            Console.WriteLine($"[MYSQL REPO] INSERT INTO users VALUES ('{userId}', '{userData}')");
        }
    }

}
