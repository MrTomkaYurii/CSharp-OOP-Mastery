using OOP___SOLID.L.LSPBadExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L
{
    public class LspBadExampleCommand : IMenuCommand
    {
        public string Title => "LSP - ПОГАНИЙ приклад (Порушення принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  LSP - ПОГАНИЙ ПРИКЛАД: Порушення принципу підстановки     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПРОБЛЕМИ цього підходу:");
            Console.WriteLine("[!] Базовий клас BirdBad містить метод Fly()");
            Console.WriteLine("[-] Але не всі птахи літають у реальному світі (пінгвін, страус, ківі)");
            Console.WriteLine("[-] Класи PenguinBad та OstrichBad змушені викидати NotSupportedException");
            Console.WriteLine("[-] Порушення LSP: неможливо безпечно замінити BirdBad на PenguinBad");
            Console.WriteLine("[-] Клієнтський код, розрахований на контракт BirdBad, неочікувано аварійно завершується");
            Console.WriteLine("[-] З'являється потреба в 'милицях': перевірках типів (is, as) та блоках try-catch\n");

            Console.WriteLine("[ТЕОРІЯ] Принцип підстановки Лісков (Liskov Substitution Principle) стверджує:");
            Console.WriteLine("   'Якщо S є підтипом T, то об'єкти типу T можуть бути замінені");
            Console.WriteLine("    об'єктами типу S без спотворення бажаних властивостей програми.'\n");
            Console.WriteLine("   У нашому випадку: екземпляр BirdBad не можна замінити на PenguinBad без поломки!");

            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            var birds = new List<BirdBad>
            {
                new SparrowBad { Name = "Сіренький" },
                new EagleBad { Name = "Орлик" },
                new PenguinBad { Name = "Пінгвінчик" },
                new OstrichBad { Name = "Страусик" }
            };

            var sanctuary = new BirdSanctuaryBad();

            sanctuary.FeedBirds(birds);

            Console.WriteLine("\n" + new string('═', 60));
            sanctuary.MakeBirdsFly(birds);  // Тут виникає аварійна ситуація!

            Console.WriteLine("\n\n[ВИСНОВОК]");
            Console.WriteLine("   Метод MakeBirdsFly очікує, що ВСІ нащадки BirdBad безпечно реалізують Fly(),");
            Console.WriteLine("   але нелітаючі птахи ламають очікування контракту базового типу.");
            Console.WriteLine("   Це пряме порушення LSP (порушення передумов та поведінкової сумісності).");
        }
    }
}
