// дослідження параметрів

using System.Reflection;

foreach (MethodInfo method in typeof(Printer).GetMethods())
{
    Console.Write($"{method.ReturnType.Name} {method.Name} (");
    //отримуємо параметри
    ParameterInfo[] parameters = method.GetParameters();
    for (int i = 0; i < parameters.Length; i++)
    {
        var param = parameters[i];
        // отримуємо індифікатори параметрів
        string modificator = "";
        if (param.IsIn) modificator = "in";
        else if (param.IsOut) modificator = "out";

        Console.Write($"{param.ParameterType.Name} {modificator} {param.Name}");
        // якщо параметр має значення по замовчуванню
        if (param.HasDefaultValue) Console.Write($"={param.DefaultValue}");
        // якщо не останній параметри то додаємо кому
        if (i < parameters.Length - 1) Console.Write(", ");
    }
    Console.WriteLine(")");
}

class Printer
{
    public void PrintMessage(string message, int times = 1)
    {
        while (times-- > 0) Console.WriteLine(message);
    }
    public void CreateMessage(out string message) => message = "Hello Guys!";
}