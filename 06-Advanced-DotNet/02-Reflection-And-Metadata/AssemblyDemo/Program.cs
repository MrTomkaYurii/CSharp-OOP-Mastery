using System.Reflection;

namespace Shared
{
    class AssemblyDemo
    {
        static void Main()
        {
            Assembly objAssembly;
            // You must supply a valid fully qualified assembly name here.            
            objAssembly = Assembly.Load("mscorlib,2.0.0.0,Neutral,b77a5c561934e089");
            
            // Loads an assembly using its file name   
            //SampleAssembly = Assembly.LoadFrom(@"C:\Windows\Microsoft.NET\Framework\v1.1.4322\caspol.exe");

            //this loads currnly running process assembly
            //SampleAssembly = Assembly.GetExecutingAssembly();

            Type[] Types = objAssembly.GetTypes();
            // Display all the types contained in the specified assembly.
            foreach (Type objType in Types)
            {
                Console.WriteLine(objType.Name.ToString());
            }

            //fetching custom attributes within an assembly
            Attribute[] arrayAttributes =
             Attribute.GetCustomAttributes(objAssembly);
            // assembly1 is an Assembly object

            foreach (Attribute attrib in arrayAttributes)
            {
                Console.WriteLine(attrib.TypeId);
            }

            Console.ReadLine();
        }
    }
}
