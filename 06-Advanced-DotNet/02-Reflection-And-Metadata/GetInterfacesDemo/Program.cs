using Shared.Common;

namespace GetInterfacesDemo
{
    class GetInterfacesDemo
    {
        static void Main()
        {
            // Get name of type
            Type t = typeof(Car);
            GetInterfaces(t);

            Console.ReadLine();
        }
        // Display implemented interfaces.
        public static void GetInterfaces(Type t)
        {
            Console.WriteLine("***** Interfaces *****");
            Type[] ifaces = t.GetInterfaces();
            foreach (Type i in ifaces)
                Console.WriteLine("->{0}", i.Name);
        }
    }
}
