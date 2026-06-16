using System;
using System.Collections.Generic;

namespace GenericListCollectionDemo
{
    class Program
    {
        static void Main()
        {
            // Створення generic-списку типу string і ініціалізація елементів
            List<string> countries = new List<string>
            {
                "INDIA",
                "USA"
            };

            Console.WriteLine("Доступ до елементів List перед вставкою:");
            foreach (var item in countries)
            {
                Console.WriteLine(item);
            }

            // Вставка нового елемента за індексом 1
            countries.Insert(1, "China");

            // Пошук індексу елемента "China"
            Console.WriteLine($"\nІндекс елемента 'China' у списку: {countries.IndexOf("China")}");

            // Виведення елементів List після Insert
            Console.WriteLine("\nСписок після вставки 'China' на позицію 1:");
            foreach (var item in countries)
            {
                Console.WriteLine(item);
            }

            // Створення другого списку типу string
            List<string> newCountries = new List<string>
            {
                "JAPAN",
                "UK"
            };

            // Вставка всього списку newCountries у список countries на індексі 2
            countries.InsertRange(2, newCountries);

            // Виведення елементів після InsertRange
            Console.WriteLine("\nСписок після InsertRange на індекс 2:");
            foreach (var item in countries)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }
    }
}
