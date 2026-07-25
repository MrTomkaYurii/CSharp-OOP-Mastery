using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Repositories
{
    // Реалізація 2: PostgreSQL база даних
    public class PostgresUserRepository : IUserRepository
    {
        public void Save(string userId, string userData)
        {
            Console.WriteLine($"[POSTGRES REPO] Підключення до PostgreSQL (localhost:5432)");
            Console.WriteLine($"[POSTGRES REPO] INSERT INTO users VALUES ('{userId}', '{userData}')");
        }
    }
}
