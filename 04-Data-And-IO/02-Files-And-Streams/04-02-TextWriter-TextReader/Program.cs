using System;
using System.IO;
using System.Text;

namespace FileHandlinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Підтримка української мови в консолі
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Шлях до файлу
            string filePath = @"C:\1\MyFile1.txt";

            // Читання одного рядка
            using (TextReader textReader = File.OpenText(filePath))
            {
                Console.WriteLine(textReader.ReadLine());
            }

            // Читання перших 4 символів
            using (TextReader textReader = File.OpenText(filePath))
            {
                char[] ch = new char[4];
                textReader.ReadBlock(ch, 0, 4);
                Console.WriteLine(ch);
            }

            // Читання всього файлу
            using (TextReader textReader = File.OpenText(filePath))
            {
                Console.WriteLine(textReader.ReadToEnd());
            }

            Console.ReadKey();
        }
    }
}
