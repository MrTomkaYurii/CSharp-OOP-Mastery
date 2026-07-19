using System;
using System.IO;
namespace DirectoryInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\NewTestFile1 ";
            DirectoryInfo directoryInfo1 = new DirectoryInfo(path);
            directoryInfo1.Delete();
            {
                Console.WriteLine("Directory has been deleted");
            }
            Console.ReadKey();
        }
    }
}