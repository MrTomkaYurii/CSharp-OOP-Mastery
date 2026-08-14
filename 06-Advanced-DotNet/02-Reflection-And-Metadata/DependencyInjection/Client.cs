using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public class Client
    {
        private IService _service;

        public Client(IService service)
        {
            _service = service;
        }

        public void RunProcess()
        {
            _service.Process();
        }
    }
}
