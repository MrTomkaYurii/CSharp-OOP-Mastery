using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{

    public class SparrowGood : BirdGood, IFlyable
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Горобець їсть зернятка та комах");
        }

        public override void MakeSound()
        {
            Console.WriteLine($"[{Name}] Цвірінькає: чик-чирик!");
        }

        public void Fly()
        {
            Console.WriteLine($"[{Name}] Горобець літає швидкими змахами крил");
        }

        public int GetMaxAltitude()
        {
            return 100; // 100 метрів
        }
    }
}
