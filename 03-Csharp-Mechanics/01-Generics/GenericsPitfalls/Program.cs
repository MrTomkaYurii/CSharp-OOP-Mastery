using System.Text;
using GenericsPitfalls;
using GenericsPitfalls.Demos;

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

            // Кожен пункт меню — окремий статичний метод із демо
            switch (choice)
            {
                case "1": RuntimeErrors.ArrayCovariance(); break;
                case "2": RuntimeErrors.NonGenericList(); break;
                case "3": RuntimeErrors.BoxingUnboxing(); break;
                case "4": RuntimeErrors.NullFromRepository(); break;
                case "5": RuntimeErrors.NullableValue(); break;
                case "6": RuntimeErrors.StaticPerClosedType(); break;
                case "7": RuntimeErrors.DictionaryKeys(); break;
                case "8": RuntimeErrors.ModifyDuringForeach(); break;
                case "9": RuntimeErrors.KeysWithoutEquality(); break;

                case "10": CompileErrors.ListInvariance(); break;
                case "11": CompileErrors.ConstraintViolation(); break;
                case "12": CompileErrors.NoMemberWithoutConstraint(); break;
                case "13": CompileErrors.NewOfT(); break;
                case "14": CompileErrors.TypeInference(); break;
                case "15": CompileErrors.CompareGeneric(); break;
                case "16": CompileErrors.VarianceDeclaration(); break;
                case "17": CompileErrors.VarianceDirection(); break;
                case "18": CompileErrors.RepositoryInvariance(); break;

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
        Console.WriteLine("################ Типові помилки з generics ################");
        Console.WriteLine(" ПОМИЛКИ ЧАСУ ВИКОНАННЯ (збирається, але ламається):");
        Console.WriteLine("   1. Коваріантність масивів");
        Console.WriteLine("   2. ArrayList замість List<T>");
        Console.WriteLine("   3. Boxing / unboxing");
        Console.WriteLine("   4. default(T) = null з репозиторію");
        Console.WriteLine("   5. Nullable<T>.Value без значення");
        Console.WriteLine("   6. Статичне поле в generic-класі");
        Console.WriteLine("   7. Dictionary: дублікат і відсутній ключ");
        Console.WriteLine("   8. Зміна списку під час foreach");
        Console.WriteLine("   9. Ключ без Equals / GetHashCode");
        Console.WriteLine(" ПОМИЛКИ КОМПІЛЯЦІЇ (див. Demos\\CompileErrors.cs):");
        Console.WriteLine("  10. List<Dog> -> List<Animal>");
        Console.WriteLine("  11. Порушення обмеження where");
        Console.WriteLine("  12. Доступ до члена T без обмеження");
        Console.WriteLine("  13. new T() без new()");
        Console.WriteLine("  14. Компілятор не може вивести T");
        Console.WriteLine("  15. Оператор > для T");
        Console.WriteLine("  16. Неправильний in/out в інтерфейсі");
        Console.WriteLine("  17. Контраваріантність у зворотний бік");
        Console.WriteLine("  18. IRepository<Dog> -> IRepository<Animal>");
        Console.WriteLine("   0. Вихід");
        Console.WriteLine("###########################################################");
    }
}
