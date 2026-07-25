using OOP___SOLID.I.ISPGoodExample.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I.ISPGoodExample
{
    // ДОБРИЙ ПРИКЛАД: Фрілансер реалізує виключно релевантні контракти (робота, розробка, оплата)
    public class FreelancerGood : IWorkable, IDeveloper, ISalaried
    {
        private string _name;

        public FreelancerGood(string name)
        {
            _name = name;
        }

        public void Work()
        {
            Console.WriteLine($"[{_name}] Працюю віддалено...");
        }

        public void WriteCode()
        {
            Console.WriteLine($"[{_name}] Пишу код для клієнта");
        }

        public void ReviewCode()
        {
            Console.WriteLine($"[{_name}] Переглядаю код віддаленої команди");
        }

        public void DebugCode()
        {
            Console.WriteLine($"[{_name}] Виправляю дефекти у коді");
        }

        public void ReceiveSalary()
        {
            Console.WriteLine($"[{_name}] Отримую оплату за виконаний спринт");
        }

        // Немає прив'язки до офісних методів - фрілансер працює віддалено.
    }
}
