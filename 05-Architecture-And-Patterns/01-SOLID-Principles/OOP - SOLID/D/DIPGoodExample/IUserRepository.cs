using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.D.DIPGoodExample
{
    // Абстракція для роботи з базою даних
    public interface IUserRepository
    {
        void Save(string userId, string userData);
    }
}
