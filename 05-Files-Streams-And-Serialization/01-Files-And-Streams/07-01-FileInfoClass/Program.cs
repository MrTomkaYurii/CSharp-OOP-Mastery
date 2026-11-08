using System;
using System.IO;
namespace FileInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\1\MyTestFile1.txt";
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.Create();
            {
                Console.WriteLine("File has been created");
            }
            Console.ReadKey();
        }
    }
}