using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPBadExample
{
    // ПОГАНИЙ ПРИКЛАД: Пінгвін НЕ літає в природі - порушення LSP!
    public class PenguinBad : BirdBad
    {
        public override void Eat()
        {
            Console.WriteLine($"[{Name}] Пінгвін їсть рибу");
        }

        public override void Fly()
        {
            // Порушення LSP: об'єкт підтипу ламає поведінковий контракт базового класу,
            // викидаючи виняток замість виконання заявленої операції.
            throw new NotSupportedException($"{Name}: пінгвін не вміє літати!");
        }
    }
}
