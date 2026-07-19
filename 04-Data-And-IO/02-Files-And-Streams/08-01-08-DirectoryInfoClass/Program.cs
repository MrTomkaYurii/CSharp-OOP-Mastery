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
            fl.Create();
            {
                Console.WriteLine("Directory has been created");
            }
            Console.ReadKey();
        }
    }
}