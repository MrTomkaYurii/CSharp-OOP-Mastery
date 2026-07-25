using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPGoodExample.Contracts
{
    // Інтерфейс для менеджерів
    public interface IManager
    {
        void ManageTeam();
        void ConductPerformanceReview();
        void ApproveBudget();
    }
}
