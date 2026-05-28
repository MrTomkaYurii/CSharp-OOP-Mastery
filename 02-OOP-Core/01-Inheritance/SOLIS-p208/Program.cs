using System;

/*
 * ── Абстрактний клас: суміш абстрактних і звичайних членів (Solis, с. 208) ──
 *
 * Абстрактний клас може вільно поєднувати:
 *   • поля та константи               (SideLength, TriangleSideCount);
 *   • абстрактні метод і властивість   (PrintStuff, MyInt) — реалізує похідний клас;
 *   • звичайний метод із логікою       (PerimeterLength) — використовує решту членів.
 *
 * Очікуваний вивід:
 *   This is a string.
 *   28
 *   Perimeter Length: 30
 */

// ── Абстрактний базовий клас ────────────────────────────────────────
abstract class MyBase
{
    public int SideLength = 10;            // звичайне поле
    const int TriangleSideCount = 3;       // константа

    public abstract void PrintStuff(string s);   // абстрактний метод
    public abstract int MyInt { get; set; }      // абстрактна властивість

    // Звичайний метод: спирається на константу й успадковане поле.
    public int PerimeterLength()
    {
        return TriangleSideCount * SideLength;
    }
}

// ── Похідний клас: реалізує абстрактні члени ─────────────────────────
class MyClass : MyBase
{
    public override void PrintStuff(string s)
    {
        Console.WriteLine(s);
    }

    private int _myInt;
    public override int MyInt
    {
        get { return _myInt; }
        set { _myInt = value; }
    }
}

class Program
{
    static void Main(string[] args)
    {
        MyClass mc = new MyClass();

        mc.PrintStuff("This is a string.");   // реалізований абстрактний метод

        mc.MyInt = 28;                        // реалізована абстрактна властивість
        Console.WriteLine(mc.MyInt);

        Console.WriteLine($"Perimeter Length: {mc.PerimeterLength()}");   // 3 * 10 = 30
    }
}
