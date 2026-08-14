using System.Reflection;
using Shared.Common;

namespace GetMethodsDemo
{
    class GetMethodsDemo
    {
        static void Main()
        {
            // Get name of type
            Type t = typeof(Car);
            GetMethod(t);
            GetMethods(t);

            Console.ReadLine();
        }
        // Display method names of type.
        public static void GetMethods(Type t)
        {
            Console.WriteLine("***** Methods *****");
            MethodInfo[] mi = t.GetMethods();
            foreach (MethodInfo m in mi)
                Console.WriteLine("->{0}", m.Name);
            Console.WriteLine("");
        }
        // Display method name of type.
        public static void GetMethod(Type t)
        {
            Console.WriteLine("***** Method *****");
            //This searches for name is case-sensitive. 
            //The search includes public static and public instance methods.
            MethodInfo mi = t.GetMethod("IsMoving");
            Console.WriteLine("->{0}", mi.Name);
            Console.WriteLine("");
        }
    }
}
