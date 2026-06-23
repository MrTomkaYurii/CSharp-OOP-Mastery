using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_02
{
    internal class MyClass : IEnumerable
    {
        public List<int> Mylist { get; set; } = new();

        //Here we define the Enumerator for our class. 
        //The class can now be used with something like foreach as it relies on the GetEnumerator.
        public IEnumerator GetEnumerator()
        {
            //When foreach gets used on an instance of this class we want to iterate through the MyList List<>.
            //Lists have their GetEnumerator already set. So we will just get GetEnumerator of our list and return that.
            return Mylist.GetEnumerator();
        }
    }
}
