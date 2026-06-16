using System;
using System.Collections;

namespace HasntableExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створення колекції Hashtable за замовчуванням
            // Hashtable зберігає дані у вигляді пар "ключ–значення"
            Hashtable hashtable = new Hashtable();

            // Додавання елементів у Hashtable
            hashtable.Add("EId", 1001);          // Ключ: "EId", Значення: 1001
            hashtable.Add("Name", "James");      // Ключ: "Name", Значення: "James"
            hashtable.Add("Location", "Mumbai"); // Ключ: "Location", Значення: "Mumbai"

            // Виведення всіх елементів Hashtable
            Console.WriteLine("Елементи Hashtable:");
            foreach (DictionaryEntry item in hashtable)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Копіювання всієї таблиці в масив DictionaryEntry[]
            // DictionaryEntry — це структура, яка містить Key і Value
            DictionaryEntry[] myArray = new DictionaryEntry[hashtable.Count];
            hashtable.CopyTo(myArray, 0); // копіюємо починаючи з індексу 0

            Console.WriteLine("\nЕлементи масиву після копіювання Hashtable:");
            foreach (DictionaryEntry item in myArray)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Окреме копіювання ключів
            Object[] myObjArrayKey = new Object[hashtable.Count];
            Object[] myObjArrayValue = new Object[hashtable.Count];

            Console.WriteLine("\nCopyTo: копіювання тільки ключів:");
            hashtable.Keys.CopyTo(myObjArrayKey, 0);
            foreach (var key in myObjArrayKey)
            {
                Console.WriteLine($"{key} ");
            }

            // Окреме копіювання значень
            Console.WriteLine("\nCopyTo: копіювання тільки значень:");
            hashtable.Values.CopyTo(myObjArrayValue, 0);
            foreach (var value in myObjArrayValue)
            {
                Console.WriteLine($"{value} ");
            }

            Console.ReadKey();
        }
    }
}
