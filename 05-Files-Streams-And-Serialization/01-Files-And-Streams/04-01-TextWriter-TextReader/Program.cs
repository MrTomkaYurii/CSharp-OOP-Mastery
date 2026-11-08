using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileHandlingDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Підтримка української мови
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            WriteCharAsync();
            Console.ReadKey();
        }

        // Асинхронний метод для запису тексту у файл
        public static async void WriteCharAsync()
        {
            // Шлях до файлу
            string filePath = @"C:\1\MyFile2.txt";

            // Створюємо текстовий файл і отримуємо TextWriter
            using (TextWriter textWriter = File.CreateText(filePath))
            {
                // Асинхронний запис рядків
                await textWriter.WriteLineAsync("Hello TextWriter Abstract Class!");
                await textWriter.WriteLineAsync("File Handling Tutorial in C#");
            }

            Console.WriteLine("Асинхронний запис виконано успішно");
        }
    }
}
