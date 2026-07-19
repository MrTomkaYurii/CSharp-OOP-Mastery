using System;
using System.IO;
using System.Text;

namespace FileHandlinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Увімкнення коректного виведення української мови в консоль
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Шлях до файлу, у який будемо записувати дані
            string FilePath = @"C:\1\MyFile.txt";

            // Створення екземпляра StreamWriter за шляхом до файлу
            // Якщо файл існує — він буде перезаписаний
            // Якщо файлу немає — він буде створений
            using (StreamWriter streamWriter = new StreamWriter(FilePath, false, Encoding.UTF8))
            {
                // Запитуємо у користувача текст для запису у файл
                Console.WriteLine("Введіть текст, який потрібно записати у файл:");

                // Отримуємо введені користувачем дані
                string inputData = Console.ReadLine();

                // Записуємо дані у файл
                streamWriter.Write(inputData);

                Console.WriteLine("Дані успішно записані у файл.");
            }

            Console.ReadKey();
        }
    }
}
