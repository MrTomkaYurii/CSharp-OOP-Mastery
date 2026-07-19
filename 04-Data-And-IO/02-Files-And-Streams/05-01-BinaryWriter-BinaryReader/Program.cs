using System;
using System.IO;
using System.Text;

namespace FileHandlingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Підтримка української мови
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            WriteDataToBinaryFile();
            ReadDataFromBinaryFile();

            Console.ReadKey();
        }

        static void WriteDataToBinaryFile()
        {
            // Запис бінарних даних у файл
            using (BinaryWriter writer = new BinaryWriter(File.Open(@"C:\1\MyBinaryFile2.bin", FileMode.Create)))
            {
                writer.Write("0x80234400");
                writer.Write("Windows Explorer Has Stopped Working");
                writer.Write(true);
            }
        }

        static void ReadDataFromBinaryFile()
        {
            // Читання бінарного файлу
            using (BinaryReader reader = new BinaryReader(File.Open(@"C:\1\MyBinaryFile2.bin", FileMode.Open)))
            {
                Console.WriteLine("Error Code : " + reader.ReadString());
                Console.WriteLine("Message : " + reader.ReadString());
                Console.WriteLine("Restart Explorer : " + reader.ReadBoolean());
            }
        }
    }
}
