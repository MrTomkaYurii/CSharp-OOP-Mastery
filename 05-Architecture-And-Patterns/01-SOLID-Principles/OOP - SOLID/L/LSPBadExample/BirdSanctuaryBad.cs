using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPBadExample
{
    // Клас, який працює з птахами
    public class BirdSanctuaryBad
    {
        public void MakeBirdsFly(List<BirdBad> birds)
        {
            Console.WriteLine("[ЗАПОВІДНИК] Спроба підняти всіх птахів у повітря:\n");

            foreach (var bird in birds)
            {
                try
                {
                    bird.Fly();
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine($"   [ПОМИЛКА] {ex.Message}");
                }
            }
        }

        public void FeedBirds(List<BirdBad> birds)
        {
            Console.WriteLine("\n[ЗАПОВІДНИК] Годування птахів:\n");

            foreach (var bird in birds)
            {
                bird.Eat();
            }
        }
    }
}
