using System;

/*
 * ── Розширення класу статичним методом (Solis, с. 212) ──────────────
 *
 * Клас MyData "закритий" — додати до нього метод Average безпосередньо
 * не можна (наприклад, це чужий код). Тому окремий статичний клас
 * ExtendMyData надає метод, який приймає MyData параметром і працює
 * лише з його публічним API (md.Sum()).
 *
 * Виклик поки що звичайний: ExtendMyData.Average(md).
 * На с. 214 цей самий підхід перетвориться на метод-розширення.
 *
 * Очікуваний вивід:
 *   Average: 4
 */

// ── Клас, який ми хочемо "доповнити" ────────────────────────────────
class MyData
{
    private double D1;   // приватні поля
    private double D2;
    private double D3;

    public MyData(double d1, double d2, double d3)
    {
        D1 = d1;
        D2 = d2;
        D3 = d3;
    }

    public double Sum()
    {
        return D1 + D2 + D3;
    }
}

// ── Окремий статичний клас із додатковою логікою ────────────────────
static class ExtendMyData
{
    // Обчислює середнє, спираючись лише на публічний метод Sum().
    public static double Average(MyData md)
    {
        return md.Sum() / 3;
    }
}

class Program
{
    static void Main()
    {
        MyData md = new MyData(3, 4, 5);

        Console.WriteLine("Average: {0}", ExtendMyData.Average(md));   // 12 / 3 = 4
    }
}
