// отримання інформації про методи

using System.Reflection;

Type myType = typeof(Printer);

Console.WriteLine("Методи:");
foreach (MethodInfo method in myType.GetMethods())
{
    string modificator = "";

    // якщо метод статичний
    if (method.IsStatic) modificator += "static ";
    // кщо метод віртуальний
    if (method.IsVirtual) modificator += "virtual ";

    Console.WriteLine($"{modificator}{method.ReturnType.Name} {method.Name} ()");
}
class Printer
{
    public string DefaultMessage { get; set; } = "Hello";
    public void PrintMessage(string message, int times = 1)
    {
        while (times-- > 0) Console.WriteLine(message);
    }
    public string CreateMessage() => DefaultMessage;
}