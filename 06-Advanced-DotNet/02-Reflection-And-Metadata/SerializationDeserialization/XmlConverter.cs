using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace SerializationDeserialization
{
    /// <summary>
    /// Рефлексивний XML-конвертер (демонструє використання рефлексії в рушіях серіалізації).
    /// </summary>
    public static class XmlConverter
    {
        /// <summary>
        /// Серіалізує довільний об'єкт у XML за допомогою інспекції властивостей через рефлексію.
        /// </summary>
        public static string Serialize(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            Type type = obj.GetType();
            var sb = new StringBuilder();
            sb.AppendLine($"<{type.Name}>");

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                // Пропускаємо властивості з [XmlIgnoreProperty]
                if (prop.GetCustomAttribute<XmlIgnorePropertyAttribute>() != null)
                {
                    continue;
                }

                if (!prop.CanRead) continue;

                // Визначаємо ім'я тегу: або з [XmlElementName], або за замовчуванням ім'я властивості
                var nameAttr = prop.GetCustomAttribute<XmlElementNameAttribute>();
                string tagName = nameAttr?.Name ?? prop.Name;

                object? value = prop.GetValue(obj);
                string formattedValue = FormatValue(value);

                sb.AppendLine($"  <{tagName}>{formattedValue}</{tagName}>");
            }

            sb.AppendLine($"</{type.Name}>");
            return sb.ToString();
        }

        /// <summary>
        /// Десеріалізує XML-рядок у новий екземпляр типу T за допомогою рефлексії.
        /// </summary>
        public static T Deserialize<T>(string xml) where T : new()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(xml);

            var doc = XDocument.Parse(xml);
            if (doc.Root == null)
            {
                throw new InvalidOperationException("Неможливо розпарсити кореневий елемент XML.");
            }

            // Створюємо екземпляр типу T
            T instance = new T();
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (XElement element in doc.Root.Elements())
            {
                string elementName = element.Name.LocalName;

                // Шукаємо властивість за іменем тегу або атрибутом [XmlElementName]
                PropertyInfo? prop = properties.FirstOrDefault(p =>
                {
                    var attr = p.GetCustomAttribute<XmlElementNameAttribute>();
                    return attr?.Name == elementName || p.Name.Equals(elementName, StringComparison.OrdinalIgnoreCase);
                });

                if (prop != null && prop.CanWrite)
                {
                    object? convertedValue = ConvertValue(element.Value, prop.PropertyType);
                    prop.SetValue(instance, convertedValue);
                }
            }

            return instance;
        }

        private static string FormatValue(object? val)
        {
            if (val == null) return string.Empty;
            if (val is IFormattable formattable)
                return formattable.ToString(null, CultureInfo.InvariantCulture);
            return val.ToString() ?? string.Empty;
        }

        private static object? ConvertValue(string stringValue, Type targetType)
        {
            if (targetType == typeof(string)) return stringValue;
            if (targetType == typeof(int)) return int.Parse(stringValue, CultureInfo.InvariantCulture);
            if (targetType == typeof(double)) return double.Parse(stringValue, CultureInfo.InvariantCulture);
            if (targetType == typeof(bool)) return bool.Parse(stringValue);
            if (targetType == typeof(DateTime)) return DateTime.Parse(stringValue, CultureInfo.InvariantCulture);

            return Convert.ChangeType(stringValue, targetType, CultureInfo.InvariantCulture);
        }
    }
}
