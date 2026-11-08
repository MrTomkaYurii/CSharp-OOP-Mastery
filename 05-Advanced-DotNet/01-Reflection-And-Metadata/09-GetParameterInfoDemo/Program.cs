using System.Reflection;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 09. АНАЛІЗ ПАРАМЕТРІВ МЕТОДІВ: System.Reflection.ParameterInfo");
Console.WriteLine(" Як дізнатися типи аргументів, модифікатори ref/out/in та значення за замовчуванням");
Console.WriteLine("==========================================================================\n");

Type sampleType = typeof(NetworkConfigService);
MethodInfo[] methods = sampleType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

foreach (MethodInfo method in methods)
{
    Console.WriteLine($"--- Інспекція методу: {method.ReturnType.Name} {method.Name}(...) ---");
    ParameterInfo[] parameters = method.GetParameters();

    foreach (ParameterInfo param in parameters)
    {
        // 1. Визначаємо, чи передається параметр за посиланням (ref, out або in)
        string refKind = param.ParameterType.IsByRef
            ? (param.IsOut ? "out " : (param.IsIn ? "in " : "ref "))
            : "";

        // 2. Перевіряємо, чи має параметр значення за замовчуванням (наприклад, int port = 8080)
        string optionalInfo = param.IsOptional
            ? $" [Опціональний, DefaultValue = {param.DefaultValue ?? "null"}]"
            : " [Обов'язковий]";

        Console.WriteLine($"  Позиція [{param.Position}]: {refKind}{param.ParameterType.Name,-10} Ім'я: {param.Name,-12}{optionalInfo}");
    }
    Console.WriteLine();
}

Console.WriteLine("Висновок: ParameterInfo використовується в ASP.NET Core для прив'язки параметрів запиту (Model Binding)");
Console.WriteLine("та в DI-контейнерах для визначення залежностей, які треба передати в конструктор.");

/// <summary>
/// Навчальний сервіс із різними видами параметрів для демонстрації ParameterInfo.
/// </summary>
public class NetworkConfigService
{
    // Метод із параметрами за замовчуванням (IsOptional = true)
    public void Connect(string host, int port = 8080, bool useTls = true)
    {
    }

    // Метод із модифікаторами передачі за посиланням (out, ref, in)
    public bool TryParsePacket(in ReadOnlySpan<byte> buffer, out int packetId, ref int retryCount)
    {
        packetId = 1;
        return true;
    }
}
