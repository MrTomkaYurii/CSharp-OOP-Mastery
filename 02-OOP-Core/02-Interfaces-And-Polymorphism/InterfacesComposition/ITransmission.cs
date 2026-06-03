// Контракт трансмісії - так само, як і з двигуном, Car залежить від
// абстракції ITransmission, а не від конкретної реалізації.
namespace CompositionDemo;

public interface ITransmission
{
    string Type { get; }
    int CurrentGear { get; }
    void ShiftUp();
    void ShiftDown();
    string GetStatus();
}
