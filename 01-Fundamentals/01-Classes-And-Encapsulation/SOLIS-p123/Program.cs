// Поєднання необов'язкових та іменованих аргументів.
// Саме іменовані аргументи дозволяють задати лише height, пропустивши radius,
// чого не можна зробити позиційним викликом.
// Метод private, але Main у тому ж класі, тож доступ є.

class MyClass
{
    double GetCylinderVolume(double radius = 3.0, double height = 4.0)
    {
        return 3.1416 * radius * radius * height;   // V = π·r²·h
    }

    static void Main()
    {
        MyClass mc = new MyClass();
        double volume;

        volume = mc.GetCylinderVolume(3.0, 4.0);      // обидва аргументи позиційно
        Console.WriteLine("Volume = " + volume);

        volume = mc.GetCylinderVolume(radius: 2.0);   // radius=2.0, height=4.0 (за замовчуванням)
        Console.WriteLine("Volume = " + volume);

        volume = mc.GetCylinderVolume(height: 2.0);   // radius=3.0 (за замовчуванням), height=2.0
        Console.WriteLine("Volume = " + volume);

        volume = mc.GetCylinderVolume();                // radius=3.0, height=4.0
        Console.WriteLine("Volume = " + volume);
    }
}