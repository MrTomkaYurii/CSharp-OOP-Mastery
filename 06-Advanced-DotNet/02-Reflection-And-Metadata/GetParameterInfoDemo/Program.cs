using System.Reflection;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація ParameterInfo (Інспекція параметрів методів) ===\n");

Type sampleType = typeof(SampleService);
MethodInfo[] methods = sampleType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

foreach (MethodInfo method in methods)
{
    Console.WriteLine($"Метод: {method.ReturnType.Name} {method.Name}");
    ParameterInfo[] parameters = method.GetParameters();

    if (parameters.Length == 0)
    {
        Console.WriteLine("  (параметри відсутні)");
        continue;
    }

    foreach (ParameterInfo param in parameters)
    {
        string refKind = param.ParameterType.IsByRef
            ? (param.IsOut ? "out " : (param.IsIn ? "in " : "ref "))
            : "";

        string optionalInfo = param.IsOptional
            ? $" [За замовчуванням: {param.DefaultValue ?? "null"}]"
            : "";

        Console.WriteLine($"  [{param.Position}] {refKind}{param.ParameterType.Name} {param.Name}{optionalInfo}");
    }
    Console.WriteLine();
}

public class SampleService
{
    public void Configure(string host, int port = 8080, bool useSsl = true) { }

    public bool TryParseData(string input, out int parsedValue, ref double multiplier)
    {
        parsedValue = 0;
        return true;
    }
}
