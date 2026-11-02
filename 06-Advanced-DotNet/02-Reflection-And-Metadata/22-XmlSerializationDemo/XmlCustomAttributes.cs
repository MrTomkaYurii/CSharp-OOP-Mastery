namespace XmlSerializationDemo;

/// <summary>
/// Атрибут рівня класу для задання імені кореневого тегу XML.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class XmlRootTagAttribute : Attribute
{
    public string RootName { get; }

    public XmlRootTagAttribute(string rootName) => RootName = rootName;
}

/// <summary>
/// Атрибут для запису властивості як атрибута всередині відкриваючого кореневого тегу XML.
/// Наприклад: &lt;Employee id="105"&gt;
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class XmlAttrAttribute : Attribute
{
    public string AttributeName { get; }

    public XmlAttrAttribute(string attributeName) => AttributeName = attributeName;
}

/// <summary>
/// Атрибут для перевизначення імені дочірнього XML-тегу.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class XmlTagAttribute : Attribute
{
    public string TagName { get; }

    public XmlTagAttribute(string tagName) => TagName = tagName;
}

/// <summary>
/// Атрибут для ігнорування властивості під час XML-серіалізації.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class XmlExcludeAttribute : Attribute
{
}
