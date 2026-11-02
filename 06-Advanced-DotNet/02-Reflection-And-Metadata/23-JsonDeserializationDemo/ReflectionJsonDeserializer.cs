using System.Reflection;
using System.Text.Json;

namespace JsonDeserializationDemo;

/// <summary>
/// Власний рушій десеріалізації JSON в об'єкт C# на базі рефлексії.
/// Демонструє: Activator.CreateInstance, пошук PropertyInfo за метаданими та PropertyInfo.SetValue.
/// </summary>
public static class ReflectionJsonDeserializer
{
    public static T Deserialize<T>(string json, bool traceSteps = false) where T : new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        Type targetType = typeof(T);

        // 1. Динамічно створюємо порожній екземпляр типу T у пам'яті через Activator
        T instance = (T)Activator.CreateInstance(targetType)!;

        if (traceSteps)
        {
            Console.WriteLine($"[Trace]: Створено порожній екземпляр '{targetType.Name}' через Activator.CreateInstance.");
        }

        PropertyInfo[] properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // 2. Розбираємо JSON-документ на пари "ключ -> значення"
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        foreach (JsonProperty jsonProp in root.EnumerateObject())
        {
            string jsonKey = jsonProp.Name;

            // 3. Шукаємо відповідну властивість C# за атрибутом [JsonField] або за іменем властивості
            PropertyInfo? matchedProperty = properties.FirstOrDefault(p =>
            {
                var fieldAttr = p.GetCustomAttribute<JsonFieldAttribute>();
                if (fieldAttr != null)
                {
                    return string.Equals(fieldAttr.KeyName, jsonKey, StringComparison.OrdinalIgnoreCase);
                }
                return string.Equals(p.Name, jsonKey, StringComparison.OrdinalIgnoreCase);
            });

            if (matchedProperty == null)
            {
                if (traceSteps)
                {
                    Console.WriteLine($"  [Невідомий ключ]: Поле JSON \"{jsonKey}\" не має відповідної властивості у класі {targetType.Name}.");
                }
                continue;
            }

            // 4. Перевіряємо захист [JsonExclude] та наявність сеттера CanWrite
            if (matchedProperty.IsDefined(typeof(JsonExcludeAttribute), inherit: true) || !matchedProperty.CanWrite)
            {
                if (traceSteps)
                {
                    Console.WriteLine($"  [Заблоковано]: Властивість '{matchedProperty.Name}' захищена [JsonExclude] або не має set.");
                }
                continue;
            }

            // 5. Конвертуємо значення з вузла JSON у цільовий тип властивості C# (prop.PropertyType)
            object? convertedValue = ConvertJsonElement(jsonProp.Value, matchedProperty.PropertyType);

            // 6. Записуємо значення у створений об'єкт через рефлексію: PropertyInfo.SetValue
            matchedProperty.SetValue(instance, convertedValue);

            if (traceSteps)
            {
                Console.WriteLine($"  [SetValue]: JSON \"{jsonKey}\" -> {matchedProperty.Name} ({matchedProperty.PropertyType.Name}) = {FormatPreview(convertedValue)}");
            }
        }

        return instance;
    }

    private static object? ConvertJsonElement(JsonElement element, Type targetType)
    {
        if (element.ValueKind == JsonValueKind.Null) return null;

        if (targetType == typeof(string)) return element.GetString();
        if (targetType == typeof(int)) return element.GetInt32();
        if (targetType == typeof(long)) return element.GetInt64();
        if (targetType == typeof(double)) return element.GetDouble();
        if (targetType == typeof(decimal)) return element.GetDecimal();
        if (targetType == typeof(bool)) return element.GetBoolean();

        if (targetType == typeof(string[]) && element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<string>();
            foreach (JsonElement item in element.EnumerateArray())
            {
                list.Add(item.GetString() ?? string.Empty);
            }
            return list.ToArray();
        }

        return null;
    }

    private static string FormatPreview(object? val)
    {
        if (val is string[] arr) return $"[{string.Join(", ", arr)}]";
        return val?.ToString() ?? "null";
    }
}
