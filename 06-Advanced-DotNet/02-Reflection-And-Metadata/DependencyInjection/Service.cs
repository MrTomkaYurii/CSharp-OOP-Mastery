using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public class Service : IService
    {
        public void Process()
        {
            System.Console.WriteLine("Call Process() method");
        }
    }
}
