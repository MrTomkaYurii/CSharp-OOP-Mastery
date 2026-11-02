namespace XmlDeserializationDemo;

/// <summary>
/// Атрибут рівня класу для зіставлення кореневого тегу XML.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class XmlRootTagAttribute : Attribute
{
    public string RootName { get; }

    public XmlRootTagAttribute(string rootName) => RootName = rootName;
}

/// <summary>
/// Атрибут для зчитування значення властивості з атрибута кореневого тегу XML.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class XmlAttrAttribute : Attribute
{
    public string AttributeName { get; }

    public XmlAttrAttribute(string attributeName) => AttributeName = attributeName;
}

/// <summary>
/// Атрибут для зчитування значення властивості з дочірнього елемента (тегу) XML.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class XmlTagAttribute : Attribute
{
    public string TagName { get; }

    public XmlTagAttribute(string tagName) => TagName = tagName;
}

/// <summary>
/// Атрибут для блокування запису у властивість під час десеріалізації XML.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class XmlExcludeAttribute : Attribute
{
}
