using System;
using System.IO;
using System.Text;

namespace FileClassDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Підтримка української мови в консолі
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Вказуємо шлях до файлу
            string FilePath = @"C:\1\MyFile.txt";

            // Перевіряємо, чи існує файл за допомогою File.Exists
            // Exists приймає шлях до файлу як параметр
            if (File.Exists(FilePath))
            {
                // Якщо файл існує — читаємо всі рядки методом ReadAllLines
                // ReadAllLines повертає масив рядків (string[])
                string[] lines = File.ReadAllLines(FilePath, Encoding.UTF8);

                // Використовуємо foreach щоб пройтися по кожному рядку
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("Файл MyFile.txt НЕ існує у каталозі C:\\1\\");
            }

            Console.ReadKey();
        }
    }
}
