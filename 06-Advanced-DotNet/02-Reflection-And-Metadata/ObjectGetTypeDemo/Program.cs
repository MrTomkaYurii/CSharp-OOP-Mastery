
using Shared.Common;

namespace ObjectGetTypeDemo
{
    class ObjectGetTypeDemo
    {
        static void Main(string[] args)
        {
            Car c = new Car();
            Type t = c.GetType();
            Console.WriteLine(t.FullName);
            Console.ReadLine();
        }
    }
}
