using System;
using System.IO;

namespace FileHandlinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Увімкнення коректного виводу української мови в консолі
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Вказуємо шлях до файлу, який потрібно створити на диску
            string FilePath = @"C:\1\MyFile.txt";

            // Створюємо екземпляр FileStream із режимом Create (створити або перезаписати файл)
            FileStream fileStream = new FileStream(FilePath, FileMode.Create);

            // Закриваємо потік
            fileStream.Close();

            Console.Write("Файл було створено. Шлях: C:\\1\\MyFile.txt");
            Console.ReadKey();
        }
    }
}
