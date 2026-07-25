using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPGoodExample.Contracts
{
    // Інтерфейс для тих, хто потребує харчування
    public interface IEatable
    {
        void EatLunch();
        void TakeCoffeeBreak();
    }
}
