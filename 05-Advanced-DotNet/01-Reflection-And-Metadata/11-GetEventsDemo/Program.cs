using System.Reflection;
using Shared.Common;

namespace GetEventsDemo;

public delegate void SpeedLimitExceededHandler(string carModel, int currentSpeed, int limit);

/// <summary>
/// Автомобіль із підтримкою подій для демонстрації EventInfo.
/// </summary>
public class ObservableCar : Car
{
    public event SpeedLimitExceededHandler? SpeedLimitExceeded;
    public event EventHandler? EngineStarted;

    public void TriggerAcceleration(int newSpeed)
    {
        if (newSpeed > 130)
        {
            SpeedLimitExceeded?.Invoke("Tesla Model S", newSpeed, 130);
        }
    }

    public void StartEngine()
    {
        EngineStarted?.Invoke(this, EventArgs.Empty);
    }
}

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 11. Дослідження подій типу: System.Reflection.EventInfo ===\n");

        Type carType = typeof(ObservableCar);

        // 1. Отримання списку всіх подій типу
        Console.WriteLine("--- 1. Перелік подій типу ObservableCar ---");
        EventInfo[] events = carType.GetEvents(BindingFlags.Public | BindingFlags.Instance);

        foreach (EventInfo ev in events)
        {
            Console.WriteLine($"Подія: {ev.Name}");
            Console.WriteLine($"  Тип делегата події: {ev.EventHandlerType?.Name}");
            Console.WriteLine($"  Метод підписки (AddMethod):    {ev.AddMethod?.Name}");
            Console.WriteLine($"  Метод відписки (RemoveMethod): {ev.RemoveMethod?.Name}");
            Console.WriteLine($"  IsMulticast: {ev.IsMulticast}");
            Console.WriteLine();
        }

        // 2. Динамічна підписка на подію (Dynamic Event Subscription)
        Console.WriteLine("--- 2. Динамічна підписка на подію через AddEventHandler ---");
        var carInstance = new ObservableCar();

        EventInfo? speedEvent = carType.GetEvent(nameof(ObservableCar.SpeedLimitExceeded));
        if (speedEvent != null)
        {
            // Створюємо екземпляр обробника через Delegate.CreateDelegate або лямбду
            SpeedLimitExceededHandler handler = OnSpeedExceededNotification;

            // Динамічно реєструємо підписника через рефлексію:
            speedEvent.AddEventHandler(carInstance, handler);
            Console.WriteLine("Обробник OnSpeedExceededNotification успішно динамічно підписано на подію!\n");

            // Викликаємо бізнес-метод, що генерує подію
            Console.WriteLine("Імітація перевищення швидкості до 150 км/год:");
            carInstance.TriggerAcceleration(150);

            // 3. Динамічна відписка від події
            Console.WriteLine("\n--- 3. Динамічна відписка через RemoveEventHandler ---");
            speedEvent.RemoveEventHandler(carInstance, handler);
            Console.WriteLine("Обробник успішно відписано. Повторне прискорення:");
            carInstance.TriggerAcceleration(160);
            Console.WriteLine("Подія спрацювала без сповіщення (підписник відсутній).");
        }
    }

    private static void OnSpeedExceededNotification(string carModel, int currentSpeed, int limit)
    {
        Console.WriteLine($"  [УВАГА СПОСТЕРІГАЧА]: Автомобіль {carModel} розігнався до {currentSpeed} км/год при ліміті {limit} км/год!");
    }
}
