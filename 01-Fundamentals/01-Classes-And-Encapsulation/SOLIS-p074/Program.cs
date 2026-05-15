using SOLIS_p074;

// Найпростіший приклад: клас із public-полями, створення екземпляра
// та прямий доступ до його полів ззовні.

class Program // Оголошення класу Program
{
    static void Main()
    {
        // new викликає конструктор і повертає посилання на новий об'єкт у купі.
        DaysTemp temp = new DaysTemp();              // Створення об'єкта

        // Оскільки поля public, до них можна звертатися напряму через ім'я об'єкта.
        temp.High = 85;                              // Присвоєння значень полям
        temp.Low = 60;

        Console.WriteLine("High: {0}", temp.High); // Читання значень полів
        Console.WriteLine($"Low:  {temp.Low}");
        Console.ReadKey();
    }
}
