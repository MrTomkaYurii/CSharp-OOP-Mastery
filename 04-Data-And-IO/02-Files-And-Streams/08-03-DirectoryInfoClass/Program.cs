using System;
using System.IO;

namespace DirectoryInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            String path1 = @"D:\MyTestFile1";
            string path2 = @"D:\NewTestFile1";
            DirectoryInfo directoryInfo1 = new DirectoryInfo(path1);
            DirectoryInfo directoryInfo2 = new DirectoryInfo(path2);
            directoryInfo1.MoveTo(path2);
            {
                Console.WriteLine("Directory has been Moved");
            }

            Console.ReadKey();
        }
    }
}