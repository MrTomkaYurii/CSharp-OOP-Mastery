using System.Reflection;

namespace AttributesValidationDemo;

/// <summary>
/// Універсальний рефлексивний рушій валідації (аналог DataAnnotations Validator у .NET).
/// Послідовно перевіряє:
/// 1. Атрибути на рівні окремих властивостей (PropertyInfo).
/// 2. Атрибути на рівні самого класу (Type) для міжпольової узгодженості.
/// </summary>
public static class ClassAndPropertyValidator
{
    public static bool Validate(object entity, out List<string> errors)
    {
        ArgumentNullException.ThrowIfNull(entity);
        errors = new List<string>();

        Type entityType = entity.GetType();

        // -----------------------------------------------------------------
        // ЕТАП 1: Валідація на рівні окремих властивостей (Property-Level)
        // -----------------------------------------------------------------
        PropertyInfo[] properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(entity);

            // 1.1. Перевірка [RequiredField]
            var requiredAttr = property.GetCustomAttribute<RequiredFieldAttribute>();
            if (requiredAttr != null)
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    errors.Add(requiredAttr.ErrorMessage ?? $"[Властивість {property.Name}]: Поле не може бути порожнім.");
                    continue; // Якщо поле порожнє, подальша перевірка довжини не потрібна
                }
            }

            // 1.2. Перевірка [StringLengthRange]
            var lengthAttr = property.GetCustomAttribute<StringLengthRangeAttribute>();
            if (lengthAttr != null && value is string strVal)
            {
                if (strVal.Length < lengthAttr.MinLength || strVal.Length > lengthAttr.MaxLength)
                {
                    errors.Add(lengthAttr.ErrorMessage
                        ?? $"[Властивість {property.Name}]: Довжина рядка ({strVal.Length}) має бути від {lengthAttr.MinLength} до {lengthAttr.MaxLength} символів.");
                }
            }

            // 1.3. Перевірка [NumberRange]
            var rangeAttr = property.GetCustomAttribute<NumberRangeAttribute>();
            if (rangeAttr != null && value is int intVal)
            {
                if (intVal < rangeAttr.Min || intVal > rangeAttr.Max)
                {
                    errors.Add(rangeAttr.ErrorMessage
                        ?? $"[Властивість {property.Name}]: Значення ({intVal}) виходить за межі діапазону [{rangeAttr.Min}..{rangeAttr.Max}].");
                }
            }
        }

        // -----------------------------------------------------------------
        // ЕТАП 2: Валідація на рівні всього класу (Class-Level Validation)
        // -----------------------------------------------------------------
        // 2.1. Перевірка [RangeConsistency] (узгодженість Min <= Max)
        IEnumerable<RangeConsistencyAttribute> rangeConsistencies = entityType.GetCustomAttributes<RangeConsistencyAttribute>(inherit: true);
        foreach (RangeConsistencyAttribute attr in rangeConsistencies)
        {
            PropertyInfo? minProp = entityType.GetProperty(attr.MinPropertyName);
            PropertyInfo? maxProp = entityType.GetProperty(attr.MaxPropertyName);

            if (minProp != null && maxProp != null)
            {
                int minVal = Convert.ToInt32(minProp.GetValue(entity));
                int maxVal = Convert.ToInt32(maxProp.GetValue(entity));

                if (minVal > maxVal)
                {
                    errors.Add(attr.ErrorMessage
                        ?? $"[Клас {entityType.Name}]: Значення '{attr.MinPropertyName}' ({minVal}) більше за '{attr.MaxPropertyName}' ({maxVal}).");
                }
            }
        }

        // 2.2. Перевірка [ExperienceAgeConsistency] (узгодженість віку та стажу)
        var expAttr = entityType.GetCustomAttribute<ExperienceAgeConsistencyAttribute>(inherit: true);
        if (expAttr != null)
        {
            PropertyInfo? ageProp = entityType.GetProperty(expAttr.AgePropertyName);
            PropertyInfo? expProp = entityType.GetProperty(expAttr.ExperiencePropertyName);

            if (ageProp != null && expProp != null)
            {
                int age = Convert.ToInt32(ageProp.GetValue(entity));
                int experience = Convert.ToInt32(expProp.GetValue(entity));
                int maxPossibleExperience = Math.Max(0, age - expAttr.MinimumWorkingAge);

                if (experience > maxPossibleExperience)
                {
                    errors.Add(expAttr.ErrorMessage
                        ?? $"[Клас {entityType.Name}]: Стаж ({experience} р.) перевищує максимально можливий ({maxPossibleExperience} р.) для віку {age} р.");
                }
            }
        }

        return errors.Count == 0;
    }
}
