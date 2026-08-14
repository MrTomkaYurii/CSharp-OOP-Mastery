using System.Reflection;

namespace Shared
{
    class TypeGetTypeDemo
    {
        static void Main(string[] args)
        {
            // Obtain type information using the static Type.GetType() method.
            // (don't throw an exception if Car cannot be found and ignore case).
            var objAssembly = Assembly.LoadFrom("C:\\Users\\Yurii\\source\\repos\\OOP - Reflection\\LateBindingDemo\\bin\\Debug\\net8.0\\Shared.dll");

            //get the class type information in which late bindig applied
            Type t = objAssembly.GetType("Shared.Common.Car");

            // If class Car in same assembly
            // Type t = Type.GetType("Car", false, true);

            Console.WriteLine(t.FullName);
            Console.ReadLine();
        }
    }
}
