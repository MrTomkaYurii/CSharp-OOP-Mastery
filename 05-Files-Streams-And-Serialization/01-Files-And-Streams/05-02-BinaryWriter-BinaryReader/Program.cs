using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binary_writer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string filePath = @"C:\1\test.txt";

            //write in the stream
            using (BinaryWriter bw = new BinaryWriter(File.Open(filePath,
                                    FileMode.Create), Encoding.UTF8, false))
            {
                bw.Write(76);
                bw.Write(1.2);
                bw.Write('a');
                bw.Write("Geeks");
                bw.Write(false);
            }
            if (File.Exists(filePath))
            {
                using (BinaryReader br = new BinaryReader(File.Open(filePath,
                                         FileMode.Open), Encoding.UTF8))
                {
                    //Reads the data to the stream
                    Console.WriteLine("int value is " + br.ReadInt32());
                    Console.WriteLine("Double value is " + br.ReadDouble());
                    Console.WriteLine("Char value is " + br.ReadChar());
                    Console.WriteLine("value of string is " + br.ReadString());
                    Console.WriteLine("for boolean value is " + br.ReadBoolean());
                    Console.Read();

                }

            }

        }
    }
}