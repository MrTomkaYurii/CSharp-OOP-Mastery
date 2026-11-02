using System.Text;
using ExceptionsBasics;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;            // щоб коректно виводилась українська

        while (true)
        {
            PrintMenu();
            Console.Write("Ваш вибір: ");
            string? choice = Console.ReadLine();

            if (choice is null || choice == "0")           // null — коли ввід закінчився (Ctrl+Z)
                break;

            switch (choice)
            {
                case "1": Demos.BasicTryCatch(); break;
                case "2": Demos.MultipleCatch(); break;
                case "3": Demos.FinallyAlways(); break;
                case "4": Demos.StackUnwinding(); break;
                case "5": Demos.ThrowVsThrowEx(); break;
                case "6": Demos.UsingStatement(); break;
                case "7": Demos.EmptyCatch(); break;
                case "8": Demos.ExceptionOrTryParse(); break;
                default:
                    Ui.Bad("Невідомий пункт меню.");
                    break;
            }

            Console.WriteLine();
            Console.Write("Натисніть Enter, щоб повернутись до меню...");
            Console.ReadLine();
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("############ Винятки: try / catch / finally ############");
        Console.WriteLine("   1. try / catch — перехоплення винятку");
        Console.WriteLine("   2. Кілька catch (порядок має значення)");
        Console.WriteLine("   3. finally виконується завжди");
        Console.WriteLine("   4. Розкрутка стека (виняток летить вгору)");
        Console.WriteLine("   5. throw; проти throw ex;");
        Console.WriteLine("   6. using = try / finally");
        Console.WriteLine("   7. Антипатерн: порожній catch");
        Console.WriteLine("   8. Виняток чи TryParse?");
        Console.WriteLine("   0. Вихід");
        Console.WriteLine("#########################################################");
    }
}
