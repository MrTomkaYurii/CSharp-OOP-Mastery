using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCustomAttributes
{
    public class User
    {
        public string Name { get; set; }

        [MinValue(18)]
        public int Age { get; set; }
    }
}
