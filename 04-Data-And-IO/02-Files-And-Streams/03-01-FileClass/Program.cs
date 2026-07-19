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

            // Перевірка існування файлу за допомогою File.Exists
            if (File.Exists(FilePath))
            {
                Console.WriteLine("Файл MyFile.txt існує у каталозі C:\\1\\");
            }
            else
            {
                Console.WriteLine("Файл MyFile.txt НЕ існує у каталозі C:\\1\\");
            }

            Console.ReadKey();
        }
    }
}
