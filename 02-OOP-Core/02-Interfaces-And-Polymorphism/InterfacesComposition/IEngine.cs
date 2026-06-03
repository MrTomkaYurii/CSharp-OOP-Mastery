// Контракт двигуна. Car працює через IEngine, а не через конкретний клас
// Engine - завдяки цьому можна зібрати авто з будь-яким сумісним двигуном
// (наприклад, електричним), не змінюючи код класу Car.
namespace CompositionDemo;

public interface IEngine
{
    string Type { get; }
    int HorsePower { get; }
    void Start();
    void Stop();
    string GetStatus();
}
