using System;
using System.Collections;

namespace HasntableExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створення Hashtable за допомогою синтаксису ініціалізатора колекції
            Hashtable citiesHashtable = new Hashtable()
            {
                { "UK", "London, Manchester, Birmingham" }, // Ключ: UK, Значення: список міст
                { "USA", "Chicago, New York, Washington" }, // Ключ: USA, Значення: список міст
                { "India", "Mumbai, New Delhi, Pune" }       // Ключ: India, Значення: список міст
            };

            Console.WriteLine("Створення Hashtable за допомогою ініціалізатора колекції:");

            // Перебір елементів Hashtable через DictionaryEntry
            foreach (DictionaryEntry city in citiesHashtable)
            {
                Console.WriteLine($"Key: {city.Key}, Value: {city.Value}");
            }

            Console.ReadKey();
        }
    }
}
