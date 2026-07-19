using System;
using System.IO;
namespace FileInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string path1 = @"D:\1\MyTestFile1.txt";
            string path2 = @"D:\1\MyTestFile2.txt";

            FileInfo fileInfo1 = new FileInfo(path1);
            FileInfo fileInfo2 = new FileInfo(path2);
            fileInfo1.CopyTo(path2);

            Console.WriteLine("{0} was copied to {1}.", path1, path2);
            Console.ReadKey();
        }
    }
}