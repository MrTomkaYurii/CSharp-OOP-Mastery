// КОМПОЗИЦІЯ: Car сам СТВОРЮЄ свій двигун і трансмісію прямо в
// конструкторі - їх не можна передати ззовні готовими (у Car немає
// конструктора, що приймає готовий IEngine). Тому вони не мають власного
// самостійного життя: з'являються разом з Car. Це головна відмінність від
// агрегації, де частини передаються вже готовими ззовні (див. Library.cs
// у прикладі InterfacesAggregation).
namespace CompositionDemo;

public class Car : ICar
{
    public string Model { get; }

    private readonly IEngine engine;
    private readonly ITransmission transmission;
    private bool isRunning;

    public Car(string model, string engineType, int horsePower, string transType, int gears)
    {
        Model = model;
        Console.WriteLine($"  Збірка автомобіля: {model}");

        // Двигун і трансмісія створюються ВСЕРЕДИНІ Car - це і є композиція.
        engine = new Engine(engineType, horsePower);
        transmission = new Transmission(transType, gears);

        Console.WriteLine($"  Автомобіль {model} зібрано");
    }

    public void StartCar()
    {
        Console.WriteLine($"\n  Запуск {Model}...");
        engine.Start();
        isRunning = true;
    }

    public void StopCar()
    {
        Console.WriteLine($"\n  Зупинка {Model}...");
        engine.Stop();
        isRunning = false;
    }

    public void Drive()
    {
        if (!isRunning)
        {
            Console.WriteLine("  Спочатку запустіть автомобіль!");
            return;
        }

        Console.WriteLine($"\n  {Model} їде...");
        transmission.ShiftUp();
        transmission.ShiftUp();
        Console.WriteLine("  Швидкість набрано!");
    }

    public void ShowSpecs()
    {
        Console.WriteLine($"\n  Характеристики {Model}:");
        Console.WriteLine($"  Двигун: {engine.Type} ({engine.HorsePower} к.с.) - {engine.GetStatus()}");
        Console.WriteLine($"  Трансмісія: {transmission.Type} - {transmission.GetStatus()}");
    }
}
