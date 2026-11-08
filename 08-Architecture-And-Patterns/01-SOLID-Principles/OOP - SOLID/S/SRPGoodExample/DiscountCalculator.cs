using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S.SRPGoodExample
{
    // 3. Калькулятор знижок - відповідає ЛИШЕ за бізнес-логіку знижок
    public class DiscountCalculator
    {
        public decimal Calculate(decimal totalPrice)
        {
            // Легко змінити логіку знижок без впливу на інші частини системи
            if (totalPrice > 1000)
            {
                Console.WriteLine("[ЗНИЖКА] Застосовано знижку 10% (замовлення > 1000 грн)");
                return totalPrice * 0.1m;
            }

            Console.WriteLine("[ЗНИЖКА] Знижка не застосована");
            return 0;
        }
    }
}
