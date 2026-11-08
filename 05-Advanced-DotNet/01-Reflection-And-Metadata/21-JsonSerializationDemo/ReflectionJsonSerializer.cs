using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace JsonSerializationDemo;

/// <summary>
/// Власний рушій серіалізації в JSON, побудований виключно на рефлексії (Type, PropertyInfo, GetValue).
/// Дозволяє студенту побачити, що саме відбувається всередині стандартного JsonSerializer.
/// </summary>
public static class ReflectionJsonSerializer
{
    public static string Serialize(object obj, bool traceSteps = false)
    {
        ArgumentNullException.ThrowIfNull(obj);

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (traceSteps)
        {
            Console.WriteLine($"[Trace]: Інспектуємо тип '{type.Name}'. Знайдено публічних властивостей: {properties.Length}");
        }

        var jsonEntries = new List<string>();

        foreach (PropertyInfo prop in properties)
        {
            if (!prop.CanRead) continue;

            // 1. Перевіряємо наявність атрибута [JsonExclude]
            if (prop.IsDefined(typeof(JsonExcludeAttribute), inherit: true))
            {
                if (traceSteps)
                {
                    Console.WriteLine($"  [Пропущено]: Властивість '{prop.Name}' позначена атрибутом [JsonExclude].");
                }
                continue;
            }

            // 2. Визначаємо ім'я ключа JSON (з атрибута [JsonField] або оригінальне ім'я властивості)
            var fieldAttr = prop.GetCustomAttribute<JsonFieldAttribute>();
            string jsonKey = fieldAttr?.KeyName ?? prop.Name;

            // 3. Динамічно зчитуємо значення властивості з об'єкта у пам'яті через рефлексію
            object? rawValue = prop.GetValue(obj);

            // 4. Форматуємо значення згідно зі стандартом синтаксису JSON
            string formattedValue = FormatJsonValue(rawValue);

            if (traceSteps)
            {
                Console.WriteLine($"  [Серіалізовано]: {prop.Name} ({prop.PropertyType.Name}) -> \"{jsonKey}\": {formattedValue}");
            }

            jsonEntries.Add($"  \"{jsonKey}\": {formattedValue}");
        }

        var sb = new StringBuilder();
        sb.AppendLine("{");
        sb.AppendLine(string.Join(",\n", jsonEntries));
        sb.Append('}');

        return sb.ToString();
    }

    private static string FormatJsonValue(object? value)
    {
        if (value == null)
        {
            return "null";
        }

        // Рядки в JSON обов'язково беруться у подвійні лапки
        if (value is string str)
        {
            string escaped = str.Replace("\\", "\\\\").Replace("\"", "\\\"");
            return $"\"{escaped}\"";
        }

        // Логічні значення в JSON пишуться малими літерами: true або false
        if (value is bool boolVal)
        {
            return boolVal ? "true" : "false";
        }

        // Числові типи форматуються з крапкою як роздільником дробової частини (InvariantCulture)
        if (value is int or long or double or float or decimal)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "0";
        }

        // Колекції та масиви записуються у квадратних дужках: [elem1, elem2]
        if (value is IEnumerable enumerable)
        {
            var elements = new List<string>();
            foreach (object? item in enumerable)
            {
                elements.Add(FormatJsonValue(item));
            }
            return $"[{string.Join(", ", elements)}]";
        }

        return $"\"{value}\"";
    }
}
