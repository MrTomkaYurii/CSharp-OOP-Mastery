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
            arrayList1.Add("James");  // Додавання дубліката
            arrayList1.Add(" ");      // Додавання порожнього рядка
            arrayList1.Add(true);     // Додавання логічного значення (bool)
            arrayList1.Add(4.5);      // Додавання дробового числа (double)
            arrayList1.Add(null);     // Додавання null

            // Виведення всіх елементів першого ArrayList
            foreach (var item in arrayList1)
            {
                Console.WriteLine(item);
            }

            // Додавання елементів до ArrayList за допомогою ініціалізатора об'єкта
            var arrayList2 = new ArrayList()
            {
                102, "Smith", "Smith", true, 15.6
            };

            // Виведення всіх елементів другого ArrayList
            foreach (var item in arrayList2)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }
    }
}
