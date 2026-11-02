namespace JsonDeserializationDemo;

/// <summary>
/// Атрибут для зіставлення JSON-ключа з властивістю C# під час десеріалізації.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class JsonFieldAttribute : Attribute
{
    public string KeyName { get; }

    public JsonFieldAttribute(string keyName) => KeyName = keyName;
}

/// <summary>
/// Атрибут для блокування запису у властивість під час десеріалізації з JSON.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class JsonExcludeAttribute : Attribute
{
}
