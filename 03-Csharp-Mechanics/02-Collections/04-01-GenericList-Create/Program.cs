using System;
using System.Collections.Generic;

namespace GenericListCollectionDemo
{
    class Program
    {
        static void Main()
        {
            // Створення generic-списку типу string для зберігання рядкових елементів
            List<string> countries = new List<string>();

            // Додавання рядкових елементів у generic List за допомогою методу Add
            countries.Add("INDIA");
            countries.Add("USA");

            // Наступні два рядки викличуть помилку на етапі компіляції,
            // тому що список приймає тільки string-значення
            //countries.Add(100);
            //countries.Add(true);

            // Створення ще одного generic-списку типу string
            List<string> newCountries = new List<string>();

            // Додавання елементів у список
            newCountries.Add("JAPAN");
            newCountries.Add("UK");

            // Додавання всіх елементів newCountries у countries за допомогою AddRange
            countries.AddRange(newCountries);

            // Доступ до елементів List за допомогою foreach
            Console.WriteLine("Виведення елементів List за допомогою foreach:");
            foreach (var item in countries)
            {
                Console.WriteLine(item);
            }

            // Доступ до елементів List за допомогою циклу for
            Console.WriteLine("\nВиведення елементів List за допомогою for:");
            for (int i = 0; i < countries.Count; i++)
            {
                var element = countries[i];
                Console.WriteLine(element);
            }

            // Доступ до окремих елементів за індексом
            Console.WriteLine("\nДоступ до окремих елементів List за індексом:");
            Console.WriteLine($"Перший елемент: {countries[0]}");
            Console.WriteLine($"Другий елемент: {countries[1]}");
            Console.WriteLine($"Третій елемент: {countries[2]}");
            Console.WriteLine($"Четвертий елемент: {countries[3]}");

            Console.ReadKey();
        }
    }
}
