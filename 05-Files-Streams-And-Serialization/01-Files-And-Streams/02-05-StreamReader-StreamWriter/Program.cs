using System;
using System.IO;
using System.Text;

namespace FileHandlinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Увімкнення коректного виводу української мови в консоль
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Створюємо змінну, яка міститиме шлях до текстового файлу
            string filePath = @"C:\1\MyFile.txt";

            // Створення екземпляра StreamReader для читання даних із файлу
            StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);

            Console.WriteLine("Вміст файлу:");

            // Читання текстового файлу за допомогою класу StreamReader
            // Використовуємо using, щоб автоматично закрити потік після завершення роботи
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                // ReadToEnd читає файл повністю від початку до кінця і повертає весь текст одним рядком
                Console.WriteLine(reader.ReadToEnd());
            }

            // Закриваємо streamReader, створений раніше
            streamReader.Close();

            Console.ReadKey();
        }
    }
}
