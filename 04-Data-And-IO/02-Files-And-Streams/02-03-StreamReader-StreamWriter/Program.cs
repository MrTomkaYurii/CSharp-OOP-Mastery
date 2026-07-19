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

            // Вказуємо шлях до файлу, у який будемо дописувати дані
            string filePath = @"C:\1\MyFile.txt";

            // Виконуємо деякі базові арифметичні операції
            int a = 15;
            int b = 20;
            int result = a + b;

            // Тепер потрібно зберегти значення a, b та result у файл

            // Створюємо StreamWriter у режимі Append (true)
            // true — означає, що дані будуть ДОПИСАНІ, а не перезаписані
            using (StreamWriter streamWriter = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                streamWriter.WriteLine($"Сума {a} + {b} = {result}");
            }

            Console.WriteLine("Дані змінних успішно дописані у файл.");
            Console.ReadKey();
        }
    }
}
