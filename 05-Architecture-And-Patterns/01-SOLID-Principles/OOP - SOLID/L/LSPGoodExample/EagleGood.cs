using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{
    public class EagleGood : BirdGood, IFlyable
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Орел полює на здобич з висоти");
        }

        public override void MakeSound()
        {
            Console.WriteLine($"[{Name}] Кричить: кііі-кііі!");
        }

        public void Fly()
        {
            Console.WriteLine($"[{Name}] Орел могутньо ширяє в небі");
        }

        public int GetMaxAltitude()
        {
            return 4000; // 4000 метрів
        }
    }

}
