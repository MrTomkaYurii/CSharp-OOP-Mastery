using System;
using System.IO;
using System.Text;

namespace FileClassDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Підтримка української мови
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Шлях до файлу, який потрібно створити
            string FilePath = @"C:\1\MyNewFile.txt";

            // Створення файлу методом File.Create
            // Метод повертає FileStream
            FileStream fileStream = File.Create(FilePath);

            // Закриваємо FileStream одразу після створення файлу
            fileStream.Close();

            // Перевірка існування створеного файлу
            if (File.Exists(FilePath))
            {
                // Масив рядків для запису
                string[] content = { "Hello", "And", "Welcome" };

                // Запис рядків у файл
                File.WriteAllLines(FilePath, content, Encoding.UTF8);

                Console.WriteLine("Файл MyNewFile.txt створено з такими даними:");

                // Зчитування тексту з файлу
                string fileContent = File.ReadAllText(FilePath, Encoding.UTF8);

                // Виведення вмісту у консоль
                Console.WriteLine(fileContent);
            }
            else
            {
                Console.WriteLine("Файл MyNewFile.txt НЕ існує у каталозі C:\\1\\");
            }

            Console.ReadKey();
        }
    }
}
