using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID
{
    // Базовий інтерфейс для пунктів меню (OCP - відкрито для розширення)
    public interface IMenuCommand
    {
        string Title { get; }
        void Execute();
    }
}
