using OOP___SOLID.L.LSPGoodExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L
{
    public class LspGoodExampleCommand : IMenuCommand
    {
        public string Title => "LSP - ГАРНИЙ приклад (Дотримання принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  LSP - ГАРНИЙ ПРИКЛАД: Правильна підстановка підтипів      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПЕРЕВАГИ цього підходу:");
            Console.WriteLine("[+] BirdGood містить ТІЛЬКИ спільну поведінку всіх птахів:");
            Console.WriteLine("   • Eat() - всі птахи їдять");
            Console.WriteLine("   • MakeSound() - всі птахи видають звуки\n");

            Console.WriteLine("[+] Окремий інтерфейс IFlyable для літаючих птахів");
            Console.WriteLine("[+] Окремий інтерфейс ISwimmable для плаваючих птахів");
            Console.WriteLine("[+] Пінгвін та Страус НЕ містять методу Fly() - він їм не властивий");
            Console.WriteLine("[+] Жодних неочікуваних винятків NotSupportedException!");
            Console.WriteLine("[+] БУДЬ-ЯКОГО нащадка можна безпечно передати туди, де очікується BirdGood");
            Console.WriteLine("[+] Суворе дотримання LSP - підтипи коректно замінюють базовий тип без сюрпризів");
            Console.WriteLine("[+] Гармонійне поєднання з ISP - клієнти залежать лише від цільових контрактів\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            // Всі птахи
            var allBirds = new List<BirdGood>
            {
                new SparrowGood { Name = "Сіренький" },
                new EagleGood { Name = "Орлик" },
                new ParrotGood { Name = "Кеша" },
                new PenguinGood { Name = "Пінгвінчик" },
                new OstrichGood { Name = "Страусик" },
                new KiwiGood { Name = "Ківі" }
            };

            var sanctuary = new BirdSanctuaryGood();

            // Годування працює гарантовано для ВСІХ птахів (LSP у дії)
            sanctuary.FeedBirds(allBirds);

            Console.WriteLine("\n" + new string('═', 60));

            // Політ викликається ТІЛЬКИ для тих, хто реалізує контракт IFlyable
            var flyingBirds = allBirds.OfType<IFlyable>().ToList();
            sanctuary.MakeFlyableBirdsFly(flyingBirds);

            Console.WriteLine(new string('═', 60));

            // Плавання викликається ТІЛЬКИ для тих, хто реалізує контракт ISwimmable
            var swimmingBirds = allBirds.OfType<ISwimmable>().ToList();
            sanctuary.MakeSwimmableBirdsSwim(swimmingBirds);

            Console.WriteLine(new string('═', 60));

            // Демонстрація поліморфної інформації про птахів
            Console.WriteLine("\n[ІНФОРМАЦІЯ ПРО ПТАХІВ У ЗАПОВІДНИКУ]:");

            sanctuary.ShowBirdInfo(new EagleGood { Name = "Беркут" });
            sanctuary.ShowBirdInfo(new PenguinGood { Name = "Антарктик" });
            sanctuary.ShowBirdInfo(new OstrichGood { Name = "Швидкий" });

            Console.WriteLine("\n\n[РЕЗУЛЬТАТ] ПРИНЦИП ПІДСТАНОВКИ ЛІСКОВ (LSP) ДОТРИМАНО:");
            Console.WriteLine("   • Всі клієнтські методи, які очікують BirdGood, безвідмовно працюють з УСІМА нащадками;");
            Console.WriteLine("   • Відсутні порушення інваріантів та штучні винятки NotSupportedException;");
            Console.WriteLine("   • Клієнту більше не потрібно перевіряти типи об'єктів перед викликом методів;");
            Console.WriteLine("   • Базовий тип інкапсулює лише справжній спільний контракт предметної області;");
            Console.WriteLine("   • Специфічні поведінкові ролі винесені в інтерфейси (IFlyable, ISwimmable).");
        }
    }
}
