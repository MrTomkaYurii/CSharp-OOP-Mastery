using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace XmlDeserializationDemo;

/// <summary>
/// Власний рушій десеріалізації з XML в об'єкт C# на базі рефлексії.
/// Покроково зчитує XML-атрибути та дочірні XML-теги, конвертує типи через Convert.ChangeType
/// і заповнює властивості через PropertyInfo.SetValue.
/// </summary>
public static class ReflectionXmlDeserializer
{
    public static T Deserialize<T>(string xml, bool traceSteps = false) where T : new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(xml);

        XDocument doc = XDocument.Parse(xml);
        XElement root = doc.Root ?? throw new InvalidOperationException("Відсутній кореневий елемент XML.");

        Type targetType = typeof(T);

        // 1. Створюємо екземпляр цільового класу через Activator
        T instance = (T)Activator.CreateInstance(targetType)!;

        if (traceSteps)
        {
            Console.WriteLine($"[Trace]: Розбір кореневого тегу <{root.Name.LocalName}> для класу '{targetType.Name}'.");
        }

        PropertyInfo[] properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // 2. Етап А: Зчитування XML-атрибутів кореневого тегу (наприклад, environment="Production")
        foreach (XAttribute xmlAttr in root.Attributes())
        {
            string attrName = xmlAttr.Name.LocalName;

            PropertyInfo? prop = properties.FirstOrDefault(p =>
            {
                var attrMeta = p.GetCustomAttribute<XmlAttrAttribute>();
                return attrMeta != null && string.Equals(attrMeta.AttributeName, attrName, StringComparison.OrdinalIgnoreCase);
            });

            if (prop != null && prop.CanWrite && !prop.IsDefined(typeof(XmlExcludeAttribute), inherit: true))
            {
                object? converted = ConvertStringValue(xmlAttr.Value, prop.PropertyType);
                prop.SetValue(instance, converted);

                if (traceSteps)
                {
                    Console.WriteLine($"  [XML-Атрибут -> SetValue]: @{attrName}=\"{xmlAttr.Value}\" -> {prop.Name} ({prop.PropertyType.Name}) = {converted}");
                }
            }
        }

        // 3. Етап Б: Зчитування дочірніх XML-тегів (наприклад, <PortNumber>8080</PortNumber>)
        foreach (XElement element in root.Elements())
        {
            string tagName = element.Name.LocalName;

            PropertyInfo? prop = properties.FirstOrDefault(p =>
            {
                var tagMeta = p.GetCustomAttribute<XmlTagAttribute>();
                if (tagMeta != null)
                {
                    return string.Equals(tagMeta.TagName, tagName, StringComparison.OrdinalIgnoreCase);
                }
                return string.Equals(p.Name, tagName, StringComparison.OrdinalIgnoreCase);
            });

            if (prop == null)
            {
                if (traceSteps)
                {
                    Console.WriteLine($"  [Невідомий тег]: Елемент <{tagName}> не має відповідної властивості у {targetType.Name}.");
                }
                continue;
            }

            if (prop.IsDefined(typeof(XmlExcludeAttribute), inherit: true) || !prop.CanWrite)
            {
                if (traceSteps)
                {
                    Console.WriteLine($"  [Заблоковано]: Властивість '{prop.Name}' позначена [XmlExclude] або не має сеттера.");
                }
                continue;
            }

            object? convertedValue = ConvertStringValue(element.Value, prop.PropertyType);
            prop.SetValue(instance, convertedValue);

            if (traceSteps)
            {
                Console.WriteLine($"  [XML-Елемент -> SetValue]: <{tagName}>{element.Value}</{tagName}> -> {prop.Name} ({prop.PropertyType.Name}) = {convertedValue}");
            }
        }

        return instance;
    }

    private static object? ConvertStringValue(string rawText, Type targetType)
    {
        if (targetType == typeof(string)) return rawText;
        if (targetType == typeof(int)) return int.Parse(rawText, CultureInfo.InvariantCulture);
        if (targetType == typeof(double)) return double.Parse(rawText, CultureInfo.InvariantCulture);
        if (targetType == typeof(decimal)) return decimal.Parse(rawText, CultureInfo.InvariantCulture);
        if (targetType == typeof(bool)) return bool.Parse(rawText);

        return Convert.ChangeType(rawText, targetType, CultureInfo.InvariantCulture);
    }
}
