using System;
using System.IO;

namespace FileInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = @"D:\1\MyTestFile1.txt";
            string destinationPath = @"D:\1\Dotnet\MyTestFile1.txt";
            FileInfo fileInfo = new FileInfo(sourcePath);
            fileInfo.MoveTo(destinationPath);
            Console.WriteLine("{0} was moved to {1}.", sourcePath, destinationPath);
            Console.ReadKey();
        }
    }
}