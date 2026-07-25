using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPBadExample
{
    // ПОГАНИЙ ПРИКЛАД: Страус НЕ літає в природі - ще одне порушення LSP
    public class OstrichBad : BirdBad
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Страус їсть рослини та комах");
        }

        public override void Fly()
        {
            // Порушення LSP: страуси бігають, але не літають
            throw new NotSupportedException($"{Name}: страус не вміє літати!");
        }
    }
}
