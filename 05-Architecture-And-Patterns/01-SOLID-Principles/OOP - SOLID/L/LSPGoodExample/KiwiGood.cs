using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{
    public class KiwiGood : BirdGood
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Ківі шукає їжу дзьобом у землі");
        }

        public override void MakeSound()
        {
            Console.WriteLine($"[{Name}] Свистить: ківі-ківі!");
        }

        public void Dig()
        {
            Console.WriteLine($"[{Name}] Ківі риє землю в пошуках червів");
        }

        // Немає методу Fly() - ківі не літає, тому інтерфейс IFlyable не реалізується.
    }

}
