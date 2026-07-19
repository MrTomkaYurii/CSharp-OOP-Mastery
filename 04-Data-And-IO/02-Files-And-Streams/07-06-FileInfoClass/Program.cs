using System;
using System.IO;
namespace FileInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string Path = @"D:\1\MyTestFile1.txt";
            FileInfo fileInfo = new FileInfo(Path);
            StreamWriter streamWriter = fileInfo.AppendText();
            streamWriter.WriteLine("This");
            streamWriter.WriteLine("is Extra");
            streamWriter.WriteLine("Text");
            Console.WriteLine("File has been appended");
            streamWriter.Close();
            Console.ReadKey();
        }
    }
}