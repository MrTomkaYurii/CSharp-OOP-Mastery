using System.Reflection;
using System.Text;
using Shared.Common;

namespace GetParameterInfoDemo
{
    class GetParameterInfoDemo
    {
        static void Main()
        {
            // Get name of type
            Type t = typeof(Car);
            GetParametersInfo(t);

            Console.ReadLine();
        }
        //Display Method return Type and paralmeters list
        public static void GetParametersInfo(Type t)
        {
            Console.WriteLine("***** GetParametersInfo *****");
            MethodInfo[] mi = t.GetMethods();
            foreach (MethodInfo m in mi)
            {
                // Get return value.
                string retVal = m.ReturnType.FullName;
                StringBuilder paramInfo = new StringBuilder();
                paramInfo.Append("(");

                // Get params.
                foreach (ParameterInfo pi in m.GetParameters())
                {
                    paramInfo.Append(string.Format("{0} {1} ", pi.ParameterType, pi.Name));
                }
                paramInfo.Append(")");

                // Now display the basic method sig.
                Console.WriteLine("->{0} {1} {2}", retVal, m.Name, paramInfo);
            }
            Console.WriteLine("");
        }
    }
}
