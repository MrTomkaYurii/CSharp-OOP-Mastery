using System;
using System.IO;
using System.Text;

namespace FileHandlinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Увімкнення підтримки української мови
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Вказуємо шлях до файлу
            string filePath = @"C:\1\MyTextFile.txt";

            // Запис даних у текстовий файл за допомогою StreamWriter
            using (StreamWriter streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                streamWriter.WriteLine("Welcome to DotNetTutorials");
                streamWriter.WriteLine("You are Learning File Handling in C#");
            }

            // Читання файлу методом ReadToEnd
            Console.WriteLine("\nПідхід 1: використання ReadToEnd");
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            // Читання файлу методом ReadLine
            Console.WriteLine("\nПідхід 2: використання ReadLine");
            StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string strData = streamReader.ReadLine();

            while (strData != null)
            {
                Console.WriteLine(strData);
                strData = streamReader.ReadLine();
            }

            streamReader.Close();
            Console.ReadKey();
        }
    }
}
