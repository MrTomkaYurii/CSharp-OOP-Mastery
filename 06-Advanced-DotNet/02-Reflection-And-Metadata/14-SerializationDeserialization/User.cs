namespace SerializationDeserialization
{
    /// <summary>
    /// Атрибут для ігнорування властивості при рефлексивній серіалізації.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlIgnorePropertyAttribute : Attribute
    {
    }

    /// <summary>
    /// Атрибут для зміни імені XML-тегу властивості.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlElementNameAttribute : Attribute
    {
        public string Name { get; }
        public XmlElementNameAttribute(string name) => Name = name;
    }

    /// <summary>
    /// Тестова модель для серіалізації/десеріалізації.
    /// </summary>
    public class User
    {
        [XmlElementName("FullName")]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public double Rating { get; set; }

        public bool IsActive { get; set; }

        [XmlIgnoreProperty]
        public string SecretApiKey { get; set; } = "SECRET-12345";
    }
}
