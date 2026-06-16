using System;
using System.Collections;

namespace HasntableExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створення колекції Hashtable за замовчуванням
            Hashtable hashtable = new Hashtable();

            // Додавання елементів у Hashtable у вигляді пар "ключ–значення"
            hashtable.Add("EId", 1001);        // Ключ: "EId", Значення: 1001
            hashtable.Add("Name", "James");    // Ключ: "Name", Значення: "James"
            hashtable.Add("Salary", 3500);     // Ключ: "Salary", Значення: 3500
            hashtable.Add("Location", "Mumbai"); // Ключ: "Location", Значення: "Mumbai"
            hashtable.Add("EmailID", "a@a.com"); // Ключ: "EmailID", Значення: "a@a.com"

            // Виведення всіх ключів та їхніх значень за допомогою foreach
            Console.WriteLine("Виведення Hashtable через цикл foreach:");
            foreach (object key in hashtable.Keys)
            {
                Console.WriteLine(key + " : " + hashtable[key]);
            }

            // Або альтернативний спосіб:
            // foreach (DictionaryEntry de in hashtable)
            // {
            //     Console.WriteLine($"Key: {de.Key}, Value: {de.Value}");
            // }

            Console.WriteLine("\nВиведення окремих значень за ключами:");

            // Отримання значення за ключем "Location"
            Console.WriteLine("Location : " + hashtable["Location"]);

            // Отримання значення за ключем "EmailID"
            Console.WriteLine("Email ID : " + hashtable["EmailID"]);

            Console.ReadKey();
        }
    }
}
