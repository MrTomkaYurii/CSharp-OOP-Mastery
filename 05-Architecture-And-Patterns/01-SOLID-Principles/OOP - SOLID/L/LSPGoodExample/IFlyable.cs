using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{
    // Інтерфейс для птахів, які МОЖУТЬ літати
    public interface IFlyable
    {
        void Fly();
        int GetMaxAltitude();
    }
}
