using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPBadExample
{
    // Звичайний горобець - все працює
    public class SparrowBad : BirdBad
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Горобець їсть зернятка");
        }

        public override void Fly()
        {
            Console.WriteLine($"[{Name}] Горобець літає на висоті 50м");
        }
    }
}
