using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPBadExample
{
    // Орел - все працює
    public class EagleBad : BirdBad
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Орел полює на здобич");
        }

        public override void Fly()
        {
            Console.WriteLine($"[{Name}] Орел ширяє на висоті 3000м");
        }
    }
}
