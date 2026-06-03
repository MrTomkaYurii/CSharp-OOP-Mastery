// КОМПОЗИЦІЯ (Composition) - найсильніший тип зв'язку "складається з":
// частини не можуть існувати без цілого. Ключова ознака в коді - власник
// (Car) сам створює свої частини (Engine, Transmission) у власному
// конструкторі; ззовні готовий двигун передати неможливо.
//
// НАВІЩО ТУТ ІНТЕРФЕЙСИ: усередині Car зберігаються поля типу IEngine та
// ITransmission, а не конкретні класи Engine/Transmission. Тому можна
// зібрати автомобіль з іншим типом двигуна (наприклад, електричним),
// створивши новий клас, що реалізує IEngine, - і жодного рядка коду в
// Car міняти не доведеться.
namespace CompositionDemo;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== КОМПОЗИЦІЯ: Автомобіль, двигун і трансмісія ===\n");

        // Car сам створює двигун і трансмісію - передати їх готовими
        // ззовні неможливо, бо в Car немає такого конструктора.
        var mercedes = new Car(
            model: "Mercedes S-Class",
            engineType: "V8 Biturbo",
            horsePower: 450,
            transType: "9G-TRONIC",
            gears: 9);

        mercedes.ShowSpecs();
        mercedes.StartCar();
        mercedes.Drive();
        mercedes.StopCar();

        Console.WriteLine("\nЗбірка іншого автомобіля з іншим двигуном:");
        var bmw = new Car("BMW M5", "V8 TwinTurbo", 625, "M Steptronic", 8);
        bmw.ShowSpecs();
        bmw.StartCar();
        bmw.Drive();
        bmw.StopCar();

        Console.WriteLine("\nКлючові моменти композиції:");
        Console.WriteLine("  - Car створює Engine і Transmission у своєму конструкторі.");
        Console.WriteLine("  - Їх не можна створити окремо і передати в готовий Car.");
        Console.WriteLine("  - Кожен Car має власний, унікальний двигун і трансмісію.");
    }
}
