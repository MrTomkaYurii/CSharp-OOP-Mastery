// Приклад демонструє інтерфейс IComparer на базі стандартного Array.Sort.
// Клас ReverserClass реалізує IComparer так, щоб отримати зворотний
// (спадний) порядок сортування рядків без урахування регістру,
// просто переставивши місцями аргументи стандартного порівняння.
using System;
using System.Collections;

public class Example
{
    // Явна реалізація IComparer.Compare (через ім'я інтерфейсу),
    // яка інвертує звичайний порядок порівняння
    public class ReverserClass : IComparer
    {
        // Викликаємо CaseInsensitiveComparer.Compare з переставленими місцями
        // аргументами (y, x замість x, y) - це і дає зворотне сортування
        int IComparer.Compare(Object x, Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(y, x));
        }
    }

    public static void Main()
    {
        // Ініціалізуємо масив рядків
        string[] words = { "The", "quick", "brown", "fox", "jumps", "over",
                         "the", "lazy", "dog" };

        // Виводимо початковий вміст масиву
        Console.WriteLine("The array initially contains the following values:");
        PrintIndexAndValues(words);

        // Сортуємо масив стандартним (природним) компаратором рядків
        Array.Sort(words);
        Console.WriteLine("After sorting with the default comparer:");
        PrintIndexAndValues(words);

        // Сортуємо той самий масив, але передаємо власний компаратор
        // ReverserClass - в результаті отримуємо зворотний порядок
        Array.Sort(words, new ReverserClass());
        Console.WriteLine("After sorting with the reverse case-insensitive comparer:");
        PrintIndexAndValues(words);
    }

    public static void PrintIndexAndValues(IEnumerable list)
    {
        int i = 0;
        foreach (var item in list)
            Console.WriteLine($"   [{i++}]:  {item}");

        Console.WriteLine();
    }
}
// The example displays the following output:
//       The array initially contains the following values:
//          [0]:  The
//          [1]:  quick
//          [2]:  brown
//          [3]:  fox
//          [4]:  jumps
//          [5]:  over
//          [6]:  the
//          [7]:  lazy
//          [8]:  dog
//
//       After sorting with the default comparer:
//          [0]:  brown
//          [1]:  dog
//          [2]:  fox
//          [3]:  jumps
//          [4]:  lazy
//          [5]:  over
//          [6]:  quick
//          [7]:  the
//          [8]:  The
//
//       After sorting with the reverse case-insensitive comparer:
//          [0]:  the
//          [1]:  The
//          [2]:  quick
//          [3]:  over
//          [4]:  lazy
//          [5]:  jumps
//          [6]:  fox
//          [7]:  dog
//          [8]:  brown