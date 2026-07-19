using System;
using System.IO;

namespace StringWriter_StringReader_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = @"You are reading
                            this StringWriter and StringReader in C# lesson at
                            dot net class";

            using (StringReader rtringReader = new StringReader(text))
            {
                int count = 0;
                string line;
                while ((line = rtringReader.ReadLine()) != null)
                {
                    count++;
                    Console.WriteLine("Line {0}: {1}", count, line);
                }
            }
            Console.ReadKey();
        }
    }
}