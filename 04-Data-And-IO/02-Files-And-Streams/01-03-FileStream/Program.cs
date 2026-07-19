using System;
using System.IO;
using System.Text;

namespace FileHandlinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Увімкнення коректного відображення української мови в консолі
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Вказуємо шлях до файлу, з якого будемо читати дані
            string FilePath = @"C:\1\MyFile.txt";

            // Змінна для зберігання даних, прочитаних із файлу
            string data;

            // Створюємо FileStream для читання файлу
            // FileMode.Open — відкрити файл
            // FileAccess.Read — доступ тільки для читання
            FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

            // Створюємо StreamReader для читання тексту зі Stream
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                // ReadToEnd читає весь вміст файлу до кінця та повертає як рядок
                data = streamReader.ReadToEnd();
            }

            // Виводимо прочитаний текст у консоль
            Console.WriteLine("Вміст файлу:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(data);
            Console.WriteLine("--------------------------------");

            Console.ReadKey();
        }
    }
}
