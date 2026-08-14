using Shared.Common;

namespace TypeofDemo
{
    class TypeofDemo
    {
        static void Main(string[] args)
        {
            // Get the Type using typeof.
            Type t = typeof(Car);
            Console.WriteLine(t.FullName);
            Console.ReadLine();
        }
    }
}
