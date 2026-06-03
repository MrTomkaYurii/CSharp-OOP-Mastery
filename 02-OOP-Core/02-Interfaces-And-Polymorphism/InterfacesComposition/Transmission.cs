// КОМПОЗИЦІЯ: так само, як і Engine, Transmission - невіддільна частина
// конкретного автомобіля, яку Car створює сам у своєму конструкторі.
namespace CompositionDemo;

public class Transmission : ITransmission
{
    public string Type { get; }
    public int CurrentGear { get; private set; } = 1;
    private readonly int maxGears;

    public Transmission(string type, int gears)
    {
        Type = type;
        maxGears = gears;
        Console.WriteLine($"    Зібрано трансмісію: {type} ({gears} передач)");
    }

    public void ShiftUp()
    {
        if (CurrentGear < maxGears)
        {
            CurrentGear++;
            Console.WriteLine($"    Переключено на {CurrentGear} передачу");
        }
    }

    public void ShiftDown()
    {
        if (CurrentGear > 1)
        {
            CurrentGear--;
            Console.WriteLine($"    Переключено на {CurrentGear} передачу");
        }
    }

    public string GetStatus() => $"{CurrentGear}/{maxGears} передача";
}
