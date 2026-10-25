using System.Reflection;

namespace CreateCustomAttributes
{
    /// <summary>
    /// Універсальний валідатор об'єктів на базі рефлексії (аналог рушія DataAnnotations у ASP.NET Core).
    /// </summary>
    public static class MinValueValidator
    {
        /// <summary>
        /// Перевіряє всі властивості довільного об'єкта на відповідність валідаційним атрибутам.
        /// </summary>
        public static bool Validate(object entity, out List<string> errors)
        {
            errors = new List<string>();
            ArgumentNullException.ThrowIfNull(entity);

            Type entityType = entity.GetType();
            PropertyInfo[] properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(entity);

                // 1. Перевірка [Required]
                var requiredAttr = property.GetCustomAttribute<RequiredAttribute>();
                if (requiredAttr != null)
                {
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        errors.Add(requiredAttr.ErrorMessage ?? $"Властивість '{property.Name}' не може бути порожньою.");
                    }
                }

                // 2. Перевірка [MinValue]
                var minValAttr = property.GetCustomAttribute<MinValueAttribute>();
                if (minValAttr != null && value is int intValue)
                {
                    if (intValue < minValAttr.Value)
                    {
                        errors.Add(minValAttr.ErrorMessage ?? $"Значення '{property.Name}' ({intValue}) менше за мінімально дозволене ({minValAttr.Value}).");
                    }
                }

                // 3. Перевірка [StringLength]
                var lengthAttr = property.GetCustomAttribute<StringLengthAttribute>();
                if (lengthAttr != null && value is string strVal)
                {
                    if (strVal.Length < lengthAttr.MinLength || strVal.Length > lengthAttr.MaxLength)
                    {
                        errors.Add($"Довжина поля '{property.Name}' повинна бути від {lengthAttr.MinLength} до {lengthAttr.MaxLength} символів.");
                    }
                }
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Демонстрація читання метаданих атрибутів без їх інстанціювання (через CustomAttributeData).
        /// Корисно для інспекції без виклику конструктора атрибута.
        /// </summary>
        public static void InspectRawAttributeData(Type type)
        {
            Console.WriteLine($"\n--- Дослідження сирих метаданих (CustomAttributeData) для {type.Name} ---");
            foreach (PropertyInfo prop in type.GetProperties())
            {
                IList<CustomAttributeData> customAttributes = prop.GetCustomAttributesData();
                foreach (CustomAttributeData attrData in customAttributes)
                {
                    Console.WriteLine($"  Властивість [{prop.Name}] -> Атрибут: {attrData.AttributeType.Name}");
                    foreach (var arg in attrData.ConstructorArguments)
                    {
                        Console.WriteLine($"    Конструкторний аргумент ({arg.ArgumentType.Name}): {arg.Value}");
                    }
                }
            }
        }
    }
}
