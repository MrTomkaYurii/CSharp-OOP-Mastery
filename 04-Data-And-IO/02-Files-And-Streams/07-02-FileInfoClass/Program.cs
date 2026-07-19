using System;
using System.IO;
namespace FileInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\1\MyTestFile2.txt";
            FileInfo fileInfo = new FileInfo(path);
            StreamWriter str = fileInfo.CreateText();
            str.WriteLine("Hello");
            Console.WriteLine("File has been created with text");
            str.Close();
            Console.ReadKey();
        }
    }
}