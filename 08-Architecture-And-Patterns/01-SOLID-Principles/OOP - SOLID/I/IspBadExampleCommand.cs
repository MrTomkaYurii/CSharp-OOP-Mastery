using OOP___SOLID.I.ISPBadExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I
{
    public class IspBadExampleCommand : IMenuCommand
    {
        public string Title => "ISP - ПОГАНИЙ приклад (Порушення принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  ISP - ПОГАНИЙ ПРИКЛАД: Один великий інтерфейс             ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПРОБЛЕМИ цього підходу:");
            Console.WriteLine("[!] 'Товстий' інтерфейс IWorkerBad містить 15 непов'язаних методів для всіх працівників");
            Console.WriteLine("[-] Програміст змушений реалізовувати управлінські методи менеджера");
            Console.WriteLine("[-] Робот змушений реалізовувати фізіологічні та фінансові методи (їжа, зарплата)");
            Console.WriteLine("[-] Більшість нецільових методів викидають NotImplementedException");
            Console.WriteLine("[-] Порушення ISP: клієнти змушені залежати від методів, які їм не потрібні");
            Console.WriteLine("[-] Надзвичайно висока зв'язність і крихкість архітектури");
            Console.WriteLine("[-] Додавання нового методу в загальний інтерфейс ламає ВСІ наявні класи-імплементатори\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ РОБОТИ:\n");

            var programmer = new ProgrammerBad("Іван");
            var robot = new RobotWorkerBad("T-800");

            Console.WriteLine("Програміст виконує свої завдання:");
            Console.WriteLine(new string('═', 60));
            programmer.Work();
            programmer.WriteCode();
            programmer.DebugCode();
            programmer.EatLunch();

            Console.WriteLine("\n[ПОМИЛКА ПРОЄКТУВАННЯ] Спроба викликати непритаманні методи для програміста:");
            try
            {
                programmer.ManageTeam();
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"   [ВИКЛЮЧЕННЯ] {ex.Message}");
            }

            try
            {
                programmer.ApproveBudget();
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"   [ВИКЛЮЧЕННЯ] {ex.Message}");
            }

            Console.WriteLine("\n\nРобот виконує свої завдання:");
            Console.WriteLine(new string('═', 60));
            robot.Work();
            robot.WorkNightShift();
            robot.WorkWeekends();

            Console.WriteLine("\n[ПОМИЛКА ПРОЄКТУВАННЯ] Спроба викликати непритаманні методи для робота:");
            try
            {
                robot.EatLunch();
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"   [ВИКЛЮЧЕННЯ] {ex.Message}");
            }

            try
            {
                robot.ReceiveSalary();
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"   [ВИКЛЮЧЕННЯ] {ex.Message}");
            }

            try
            {
                robot.WriteCode();
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"   [ВИКЛЮЧЕННЯ] {ex.Message}");
            }

            Console.WriteLine("\n\n[ВИСНОВОК]");
            Console.WriteLine("   Класи змушені залежати від контрактів та методів, які вони не використовують!");
            Console.WriteLine("   Це класичне порушення принципу розділення інтерфейсів (ISP).");
        }
    }
}
