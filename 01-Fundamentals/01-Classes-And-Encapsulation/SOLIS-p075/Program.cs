// Показує, що кожен екземпляр класу має власний набір полів,
// а методи екземпляра працюють із полями саме "свого" об'єкта.

class DaysTemp // Оголошення класу
{
    public int High, Low;   // Оголошення полів екземпляра (без ініціалізатора => 0)
    public int Average()    // Метод екземпляра: неявно звертається до High/Low поточного об'єкта
    {
        return (High + Low) / 2;
    }
}

class Program
{
    static void Main()
    {
        // Створення двох екземплярів DaysTemp
        DaysTemp t1 = new DaysTemp();
        DaysTemp t2 = new DaysTemp();

        // Запис у поля кожного екземпляра
        t1.High = 76;
        t1.Low = 57;
        t2.High = 75;
        t2.Low = 53;

        // Читання полів кожного екземпляра та виклик його методу.
        // t1.Average() і t2.Average() повертають різні результати,
        // бо працюють із різними наборами полів.
        Console.WriteLine("t1: {0}, {1}, {2}", t1.High, t1.Low, t1.Average());
        Console.WriteLine("t2: {0}, {1}, {2}", t2.High, t2.Low, t2.Average());
    }
}
