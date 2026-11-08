using System;
using System.IO;

namespace FileInfoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"D:\1\FileInfo1.txt";
            FileInfo fileInfo = new FileInfo(filePath);
            //Create File
            using (StreamWriter sw = fileInfo.CreateText())
            {
                sw.WriteLine("Hello FileInfo Class");
            }

            //Display File Info            
            Console.WriteLine("File Created on : " + fileInfo.CreationTime);
            Console.WriteLine("Directory Name : " + fileInfo.DirectoryName);
            Console.WriteLine("Name of File : " + fileInfo.Name);
            Console.WriteLine("Full Name of File : " + fileInfo.FullName);
            Console.WriteLine("Length of File : " + fileInfo.Length);
            Console.WriteLine("Is Readonly : " + fileInfo.IsReadOnly);
            Console.WriteLine("File is Last Accessed on : " + fileInfo.LastAccessTime);

            //Deleting File
            Console.WriteLine("Press small y for delete this file");
            try
            {
                char ch = Convert.ToChar(Console.ReadLine());
                if (ch == 'y')
                {
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                        Console.WriteLine(filePath + " Deleted Successfully");
                    }
                    else
                    {
                        Console.WriteLine("File doesn't exist");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Press Anykey to Exit...");
            }
            Console.ReadKey();
        }
    }
}