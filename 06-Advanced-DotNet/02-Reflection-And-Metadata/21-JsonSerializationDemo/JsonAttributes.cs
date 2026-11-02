namespace JsonSerializationDemo;

/// <summary>
/// Користувацький атрибут для перевизначення імені ключа у вихідному JSON.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class JsonFieldAttribute : Attribute
{
    public string KeyName { get; }

    public JsonFieldAttribute(string keyName)
    {
        KeyName = keyName;
    }
}

/// <summary>
/// Користувацький атрибут для виключення властивості з процесу JSON-серіалізації.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class JsonExcludeAttribute : Attribute
{
}
