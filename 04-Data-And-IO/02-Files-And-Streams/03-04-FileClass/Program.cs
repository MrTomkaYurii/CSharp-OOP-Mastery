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

            // Вказуємо шляхи до вихідного та копійованого файлів
            string SourceFilePath = @"C:\1\MyFile.txt";
            string DestinationFilePath = @"C:\1\MyFileCopy.txt";

            // Перевіряємо, чи існує вихідний файл
            if (File.Exists(SourceFilePath))
            {
                // Якщо вихідний файл існує — копіюємо його у новий файл
                // Параметр overwrite = true дозволяє перезаписувати файл-ціль, якщо він вже існує
                File.Copy(SourceFilePath, DestinationFilePath, true);

                // Зчитуємо вміст нового файлу
                string lines = File.ReadAllText(DestinationFilePath, Encoding.UTF8);

                // Виводимо вміст копії
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
