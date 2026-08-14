using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCustomAttributes
{
    //валідатор, який перевірятиме такі атрибути через рефлексію
    //(аналогічно тому, як це робить DataAnnotations у .NET).
    public static class MinValueValidator
    {
        public static bool Validate(User person)
        {
            var type = typeof(User);
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);

                foreach (var attr in attributes)
                {
                    if (attr is MinValueAttribute minValueAttribute)
                    {
                        var propValue = property.GetValue(person, null);
                        if (propValue == null ||
                            int.Parse(propValue.ToString()) < minValueAttribute.Value)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
