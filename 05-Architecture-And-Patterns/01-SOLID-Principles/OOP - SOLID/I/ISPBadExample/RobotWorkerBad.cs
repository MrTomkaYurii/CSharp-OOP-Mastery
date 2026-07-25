using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPBadExample
{
    // ПОГАНИЙ ПРИКЛАД: Робот змушений реалізовувати методи, які до нього не застосовні
    public class RobotWorkerBad : IWorkerBad
    {
        private string _model;

        public RobotWorkerBad(string model)
        {
            _model = model;
        }

        // Методи, які робот може виконувати
        public void Work()
        {
            Console.WriteLine($"[Робот {_model}] Виконую виробниче завдання...");
        }

        public void WorkNightShift()
        {
            Console.WriteLine($"[Робот {_model}] Працюю в нічну зміну без перерви");
        }

        public void WorkWeekends()
        {
            Console.WriteLine($"[Робот {_model}] Працюю по вихідних у безперервному циклі");
        }

        // Роботи не їдять, не п'ють каву, не отримують заробітну плату:
        public void EatLunch()
        {
            throw new NotImplementedException("Робот не їсть!");
        }

        public void TakeCoffeeBreak()
        {
            throw new NotImplementedException("Робот не п'є каву!");
        }

        public void ReceiveSalary()
        {
            throw new NotImplementedException("Робот не отримує зарплату!");
        }

        public void UseComputer()
        {
            throw new NotImplementedException("Робот сам є комп'ютером!");
        }

        public void AttendMeeting()
        {
            throw new NotImplementedException("Робот не ходить на зустрічі!");
        }

        public void SendEmail()
        {
            throw new NotImplementedException("Робот не відправляє email!");
        }

        public void ManageTeam()
        {
            throw new NotImplementedException("Робот не управляє командою!");
        }

        public void ConductPerformanceReview()
        {
            throw new NotImplementedException("Робот не проводить оцінку!");
        }

        public void ApproveBudget()
        {
            throw new NotImplementedException("Робот не затверджує бюджет!");
        }

        public void WriteCode()
        {
            throw new NotImplementedException("Робот не пише код!");
        }

        public void ReviewCode()
        {
            throw new NotImplementedException("Робот не переглядає код!");
        }

        public void DebugCode()
        {
            throw new NotImplementedException("Робот не дебажить!");
        }
    }

}
