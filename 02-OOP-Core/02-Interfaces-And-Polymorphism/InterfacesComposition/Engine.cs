// КОМПОЗИЦІЯ: об'єкт Engine "народжується" разом з конкретним Car - його
// створює сам Car у своєму конструкторі (див. Car.cs), і ззовні такий
// двигун ніколи не існує сам по собі та не передається в готовому вигляді.
namespace CompositionDemo;

public class Engine : IEngine
{
    public string Type { get; }
    public int HorsePower { get; }
    private bool isRunning;

    public Engine(string type, int horsePower)
    {
        Type = type;
        HorsePower = horsePower;
        Console.WriteLine($"    Зібрано двигун: {type} ({horsePower} к.с.)");
    }

    public void Start()
    {
        isRunning = true;
        Console.WriteLine($"    Двигун запущено: {Type}");
    }

    public void Stop()
    {
        isRunning = false;
        Console.WriteLine("    Двигун зупинено");
    }

    public string GetStatus() => isRunning ? "працює" : "вимкнено";
}
