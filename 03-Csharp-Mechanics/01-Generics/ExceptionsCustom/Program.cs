using System.Text;
using ExceptionsCustom;

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
                case "1": Demos.SystemThrown(); break;
                case "2": Demos.SystemGuards(); break;
                case "3": Demos.Hierarchy(); break;
                case "4": Demos.CustomException(); break;
                case "5": Demos.CatchByHierarchy(); break;
                case "6": Demos.InnerExceptionDemo(); break;
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
        Console.WriteLine("######## Винятки: вбудовані та власні ########");
        Console.WriteLine(" ВБУДОВАНІ (\"зарезервовані\" в .NET):");
        Console.WriteLine("   1. Що кидає сам CLR (NullReference, IndexOutOfRange...)");
        Console.WriteLine("   2. Що кидаємо ми (Argument*, InvalidOperation)");
        Console.WriteLine("   3. Ієрархія: усі від System.Exception");
        Console.WriteLine(" ВЛАСНІ:");
        Console.WriteLine("   4. Власний виняток: навіщо він потрібен");
        Console.WriteLine("   5. Catch за ієрархією власних і вбудованих");
        Console.WriteLine("   6. InnerException та відкат переказу");
        Console.WriteLine("   0. Вихід");
        Console.WriteLine("##############################################");
    }
}
