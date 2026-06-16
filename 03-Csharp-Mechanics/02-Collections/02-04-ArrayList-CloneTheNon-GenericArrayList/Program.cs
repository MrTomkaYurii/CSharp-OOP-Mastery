using System;
using System.Collections;

namespace Csharp8Features
{
    public class ArrayListDemo
    {
        public static void Main()
        {
            // Ініціалізація ArrayList за допомогою ініціалізатора об'єкта
            ArrayList arrayList = new ArrayList()
            {
                "India",
                "USA",
                "UK",
                "Denmark",
                "HongKong",
            };

            // Виведення елементів ArrayList
            Console.WriteLine("Елементи ArrayList:");
            foreach (var item in arrayList)
            {
                Console.WriteLine($"{item} ");
            }

            // Створення копії ArrayList за допомогою методу Clone
            ArrayList cloneArrayList = (ArrayList)arrayList.Clone();

            // Виведення елементів клонованого ArrayList
            Console.WriteLine("\nЕлементи клонованого ArrayList:");
            foreach (var item in cloneArrayList)
            {
                Console.WriteLine($"{item} ");
            }

            Console.ReadKey();
        }
    }
}
