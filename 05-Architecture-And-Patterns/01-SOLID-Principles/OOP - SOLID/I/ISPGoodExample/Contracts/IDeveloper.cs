using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPGoodExample.Contracts
{
    // Інтерфейс для розробників
    public interface IDeveloper
    {
        void WriteCode();
        void ReviewCode();
        void DebugCode();
    }
}
