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

            // Вказуємо шлях до існуючого файлу, в який потрібно дописати дані
            string FilePath = @"C:\1\MyFile.txt";

            // Створюємо об'єкт FileStream у режимі Append — дописати в існуючий файл
            FileStream fileStream = new FileStream(FilePath, FileMode.Append);

            // Створюємо масив байтів, що містить текст, який потрібно дописати
            byte[] bytedata = Encoding.UTF8.GetBytes("C# — це об’єктно-орієнтована мова програмування");

            // Записуємо байти у файл
            // bytedata — масив даних
            // 0 — початкова позиція
            // bytedata.Length — кількість байтів для запису
            fileStream.Write(bytedata, 0, bytedata.Length);

            // Закриваємо потік
            fileStream.Close();

            Console.WriteLine("Успішно дописано у файл: C# — це об’єктно-орієнтована мова програмування");
            Console.ReadKey();
        }
    }
}
