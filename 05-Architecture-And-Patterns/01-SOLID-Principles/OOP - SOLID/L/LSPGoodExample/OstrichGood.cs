using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{

    public class OstrichGood : BirdGood
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Страус їсть рослини, насіння та комах");
        }

        public override void MakeSound()
        {
            Console.WriteLine($"[{Name}] Ревіт: бу-бу-бу!");
        }

        public void Run()
        {
            Console.WriteLine($"[{Name}] Страус біжить зі швидкістю 70 км/год!");
        }

        // Немає методу Fly() - страус не літає, тому інтерфейс IFlyable не реалізується.
    }
}
