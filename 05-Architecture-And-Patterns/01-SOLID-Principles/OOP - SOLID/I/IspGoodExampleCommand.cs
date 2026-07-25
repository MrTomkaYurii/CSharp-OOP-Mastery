using OOP___SOLID.I.ISPGoodExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.I
{
    public class IspGoodExampleCommand : IMenuCommand
    {
        public string Title => "ISP - ГАРНИЙ приклад (Дотримання принципу)";

        public void Execute()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  ISP - ГАРНИЙ ПРИКЛАД: Розділені специфічні інтерфейси     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.WriteLine("ПЕРЕВАГИ цього підходу:");
            Console.WriteLine("[+] Замість одного велетенського інтерфейсу - 8 вузькоспеціалізованих рольових контрактів:");
            Console.WriteLine("   • IWorkable, IComputerUser, IOfficeWorker");
            Console.WriteLine("   • IEatable, ISalaried, IManager");
            Console.WriteLine("   • IDeveloper, IShiftWorker\n");

            Console.WriteLine("[+] Кожен клас реалізує ТІЛЬКИ потрібні йому інтерфейси");
            Console.WriteLine("[+] Програміст НЕ реалізує методи менеджера");
            Console.WriteLine("[+] Робот НЕ реалізує методи для їжі та зарплати");
            Console.WriteLine("[+] Фрілансер НЕ реалізує прив'язані до офісу методи");
            Console.WriteLine("[+] Інтерн НЕ реалізує методи для фіксованої зарплати");
            Console.WriteLine("[+] Жодних фіктивних методів та NotImplementedException!");
            Console.WriteLine("[+] Легко додавати нові типи виконавців без переписування наявних класів");
            Console.WriteLine("[+] Суворе дотримання ISP - клієнти захищені від непотрібних залежностей\n");

            Console.WriteLine(new string('─', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 1: Програміст\n");

            var programmer = new ProgrammerGood("Іван");
            programmer.Work();
            programmer.WriteCode();
            programmer.DebugCode();
            programmer.UseComputer();
            programmer.EatLunch();
            programmer.TakeCoffeeBreak();
            programmer.ReceiveSalary();

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 2: Менеджер\n");

            var manager = new ManagerGood("Олена");
            manager.Work();
            manager.ManageTeam();
            manager.ConductPerformanceReview();
            manager.ApproveBudget();
            manager.AttendMeeting();
            manager.SendEmail();
            manager.EatLunch();
            manager.ReceiveSalary();

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 3: Робот (БЕЗ їжі та зарплати)\n");

            var robot = new RobotWorkerGood("T-800");
            robot.Work();
            robot.WorkNightShift();
            robot.WorkWeekends();
            // У робота немає методів EatLunch(), ReceiveSalary() - вони йому не потрібні за його призначенням!

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 4: Фрілансер (БЕЗ офісних методів)\n");

            var freelancer = new FreelancerGood("Максим");
            freelancer.Work();
            freelancer.WriteCode();
            freelancer.ReviewCode();
            freelancer.ReceiveSalary();
            // У фрілансера немає AttendMeeting(), SendEmail() - він працює поза офісними процедурами!

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("ДЕМОНСТРАЦІЯ 5: Інтерн (БЕЗ зарплати)\n");

            var intern = new InternGood("Анна");
            intern.Work();
            intern.WriteCode();
            intern.ReviewCode();
            intern.UseComputer();
            intern.EatLunch();
            intern.TakeCoffeeBreak();
            // У інтерна немає ISalaried - він на стажуванні!

            Console.WriteLine("\n\n[РЕЗУЛЬТАТ] ПРИНЦИП РОЗДІЛЕННЯ ІНТЕРФЕЙСІВ (ISP) ДОТРИМАНО:");
            Console.WriteLine("   • Кожен виконавець реалізує ТІЛЬКИ свій профільний набір контрактів:");
            Console.WriteLine("     - Програміст: кодування + робоче місце + відпочинок + оплата;");
            Console.WriteLine("     - Менеджер: управління + офісна діяльність + відпочинок + оплата;");
            Console.WriteLine("     - Робот: робота + змінні графіки (БЕЗ біологічних потреб та зарплати);");
            Console.WriteLine("     - Фрілансер: кодування + пряма оплата (БЕЗ офісних мітингів);");
            Console.WriteLine("     - Інтерн: кодування + офіс + навчання (БЕЗ штатної ставки).");
            Console.WriteLine("\n   Жоден клас не змушений містити методи-заглушки чи порожні реалізації!");
        }
    }
}
