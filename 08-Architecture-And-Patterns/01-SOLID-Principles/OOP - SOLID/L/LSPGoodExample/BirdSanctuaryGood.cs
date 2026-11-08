using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{
    public class BirdSanctuaryGood
    {
        // Метод працює з УСІМА птахами (базовий клас)
        public void FeedBirds(List<BirdGood> birds)
        {
            Console.WriteLine("[ЗАПОВІДНИК] Годування птахів (поліморфний виклик):\n");

            foreach (var bird in birds)
            {
                bird.Eat();
                bird.MakeSound();
                Console.WriteLine();
            }
        }

        // Метод працює ТІЛЬКИ з літаючими птахами (інтерфейс IFlyable)
        public void MakeFlyableBirdsFly(List<IFlyable> flyingBirds)
        {
            Console.WriteLine("[ЗАПОВІДНИК] Демонстрація польоту літаючих птахів:\n");

            foreach (var bird in flyingBirds)
            {
                bird.Fly();
                Console.WriteLine($"   Максимальна висота: {bird.GetMaxAltitude()}м");
                Console.WriteLine();
            }
        }

        // Метод працює ТІЛЬКИ з плаваючими птахами
        public void MakeSwimmableBirdsSwim(List<ISwimmable> swimmingBirds)
        {
            Console.WriteLine("[ЗАПОВІДНИК] Демонстрація плавання плаваючих птахів:\n");

            foreach (var bird in swimmingBirds)
            {
                bird.Swim();
                Console.WriteLine();
            }
        }

        // Можна безпечно передати БУДЬ-ЯКОГО птаха - спільний контракт гарантовано виконується
        public void ShowBirdInfo(BirdGood bird)
        {
            Console.WriteLine($"\n[ПАСПОРТ ПТАХА]:");
            Console.WriteLine(new string('─', 40));
            Console.WriteLine($"Ім'я: {bird.Name}");
            Console.Write($"Вміння: їжа, звуки");

            if (bird is IFlyable)
                Console.Write(", політ");
            if (bird is ISwimmable)
                Console.Write(", плавання");

            Console.WriteLine();
            Console.WriteLine(new string('─', 40));

            bird.Eat();
            bird.MakeSound();

            if (bird is IFlyable flyable)
            {
                flyable.Fly();
                Console.WriteLine($"Висота польоту: {flyable.GetMaxAltitude()}м");
            }

            if (bird is ISwimmable swimmable)
            {
                swimmable.Swim();
            }

            if (bird is OstrichGood ostrich)
            {
                ostrich.Run();
            }

            if (bird is KiwiGood kiwi)
            {
                kiwi.Dig();
            }
        }
    }
}
