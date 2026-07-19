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

            // Вказуємо шлях до файлу
            string FilePath = @"C:\1\MyFile.txt";

            // Перевіряємо, чи існує файл за допомогою File.Exists
            if (File.Exists(FilePath))
            {
                // Якщо файл існує — читаємо увесь текст методом ReadAllText
                // ReadAllText: відкриває файл, читає всі дані і закриває його
                string lines = File.ReadAllText(FilePath, Encoding.UTF8);

                Console.WriteLine(lines);
            }
            else
            {
                Console.WriteLine("Файл MyFile.txt НЕ існує у каталозі C:\\1\\");
            }

            Console.ReadKey();
        }
    }
}
