using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPGoodExample
{
    // Базовий клас для всіх птахів (містить виключно спільну поведінку доменної моделі)
    public abstract class BirdGood
    {
        public string Name { get; set; } = string.Empty;

        // Всі птахи їдять - це спільна поведінка
        public abstract void Eat();

        // Всі птахи видають звуки
        public abstract void MakeSound();
    }
}
