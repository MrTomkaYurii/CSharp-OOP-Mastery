using System;
using System.Collections;
using System.Collections.Generic;

class MyClass
{
    public IEnumerator<string> GetEnumerator()       // Returns the enumerator
    {
        return BlackAndWhite();
    }

    public IEnumerator<string> BlackAndWhite()      // Iterator
    {
        yield return "black";
        yield return "gray";
        yield return "white";
    }
}
//Alternatively, you can implement the enumerator manually:
//class BlackAndWhiteEnumerator : IEnumerator<string>
//{
//    int position = -1;
//    public string Current
//    {
//        get
//        {
//            switch (position)
//            {
//                case 0: return "black";
//                case 1: return "gray";
//                case 2: return "white";
//                default: throw new InvalidOperationException();
//            }
//        }
//    }
//    object IEnumerator.Current => Current;
//    public bool MoveNext()
//    {
//        position++;
//        return position < 3;
//    }
//    public void Reset() => position = -1;
//    public void Dispose() { }
//}

class Program
{
    static void Main()
    {
        MyClass mc = new MyClass();

        foreach (string shade in mc)
            Console.WriteLine(shade);
    }
}