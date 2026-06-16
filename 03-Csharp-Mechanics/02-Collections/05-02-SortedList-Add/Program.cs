using System;
using System.Collections.Generic;

namespace GenericSortedListDemo
{
    class Program
    {
        static void Main()
        {
            // Створення generic SortedList за допомогою ініціалізатора колекції.
            // SortedList автоматично сортує елементи за ключем (у цьому випадку — за зростанням int).
            SortedList<int, string> genericSortedList = new SortedList<int, string>
            {
                { 1, "One" },
                { 5, "Five" },
                { 4, "Four" },
                { 2, "Two" },
                { 3, "Three" }
            };

            // Доступ до елементів SortedList через цикл foreach.
            // foreach перебирає елементи вже у відсортованому порядку.
            Console.WriteLine("Доступ до SortedList через foreach:");
            foreach (KeyValuePair<int, string> item in genericSortedList)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            Console.ReadKey();
        }
    }
}
