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

            // Вказуємо шлях до файлу, у який будемо записувати дані
            string filePath = @"C:\1\MyFile.txt";

            // Виконуємо деякі базові операції з числами
            int a = 15;
            int b = 20;
            int result = a + b;

            // Тепер потрібно записати значення a, b та result у файл

            // Створюємо об'єкт StreamWriter у блоці using
            // using автоматично викликає Flush та Close, навіть у разі помилок
            using (StreamWriter streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                streamWriter.Write($"Сума {a} + {b} = {result}");
            }

            Console.WriteLine("Дані змінних збережено у файл.");
            Console.ReadKey();
        }
    }
}
