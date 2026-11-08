using System;
using System.IO;

namespace DirectoryInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            String path = @"D:\MyTestFile1";
            DirectoryInfo fl = new DirectoryInfo(path);
            DirectoryInfo dis = fl.CreateSubdirectory("MyTestFile2");
            {
                Console.WriteLine("SubDirectory has been created");
            }
            Console.ReadKey();
        }
    }
}