using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.L.LSPBadExample
{
    // Базовий клас для птахів (помилкове узагальнення для поведінки польоту)
    public abstract class BirdBad
    {
        public string Name { get; set; } = string.Empty;

        public abstract void Eat();
        public abstract void Fly();  // Помилка проєктування: Не всі птахи літають у реальності!
    }
}
