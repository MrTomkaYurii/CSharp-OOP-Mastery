using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPBadExample
{
    // ПОГАНИЙ ПРИКЛАД: Програміст змушений реалізовувати ВСІ методи товстого інтерфейсу
    public class ProgrammerBad : IWorkerBad
    {
        private string _name;

        public ProgrammerBad(string name)
        {
            _name = name;
        }

        // Методи, які програміст реально використовує
        public void Work()
        {
            Console.WriteLine($"[{_name}] Працюю над проєктом...");
        }

        public void WriteCode()
        {
            Console.WriteLine($"[{_name}] Пишу код...");
        }

        public void ReviewCode()
        {
            Console.WriteLine($"[{_name}] Переглядаю код колег...");
        }

        public void DebugCode()
        {
            Console.WriteLine($"[{_name}] Виправляю баги...");
        }

        public void UseComputer()
        {
            Console.WriteLine($"[{_name}] Працюю за комп'ютером");
        }

        public void EatLunch()
        {
            Console.WriteLine($"[{_name}] Обідаю в їдальні");
        }

        public void ReceiveSalary()
        {
            Console.WriteLine($"[{_name}] Отримую заробітну плату");
        }

        // Непрофільні методи, які програмісту не потрібні, але контракт змушує їх оголошувати:
        public void AttendMeeting()
        {
            throw new NotImplementedException($"{_name} не ходить на зустрічі");
        }

        public void SendEmail()
        {
            throw new NotImplementedException($"{_name} не відправляє email");
        }

        public void TakeCoffeeBreak()
        {
            throw new NotImplementedException($"{_name} не бере кавові перерви");
        }

        public void ManageTeam()
        {
            throw new NotImplementedException($"{_name} не управляє командою");
        }

        public void ConductPerformanceReview()
        {
            throw new NotImplementedException($"{_name} не проводить оцінку продуктивності");
        }

        public void ApproveBudget()
        {
            throw new NotImplementedException($"{_name} не затверджує бюджет");
        }

        public void WorkNightShift()
        {
            throw new NotImplementedException($"{_name} не працює в нічну зміну");
        }

        public void WorkWeekends()
        {
            throw new NotImplementedException($"{_name} не працює по вихідних");
        }
    }
}
