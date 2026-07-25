using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample.Repositories
{
    // Реалізація 3: MongoDB база даних
    public class MongoUserRepository : IUserRepository
    {
        public void Save(string userId, string userData)
        {
            Console.WriteLine($"[MONGO REPO] Підключення до MongoDB (localhost:27017)");
            Console.WriteLine($"[MONGO REPO] db.users.insertOne({{ _id: '{userId}', data: '{userData}' }})");
        }
    }
}
