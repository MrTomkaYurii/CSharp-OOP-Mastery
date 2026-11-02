using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 14. ПІЗНЄ ЗВ'ЯЗУВАННЯ ТА ДИНАМІЧНА АКТИВАЦІЯ: Activator.CreateInstance");
Console.WriteLine(" Як створювати об'єкти та викликати методи, коли тип невідомий під час компіляції");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Знаходження типу за рядковим ім'ям у завантаженій збірці
// -----------------------------------------------------------------------
// Раннє зв'язування (Early Binding): Car car = new Car(); — компілятор знає тип Car наперед.
// Пізнє зв'язування (Late Binding): ми маємо лише рядок "Shared.Common.Car" і створюємо об'єкт у runtime.
Console.WriteLine("--- Крок 1: Пошук типу за рядком 'Shared.Common.Car' ---");
Assembly sharedAssembly = typeof(Car).Assembly;
Type carType = sharedAssembly.GetType("Shared.Common.Car")!;
Console.WriteLine($"  Знайдено тип у runtime: {carType.FullName}\n");

// -----------------------------------------------------------------------
// Крок 2: Створення об'єкта через конструктор без параметрів
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 2: Виклик Activator.CreateInstance(carType) без параметрів ---");
object defaultCar = Activator.CreateInstance(carType)!;
Console.WriteLine($"  Створено екземпляр за замовчуванням: Колір = {carType.GetField("Color")?.GetValue(defaultCar)}\n");

// -----------------------------------------------------------------------
// Крок 3: Створення об'єкта з передачею аргументів у конструктор
// -----------------------------------------------------------------------
// CLR автоматично знайде конструктор Car(string color, int speed), який підходить під передані аргументи.
Console.WriteLine("--- Крок 3: Виклик Activator.CreateInstance із параметрами ('Metallic Red', 110) ---");
object customCar = Activator.CreateInstance(carType, "Metallic Red", 110)!;
Console.WriteLine($"  Колір авто:     {carType.GetField("Color")?.GetValue(customCar)}");
Console.WriteLine($"  Швидкість авто: {carType.GetProperty("Speed")?.GetValue(customCar)} км/год\n");

// -----------------------------------------------------------------------
// Крок 4: Виклик методів на динамічно створеному об'єкті
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 4: Динамічний виклик методів IsMoving() та CalculateMPG(...) ---");
MethodInfo isMovingMethod = carType.GetMethod("IsMoving")!;
bool isMoving = (bool)isMovingMethod.Invoke(customCar, null)!;
Console.WriteLine($"  Результат IsMoving(): {isMoving}");

MethodInfo calcMpgMethod = carType.GetMethod("CalculateMPG")!;
object[] mpgArgs = new object[] { 1000, 1450, 15.0 }; // startMiles, endMiles, gallons
double mpg = (double)calcMpgMethod.Invoke(customCar, mpgArgs)!;
Console.WriteLine($"  Результат CalculateMPG(1000, 1450, 15.0): {mpg:F2} миль на галон\n");

// -----------------------------------------------------------------------
// Крок 5: Обробка помилок при пізньому зв'язуванні
// -----------------------------------------------------------------------
// Оскільки компілятор не перевіряє типи аргументів у масиві object[], помилка виявиться під час виконання.
Console.WriteLine("--- Крок 5: Перехоплення помилки при передачі некоректного типу аргументу ---");
try
{
    calcMpgMethod.Invoke(customCar, new object[] { "не число", 200, 10 });
}
catch (ArgumentException ex)
{
    Console.WriteLine($"  [Перехоплено ArgumentException]: {ex.Message}");
}

Console.WriteLine("\nВисновок: Activator.CreateInstance — найпростіший спосіб створити об'єкт за його Type.");
