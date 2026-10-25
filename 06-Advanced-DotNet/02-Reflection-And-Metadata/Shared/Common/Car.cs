namespace Shared.Common
{
    /// <summary>
    /// Базовий клас автомобіля з відкритими, закритими та статичними членами.
    /// Використовується для демонстрації рефлексії полів, властивостей, методів та конструкторів.
    /// </summary>
    public class Car : ICar
    {
        // Публічне поле (для демонстрації FieldInfo)
        public string Color;

        // Приватні поля (для демонстрації доступу через BindingFlags.NonPublic)
        private string _licensePlate = "AA 1234 BB";
        private double _maxSpeed = 220.0;
        private int _odometer = 15000;
        private int _speed;

        // Статичне поле
        private static int _totalCarsProduced = 42;

        // Публічні властивості
        public int Speed => _speed;
        public double MaxSpeed => _maxSpeed;
        public string LicensePlate => _licensePlate;

        // Конструктор за замовчуванням
        public Car()
        {
            Color = "White";
            _speed = 0;
        }

        // Параметризований конструктор
        public Car(string color, int speed)
        {
            Color = color;
            _speed = speed;
        }

        // Приватний конструктор (для демонстрації інстанціювання закритих конструкторів)
        private Car(string licensePlate, double maxSpeed, int odometer)
        {
            Color = "Black";
            _licensePlate = licensePlate;
            _maxSpeed = maxSpeed;
            _odometer = odometer;
        }

        // Публічний екземплярний метод
        public void Accelerate(int accelerateBy)
        {
            _speed += accelerateBy;
        }

        // Метод інтерфейсу ICar
        public bool IsMoving()
        {
            return _speed > 0;
        }

        // Метод з кількома параметрами
        public double CalculateMPG(int startMiles, int endMiles, double gallons)
        {
            return gallons > 0 ? (endMiles - startMiles) / gallons : 0;
        }

        // Приватний метод (для демонстрації виклику непублічних методів)
        private void ResetOdometer()
        {
            _odometer = 0;
        }

        // Статичний метод
        public static int GetTotalProduced() => _totalCarsProduced;
    }
}
