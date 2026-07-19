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
            StreamReader streamReader = fileInfo.OpenText();
            string s = "";
            while ((s = streamReader.ReadLine()) != null)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();
        }
    }
}