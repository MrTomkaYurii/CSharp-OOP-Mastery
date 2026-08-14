using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SerializationDeserialization
{
    public static class XmlConverter
    {
        // DeSerialize() method
        public static T Deserialize<T>(string s)
        {
            // instantiate the object to be deserialized
            var assembly = typeof(T).Assembly;
            var obj = assembly.CreateInstance(typeof(T).FullName);
            if (obj == null)
            {
                throw new Exception($"Can't instantiate a new object of type '{typeof(T).FullName}'");
            }

            // parse input string int XML document
            var doc = XDocument.Parse(s);
            if (doc.Root == null)
            {
                throw new ArgumentException("Can't parse specified xml string");
            }

            // deserialize class members states
            foreach (var node in doc.Root.Elements())
            {
                var property =
                    obj.GetType()
                       .GetProperty(node.Name.LocalName, BindingFlags.Public | BindingFlags.Instance);
                if (null != property && property.CanWrite)
                {
                    if (typeof(int) == property.PropertyType)
                    {
                        property.SetValue(obj, int.Parse(node.Value));
                    }
                    else if (typeof(string) == property.PropertyType)
                    {
                        property.SetValue(obj, node.Value);
                    }
                    // TODO: add other types conversion
                }
            }
            return (T)obj;
        }


        // Serialize() method
        public static string Serialize(object obj)
        {
            var sb = new StringBuilder();
            var type = obj.GetType();

            sb.AppendLine("<" + type.Name + ">");

            var props = new List<PropertyInfo>(type.GetProperties());
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(obj, null);
                sb.AppendLine("\t<" + prop.Name + ">" + propValue + "</" + prop.Name + ">");
            }

            sb.AppendLine("</" + type.Name + ">");

            return sb.ToString();
        }
    }
}
