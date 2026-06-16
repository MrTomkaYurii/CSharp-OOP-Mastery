using System;
using System.Collections;

namespace Csharp8Features
{
    public class ArrayListDemo
    {
        public static void Main()
        {
            // Додавання елементів до ArrayList за допомогою методу Add()
            ArrayList arrayList1 = new ArrayList();
            arrayList1.Add(101);      // Додавання цілого числа (int)
            arrayList1.Add("James");  // Додавання рядка (string)
            arrayList1.Add(true);     // Додавання логічного значення (bool)
            arrayList1.Add(4.5);      // Додавання дробового числа (double)

            // Ітерація елементів за допомогою циклу foreach
            Console.WriteLine("Використання циклу foreach");
            foreach (var item in arrayList1)
            {
                Console.Write($"{item} ");
            }

            // Ітерація елементів за допомогою циклу for
            Console.WriteLine("\n\nВикористання циклу for");
            for (int i = 0; i < arrayList1.Count; i++)
            {
                Console.Write($"{arrayList1[i]} ");
            }

            Console.ReadKey();
        }
    }
}
