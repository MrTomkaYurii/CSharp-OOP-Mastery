using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_02
{
    internal class MySecondClass : IEnumerable
    {
        public int[] MyArray { get; set; }

        //Here we've implemented the enumerator so our class is enumerable and we can use foreach with it.
        //We could just call GetEnumerator() on the array to get its enumerator(as arrays implement IEnumerable already) like we did for the list in the example above.
        //But we'll write our own enumerator with the help of the yield keyword for demonstration purposes.
        public IEnumerator GetEnumerator()
        {
            //Let's iterate over the array and get its values.
            for (int i = 0; i < MyArray.Length; i++)
            {
                int value = MyArray[i];

                //... we could add some additional logic or calculations here if needed ...

                //Finally we yield return the value.
                yield return value; //See the code in the next section of this post to see how yield works.
            }
        }
    }
}
