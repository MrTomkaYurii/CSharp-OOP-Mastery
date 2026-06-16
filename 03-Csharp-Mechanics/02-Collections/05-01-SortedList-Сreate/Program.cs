using System;
using System.Collections.Generic;

namespace GenericSortedListDemo
{
    class Program
    {
        static void Main()
        {
            // Створення generic SortedList — колекція, яка зберігає пари "ключ–значення"
            // і автоматично сортує їх за ключем
            SortedList<int, string> genericSortedList = new SortedList<int, string>();

            // Додавання елементів у випадковому порядку — SortedList все одно їх відсортує
            genericSortedList.Add(1, "One");
            genericSortedList.Add(5, "Five");
            genericSortedList.Add(4, "Four");
            genericSortedList.Add(2, "Two");
            genericSortedList.Add(3, "Three");

            // У SortedList ключ НЕ може бути null
            //genericSortedList.Add(null, "Ten"); // Помилка компіляції

            // Дублікати ключів НЕ допускаються — буде виняток ArgumentException
            //genericSortedList.Add(2, "Any Value");

            // Доступ до SortedList через цикл for (через індексацію)
            Console.WriteLine("Доступ до Generic SortedList через цикл for:");
            for (int i = 0; i < genericSortedList.Count; i++)
            {
                Console.WriteLine("Key : " + genericSortedList.Keys[i] + "\tValue : " + genericSortedList.Values[i]);
            }

            // Доступ до SortedList через foreach
            Console.WriteLine("\nДоступ до SortedList через foreach:");
            foreach (KeyValuePair<int, string> item in genericSortedList)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Доступ до окремих значень по ключу
            Console.WriteLine("\nДоступ до окремих елементів SortedList за ключем:");
            Console.WriteLine($"Key: 1, Value: {genericSortedList[1]}");
            Console.WriteLine($"Key: 2, Value: {genericSortedList[2]}");
            Console.WriteLine($"Key: 3, Value: {genericSortedList[3]}");

            Console.ReadKey();
        }
    }
}
