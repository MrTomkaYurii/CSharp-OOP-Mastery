using System.Globalization;
using System.Reflection;
using System.Text;

namespace XmlSerializationDemo;

/// <summary>
/// Власний рушій XML-серіалізації на базі рефлексії.
/// Демонструє роботу з атрибутами класу ([XmlRootTag]) та властивостей ([XmlAttr], [XmlTag], [XmlExclude]).
/// </summary>
public static class ReflectionXmlSerializer
{
    public static string Serialize(object obj, bool traceSteps = false)
    {
        ArgumentNullException.ThrowIfNull(obj);

        Type type = obj.GetType();

        // 1. Визначаємо назву кореневого тегу з атрибута класу [XmlRootTag] або беремо ім'я класу
        var rootAttr = type.GetCustomAttribute<XmlRootTagAttribute>();
        string rootTagName = rootAttr?.RootName ?? type.Name;

        if (traceSteps)
        {
            Console.WriteLine($"[Trace]: Тип '{type.Name}' -> Кореневий тег XML: <{rootTagName}>");
        }

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var xmlAttributes = new List<string>();
        var xmlChildElements = new List<string>();

        foreach (PropertyInfo prop in properties)
        {
            if (!prop.CanRead) continue;

            // 2. Перевіряємо, чи слід пропустити властивість ([XmlExclude])
            if (prop.IsDefined(typeof(XmlExcludeAttribute), inherit: true))
            {
                if (traceSteps)
                {
                    Console.WriteLine($"  [Пропущено]: Властивість '{prop.Name}' позначена [XmlExclude].");
                }
                continue;
            }

            object? rawValue = prop.GetValue(obj);
            string formattedValue = FormatXmlValue(rawValue);

            // 3. Перевіряємо, чи властивість є XML-атрибутом відкриваючого тегу ([XmlAttr])
            var attrMeta = prop.GetCustomAttribute<XmlAttrAttribute>();
            if (attrMeta != null)
            {
                string attrName = attrMeta.AttributeName;
                xmlAttributes.Add($"{attrName}=\"{formattedValue}\"");

                if (traceSteps)
                {
                    Console.WriteLine($"  [XML-Атрибут]: {prop.Name} -> @{attrName}=\"{formattedValue}\"");
                }
                continue;
            }

            // 4. Інакше записуємо як вкладений XML-тег ([XmlTag] або ім'я властивості)
            var tagMeta = prop.GetCustomAttribute<XmlTagAttribute>();
            string tagName = tagMeta?.TagName ?? prop.Name;

            xmlChildElements.Add($"  <{tagName}>{formattedValue}</{tagName}>");

            if (traceSteps)
            {
                Console.WriteLine($"  [XML-Елемент]: {prop.Name} -> <{tagName}>{formattedValue}</{tagName}>");
            }
        }

        // 5. Збираємо підсумковий XML-документ
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

        string attributesPart = xmlAttributes.Count > 0
            ? " " + string.Join(" ", xmlAttributes)
            : string.Empty;

        sb.AppendLine($"<{rootTagName}{attributesPart}>");
        foreach (string child in xmlChildElements)
        {
            sb.AppendLine(child);
        }
        sb.Append($"</{rootTagName}>");

        return sb.ToString();
    }

    private static string FormatXmlValue(object? val)
    {
        if (val == null) return string.Empty;
        if (val is bool b) return b ? "true" : "false";
        if (val is IFormattable formattable)
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        return val.ToString() ?? string.Empty;
    }
}
