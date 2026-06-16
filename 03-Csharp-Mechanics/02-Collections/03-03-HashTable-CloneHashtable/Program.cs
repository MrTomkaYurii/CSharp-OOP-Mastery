using System;
using System.Collections;

namespace HasntableExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створення колекції Hashtable за допомогою конструктора за замовчуванням
            Hashtable hashtable = new Hashtable();

            // Додавання елементів у Hashtable за допомогою методу Add
            hashtable.Add("EId", 1001);           // Ключ: EId, Значення: 1001
            hashtable.Add("Name", "James");       // Ключ: Name, Значення: James
            hashtable.Add("Job", "Developer");    // Ключ: Job, Значення: Developer
            hashtable.Add("Salary", 3500);        // Ключ: Salary, Значення: 3500
            hashtable.Add("Location", "Mumbai");  // Ключ: Location, Значення: Mumbai
            hashtable.Add("Dept", "IT");          // Ключ: Dept, Значення: IT
            hashtable.Add("EmailID", "a@a.com");  // Ключ: EmailID, Значення: a@a.com

            Console.WriteLine("Елементи Hashtable:");
            foreach (DictionaryEntry item in hashtable)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            // Створення копії Hashtable за допомогою методу Clone
            Hashtable cloneHashtable = (Hashtable)hashtable.Clone();

            Console.WriteLine("\nЕлементи клонованого Hashtable:");
            foreach (DictionaryEntry item in cloneHashtable)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            Console.ReadKey();
        }
    }
}
