using System.Reflection;
using Shared.Common;

namespace GetFieldsPropertiesDemo
{
    class GetFieldsPropertiesDemo
    {
        static void Main()
        {
            // Get name of type
            Type t = typeof(Car);
            GetFields(t);
            GetProperties(t);

            Console.ReadLine();
        }

        // Display field names of type.
        public static void GetFields(Type t)
        {
            Console.WriteLine("***** Fields *****");
            FieldInfo[] fi = t.GetFields();
            foreach (FieldInfo field in fi)
                Console.WriteLine("->{0}", field.Name);
            Console.WriteLine("");
        }
        // Display property names of type.
        public static void GetProperties(Type t)
        {
            Console.WriteLine("***** Properties *****");
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo prop in pi)
                Console.WriteLine("->{0}", prop.Name);
            Console.WriteLine("");
        }
    }
}
