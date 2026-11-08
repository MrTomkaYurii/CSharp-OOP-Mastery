using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S.SRPGoodExample
{
    // 2. Валідатор - відповідає ЛИШЕ за валідацію
    public class OrderValidator
    {
        public void Validate(Order order)
        {
            if (string.IsNullOrEmpty(order.CustomerEmail))
                throw new ArgumentException("Email не може бути порожнім");

            if (order.Items == null || !order.Items.Any())
                throw new ArgumentException("Замовлення повинно містити товари");

            if (order.TotalPrice <= 0)
                throw new ArgumentException("Сума замовлення має бути більше 0");

            Console.WriteLine("[ВАЛІДАЦІЯ] [ОК] Замовлення успішно пройшло перевірку");
        }
    }
}
