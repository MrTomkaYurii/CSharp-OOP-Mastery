namespace CreateCustomAttributes
{
    /// <summary>
    /// Атрибут для перевірки мінімального числового значення властивості.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : Attribute
    {
        public int Value { get; }
        public string? ErrorMessage { get; set; }

        public MinValueAttribute(int value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Атрибут для обов'язкових рядкових властивостей (не можуть бути null або порожніми).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : Attribute
    {
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Атрибут для обмеження довжини рядка.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringLengthAttribute : Attribute
    {
        public int MinLength { get; }
        public int MaxLength { get; }

        public StringLengthAttribute(int minLength, int maxLength)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }
}
