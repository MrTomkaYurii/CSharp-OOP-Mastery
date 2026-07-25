using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{
    public class ParrotGood : BirdGood, IFlyable
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Папуга їсть фрукти та горіхи");
        }

        public override void MakeSound()
        {
            Console.WriteLine($"[{Name}] Каже: Привіт! Я папуга!");
        }

        public void Fly()
        {
            Console.WriteLine($"[{Name}] Папуга яскраво літає між деревами");
        }

        public int GetMaxAltitude()
        {
            return 500; // 500 метрів
        }
    }
}
