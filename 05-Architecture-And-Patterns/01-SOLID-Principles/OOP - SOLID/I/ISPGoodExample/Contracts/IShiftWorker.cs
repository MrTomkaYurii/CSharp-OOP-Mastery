using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPGoodExample.Contracts
{

    // Інтерфейс для працівників зі змінним графіком
    public interface IShiftWorker
    {
        void WorkNightShift();
        void WorkWeekends();
    }
}
