using System;
using System.Collections.Generic;

namespace GenericSortedListDemo
{
    class Program
    {
        static void Main()
        {
            // Створення Generic SortedList
            // Ключі та значення — тип string
            SortedList<string, string> genericSortedList = new SortedList<string, string>
            {
                { "Ind", "India" },
                { "USA", "United State of America" },
                { "SA", "South Africa" },
                { "PAK", "Pakistan" }
            };

            // Виведення всіх елементів SortedList через foreach
            Console.WriteLine("Елементи Generic SortedList:");
            foreach (KeyValuePair<string, string> item in genericSortedList)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Перевірка наявності ключа методом ContainsKey
            Console.WriteLine("\nПеревірка ключів ContainsKey:");
            Console.WriteLine("Чи існує ключ 'Ind' : " + genericSortedList.ContainsKey("Ind"));
            Console.WriteLine("Чи існує ключ 'NZ' : " + genericSortedList.ContainsKey("NZ"));

            // Перевірка наявності значення методом ContainsValue
            Console.WriteLine("\nПеревірка значень ContainsValue:");
            Console.WriteLine("Чи існує значення 'India' : " + genericSortedList.ContainsValue("India"));
            Console.WriteLine("Чи існує значення 'Bangladesh' : " + genericSortedList.ContainsValue("Bangladesh"));

            Console.ReadKey();
        }
    }
}
