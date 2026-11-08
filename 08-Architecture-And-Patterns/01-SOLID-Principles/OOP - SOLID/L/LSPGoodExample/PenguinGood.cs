using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{
    public class PenguinGood : BirdGood, ISwimmable
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Пінгвін їсть рибу та кальмарів");
        }

        public override void MakeSound()
        {
            Console.WriteLine($"[{Name}] Кричить: га-га-га!");
        }

        public void Swim()
        {
            Console.WriteLine($"[{Name}] Пінгвін швидко плаває під водою (до 35 км/год)");
        }

        // Немає методу Fly() - пінгвін реалізує контракт плавання ISwimmable, а не IFlyable.
    }
}
