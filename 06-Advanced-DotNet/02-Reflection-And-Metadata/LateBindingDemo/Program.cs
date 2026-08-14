using System.Reflection;

namespace LateBindingDemo
{
    class LateBindingDemo
    {
        static void Main()
        {
            Assembly objAssembly;

            // Loads an assembly  
            objAssembly = Assembly.LoadFrom("C:\\Users\\Yurii\\source\\repos\\OOP - Reflection\\LateBindingDemo\\bin\\Debug\\net8.0\\Shared.dll");

            //get the class type information in which late bindig applied
            Type classType = objAssembly.GetType("Shared.Common.Car");

            // альтернатива
            //Type classType = typeof(Shared.Common.Car)

            //create the instance of class using System.Activator class
            object obj = Activator.CreateInstance(classType);

            //get the method information
            MethodInfo mi = classType.GetMethod("IsMoving");

            //Late Binding using Invoke method without parameters
            bool isCarMoving;
            isCarMoving = (bool)mi.Invoke(obj, null);
            if (isCarMoving)
            {
                Console.WriteLine("Car Moving Status is : Moving");
            }
            else
            {
                Console.WriteLine("Car Moving Status is : Not Moving");
            }

            //Late Binding with parameters
            object[] parameters = new object[3];
            parameters[0] = 32456;//parameter 1 startMiles
            parameters[1] = 32810;//parameter 2 end Miles
            parameters[2] = 10.6;//parameter 3 gallons
            mi = classType.GetMethod("calculateMPG");
            double MilesPerGallon;
            MilesPerGallon = (double)mi.Invoke(obj, parameters);
            Console.WriteLine("Miles per gallon is : " + MilesPerGallon);

            Console.ReadLine();
        }

    }

}
