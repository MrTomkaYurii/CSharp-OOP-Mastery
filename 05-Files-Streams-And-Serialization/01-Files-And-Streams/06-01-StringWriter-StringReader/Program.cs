using System;
using System.Text;
using System.IO;

namespace StringWriter_StringReader_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Hello. This is First Line \nHello. This is Second Line \nHello. This is Third Line";
            //Writing string into StringBuilder
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);

            //Store Data on StringBuilder
            stringWriter.WriteLine(text);
            stringWriter.Flush();
            stringWriter.Close();

            //Read Entry
            StringReader reader = new StringReader(stringBuilder.ToString());
            //Check to End of File
            while (reader.Peek() > -1)
            {
                Console.WriteLine(reader.ReadLine());
            }

            Console.ReadKey();
        }
    }
}