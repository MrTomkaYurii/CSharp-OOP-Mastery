using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPBadExample
{
    // Універсальний інтерфейс для всіх типів працівників
    public interface IWorkerBad
    {
        // Методи для офісних працівників
        void Work();
        void AttendMeeting();
        void SendEmail();
        void UseComputer();

        // Методи для працівників, які їдять на роботі
        void EatLunch();
        void TakeCoffeeBreak();

        // Методи для працівників, які отримують зарплату
        void ReceiveSalary();

        // Методи для менеджерів
        void ManageTeam();
        void ConductPerformanceReview();
        void ApproveBudget();

        // Методи для програмістів
        void WriteCode();
        void ReviewCode();
        void DebugCode();

        // Методи для працівників зі змінним графіком
        void WorkNightShift();
        void WorkWeekends();
    }
}
