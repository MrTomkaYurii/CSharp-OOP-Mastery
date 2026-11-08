namespace Shared.Common
{
    /// <summary>
    /// Похідний клас спортивного автомобіля для демонстрації ієрархії наслідування та поліморфізму в рефлексії.
    /// </summary>
    public sealed class SportsCar : Car
    {
        public bool HasTurbo { get; set; } = true;

        public SportsCar()
        {
            Color = "Red";
        }

        public SportsCar(string color, int speed, bool hasTurbo) : base(color, speed)
        {
            HasTurbo = hasTurbo;
        }

        public void ActivateTurbo()
        {
            if (HasTurbo)
            {
                Accelerate(50);
            }
        }
    }
}
