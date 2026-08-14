using Shared.Common;
using System.Reflection;

namespace GetConstructorInfoDemo
{
    class GetConstructorInfoDemo
    {
        static void Main()
        {
            // Get name of type
            Type t = typeof(Car);
            GetConstructorsInfo(t);

            Console.ReadLine();
        }
        // Display method names of type.
        public static void GetConstructorsInfo(Type t)
        {
            Console.WriteLine("***** ConstructorsInfo *****");
            ConstructorInfo[] ci = t.GetConstructors();
            foreach (ConstructorInfo c in ci)
                Console.WriteLine(c.ToString());
            Console.WriteLine("");
        }
    }
}
