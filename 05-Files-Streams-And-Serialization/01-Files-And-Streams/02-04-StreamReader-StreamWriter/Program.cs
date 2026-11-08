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

            // Створення змінної для збереження шляху до файлу
            string filePath = @"C:\1\MyFile.txt";

            // Створення екземпляра StreamReader для читання даних із файлу
            StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);

            Console.WriteLine("Вміст файлу:");

            // Це використовується, щоб вказати, з якої позиції починати читання потоку
            // BaseStream: повертає базовий (внутрішній) потік
            // Seek: встановлює нову позицію в потоці
            // SeekOrigin.Begin: означає початок потоку
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

            // ReadLine читає один рядок символів з поточного потоку
            // Повертає наступний рядок, або null якщо досягнуто кінця файлу
            string strData = streamReader.ReadLine();

            // Читати весь файл пострічково, поки strData не стане null
            while (strData != null)
            {
                // Виводимо прочитаний рядок
                Console.WriteLine(strData);

                // Читаємо наступний рядок
                strData = streamReader.ReadLine();
            }

            // Закриття об’єкта StreamReader
            streamReader.Close();

            Console.WriteLine("\nФайл успішно прочитано.");
            Console.ReadKey();
        }
    }
}
