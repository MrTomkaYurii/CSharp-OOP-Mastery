using OOP___SOLID.I.ISPGoodExample.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPGoodExample
{
    // ДОБРИЙ ПРИКЛАД: Програміст реалізує ТІЛЬКИ ті інтерфейси, які відповідають його професійній ролі
    public class ProgrammerGood : IWorkable, IDeveloper, IComputerUser, IEatable, ISalaried
    {
        private string _name;

        public ProgrammerGood(string name)
        {
            _name = name;
        }

        // IWorkable
        public void Work()
        {
            Console.WriteLine($"[{_name}] Працюю над проєктом...");
        }

        // IDeveloper
        public void WriteCode()
        {
            Console.WriteLine($"[{_name}] Пишу код на C#...");
        }

        public void ReviewCode()
        {
            Console.WriteLine($"[{_name}] Переглядаю pull request колеги...");
        }

        public void DebugCode()
        {
            Console.WriteLine($"[{_name}] Виправляю дефекти у коді...");
        }

        // IComputerUser
        public void UseComputer()
        {
            Console.WriteLine($"[{_name}] Працюю за робочою станцією");
        }

        // IEatable
        public void EatLunch()
        {
            Console.WriteLine($"[{_name}] Обідаю");
        }

        public void TakeCoffeeBreak()
        {
            Console.WriteLine($"[{_name}] Роблю коротку перерву");
        }

        // ISalaried
        public void ReceiveSalary()
        {
            Console.WriteLine($"[{_name}] Отримую заробітну плату інженера");
        }
    }

}
