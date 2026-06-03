namespace CompositionDemo;

public interface ICar
{
    string Model { get; }
    void StartCar();
    void StopCar();
    void Drive();
    void ShowSpecs();
}
