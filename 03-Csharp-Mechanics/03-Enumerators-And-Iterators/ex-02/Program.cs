using System;
using System.Collections;
using System.Collections.Generic;

namespace ex_02
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass MC = new MyClass();

            //Fill up the list.
            MC.Mylist = new List<int>() { 10, 5, 15, 20 };

            //We can now use foreach on our object.
            foreach (var item in MC)
                Console.WriteLine(item);

            Console.WriteLine("----");

            MySecondClass MSC = new MySecondClass();

            //Fill up the array.
            MSC.MyArray = new int[5] { 10, 5, 2, 3, 7 };

            //We can now use foreach on our object.
            foreach (var item in MSC)
                Console.WriteLine(item);

            Console.ReadLine();
        }
    }
}
