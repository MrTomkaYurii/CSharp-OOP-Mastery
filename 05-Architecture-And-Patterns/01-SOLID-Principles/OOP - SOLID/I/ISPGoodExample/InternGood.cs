using OOP___SOLID.I.ISPGoodExample.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPGoodExample
{
    // ДОБРИЙ ПРИКЛАД: Інтерн реалізує контракти роботи, розробки, використання ПК та обіду (без ISalaried)
    public class InternGood : IWorkable, IDeveloper, IComputerUser, IEatable
    {
        private string _name;

        public InternGood(string name)
        {
            _name = name;
        }

        public void Work()
        {
            Console.WriteLine($"[{_name}] Вчуся та допомагаю команді...");
        }

        public void WriteCode()
        {
            Console.WriteLine($"[{_name}] Пишу код під наглядом ментора");
        }

        public void ReviewCode()
        {
            Console.WriteLine($"[{_name}] Вивчаю код досвідчених розробників");
        }

        public void DebugCode()
        {
            Console.WriteLine($"[{_name}] Досліджую причини виникнення багів");
        }

        public void UseComputer()
        {
            Console.WriteLine($"[{_name}] Користуюся робочим ноутбуком");
        }

        public void EatLunch()
        {
            Console.WriteLine($"[{_name}] Обідаю разом із командою");
        }

        public void TakeCoffeeBreak()
        {
            Console.WriteLine($"[{_name}] Спілкуюся з колегами під час перерви");
        }

        // Не реалізує ISalaried - інтерн проходить навчальну практику без штатного окладу.
    }
}
