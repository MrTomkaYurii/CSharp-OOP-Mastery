using System;
using System.Collections.Generic;

namespace GenericSortedListDemo
{
    class Program
    {
        static void Main()
        {
            // Створення Generic SortedList
            // Тут і ключі, і значення — тип string
            SortedList<string, string> genericSortedList = new SortedList<string, string>();

            // Додавання елементів у SortedList
            genericSortedList.Add("Ind", "India");
            genericSortedList.Add("USA", "United State of America");
            genericSortedList.Add("SA", "South Africa");
            genericSortedList.Add("PAK", "Pakistan");

            // Виведення елементів SortedList + інформація про кількість елементів і ємність (Capacity)
            Console.WriteLine($"SortedList — Count={genericSortedList.Count}, Capacity={genericSortedList.Capacity}");
            foreach (KeyValuePair<string, string> item in genericSortedList)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Видалення елемента з ключем "PAK" методом Remove
            genericSortedList.Remove("PAK");
            Console.WriteLine($"\nПісля Remove(\"PAK\") — Count={genericSortedList.Count}, Capacity={genericSortedList.Capacity}");
            foreach (KeyValuePair<string, string> item in genericSortedList)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Видалення елемента за індексом (RemoveAt)
            genericSortedList.RemoveAt(1);
            Console.WriteLine($"\nПісля RemoveAt(1) — Count={genericSortedList.Count}, Capacity={genericSortedList.Capacity}");
            foreach (KeyValuePair<string, string> item in genericSortedList)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Повне очищення колекції — Clear()
            genericSortedList.Clear();
            Console.WriteLine($"\nПісля Clear() — Count={genericSortedList.Count}, Capacity={genericSortedList.Capacity}");

            Console.ReadKey();
        }
    }
}
