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

            // Шлях до файлу, який потрібно видалити
            string FilePath = @"C:\1\MyFileCopy.txt";

            // Перевірка, чи існує файл
            if (File.Exists(FilePath))
            {
                // Видалення файлу методом File.Delete
                File.Delete(FilePath);
                Console.WriteLine("Файл MyFileCopy.txt успішно видалено.");
            }
            else
            {
                Console.WriteLine("Файл MyFileCopy.txt НЕ існує у каталозі C:\\1\\");
            }

            Console.ReadKey();
        }
    }
}
