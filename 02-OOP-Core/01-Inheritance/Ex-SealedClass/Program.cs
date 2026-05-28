using System;

namespace Ex_SealedClass;

/*
 * ── Запечатаний клас `sealed` ──────────────────────────────────────
 *
 * `sealed` перед словом `class` забороняє успадковувати цей клас.
 * Це роблять тоді, коли клас має працювати саме так, як написано,
 * і зміна його поведінки через спадкоємця небажана
 * (приклади зі стандартної бібліотеки: string, StringBuilder).
 *
 * Також `sealed` можна поставити перед `override`-методом: тоді метод
 * далі по ланцюжку успадкування вже НЕ можна перевизначати.
 *
 * Очікуваний вивід:
 *   Пароль '1234' підходить? False
 *   Пароль 'qwerty' підходить? True
 *   Автомобіль їде
 */

// ── Приклад 1: запечатаний клас ────────────────────────────────────

// Від класу Password не можна успадкуватися.
sealed class Password
{
    private string value;

    public Password(string value)
    {
        this.value = value;
    }

    public bool IsCorrect(string input)
    {
        return input == value;
    }
}

// Спроба успадкувати запечатаний клас — помилка компіляції:
// class FakePassword : Password        // CS0509: не можна успадкувати sealed-клас 'Password'
// {
// }

// ── Приклад 2: запечатаний метод ──────────────────────────────────

class Transport
{
    public virtual void Move()
    {
        Console.WriteLine("Транспорт рухається");
    }
}

class Car : Transport
{
    // Car перевизначає метод Move і одночасно "запечатує" його.
    public sealed override void Move()
    {
        Console.WriteLine("Автомобіль їде");
    }
}

class SportsCar : Car
{
    // Метод Move запечатано в класі Car, тому тут його перевизначити не можна:
    // public override void Move()       // CS0239: метод 'Car.Move()' запечатаний
    // {
    // }
}

class Program
{
    static void Main()
    {
        Password password = new Password("qwerty");
        Console.WriteLine($"Пароль '1234' підходить? {password.IsCorrect("1234")}");
        Console.WriteLine($"Пароль 'qwerty' підходить? {password.IsCorrect("qwerty")}");

        SportsCar car = new SportsCar();
        car.Move();   // успадкована від Car запечатана реалізація
    }
}
