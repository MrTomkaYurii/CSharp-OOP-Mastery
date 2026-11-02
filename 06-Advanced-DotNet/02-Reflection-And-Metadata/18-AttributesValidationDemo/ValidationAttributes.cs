namespace AttributesValidationDemo;

// =========================================================================
// ЧАСТИНА 1: АТРИБУТИ ВАЛІДАЦІЇ НА РІВНІ КЛАСУ (AttributeTargets.Class)
// Використовуються, коли потрібно перевірити бізнес-правило всього об'єкта
// або узгодженість декількох властивостей між собою.
// =========================================================================

/// <summary>
/// Атрибут рівня класу: перевіряє, щоб числове значення однієї властивості (MinProperty)
/// не перевищувало значення іншої властивості (MaxProperty).
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RangeConsistencyAttribute : Attribute
{
    public string MinPropertyName { get; }
    public string MaxPropertyName { get; }
    public string? ErrorMessage { get; set; }

    public RangeConsistencyAttribute(string minPropertyName, string maxPropertyName)
    {
        MinPropertyName = minPropertyName;
        MaxPropertyName = maxPropertyName;
    }
}

/// <summary>
/// Атрибут рівня класу: перевіряє комплексне бізнес-правило стажу роботи відносно віку
/// (наприклад, стаж роботи не може перевищувати: Вік - 18 років).
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ExperienceAgeConsistencyAttribute : Attribute
{
    public string AgePropertyName { get; }
    public string ExperiencePropertyName { get; }
    public int MinimumWorkingAge { get; set; } = 18;
    public string? ErrorMessage { get; set; }

    public ExperienceAgeConsistencyAttribute(string agePropertyName, string experiencePropertyName)
    {
        AgePropertyName = agePropertyName;
        ExperiencePropertyName = experiencePropertyName;
    }
}

// =========================================================================
// ЧАСТИНА 2: АТРИБУТИ ВАЛІДАЦІЇ НА РІВНІ ВЛАСТИВОСТЕЙ (AttributeTargets.Property)
// Використовуються для перевірки кожного окремого поля моделі.
// =========================================================================

/// <summary>
/// Атрибут для обов'язкових рядкових властивостей (не можуть бути null або порожніми).
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RequiredFieldAttribute : Attribute
{
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Атрибут для обмеження допустимої довжини рядка.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class StringLengthRangeAttribute : Attribute
{
    public int MinLength { get; }
    public int MaxLength { get; }
    public string? ErrorMessage { get; set; }

    public StringLengthRangeAttribute(int minLength, int maxLength)
    {
        MinLength = minLength;
        MaxLength = maxLength;
    }
}

/// <summary>
/// Атрибут для перевірки діапазону цілочисельних значень.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NumberRangeAttribute : Attribute
{
    public int Min { get; }
    public int Max { get; }
    public string? ErrorMessage { get; set; }

    public NumberRangeAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}
