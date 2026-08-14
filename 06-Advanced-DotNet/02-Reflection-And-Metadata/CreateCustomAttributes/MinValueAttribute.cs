using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCustomAttributes
{
    public class MinValueAttribute : Attribute
    {
        public int Value { get; }
        public MinValueAttribute(int value) => Value = value;
    }
}
