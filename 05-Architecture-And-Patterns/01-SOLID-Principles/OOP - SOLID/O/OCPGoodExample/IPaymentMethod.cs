using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O.OCPGoodExample
{
    // Базовий інтерфейс для способів оплати
    public interface IPaymentMethod
    {
        string Name { get; }
        void Process(decimal amount);
        decimal CalculateFee(decimal amount);
    }
}
