using System;
using System.IO;

namespace DirectoryInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string DirectoryPath = @"D:\Dotnet";
            DirectoryInfo directoryInfo = new DirectoryInfo(DirectoryPath);
            try
            {
                if (directoryInfo.Exists)
                {
                    Console.WriteLine("{0} Directory is already exists", DirectoryPath);
                    Console.WriteLine("Directory Name : " + directoryInfo.Name);
                    Console.WriteLine("Path : " + directoryInfo.FullName);
                    Console.WriteLine("Directory is created on : " + directoryInfo.CreationTime);
                    Console.WriteLine("Directory is Last Accessed on " + directoryInfo.LastAccessTime);
                }
                else
                {
                    directoryInfo.Create();
                    Console.WriteLine(DirectoryPath + "Directory is created successfully");
                }
                //Delete this directory
                Console.WriteLine("If you want to delete this directory press small y. Press any key to exit.");
                try
                {
                    char ch = Convert.ToChar(Console.ReadLine());
                    if (ch == 'y')
                    {
                        if (directoryInfo.Exists)
                        {
                            directoryInfo.Delete();
                            Console.WriteLine(DirectoryPath + "Directory Deleted");
                        }
                        else
                        {
                            Console.WriteLine(DirectoryPath + "Directory Not Exists");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Press Enter to Exit");
                }
                Console.ReadKey();
            }
            catch (DirectoryNotFoundException d)
            {
                Console.WriteLine("Exception raised : " + d.Message);
                Console.ReadKey();
            }
        }
    }
}