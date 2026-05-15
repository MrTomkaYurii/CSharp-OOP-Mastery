// Необов'язковий (optional) параметр: якщо в оголошенні задано значення за замовчуванням
// (b = 3), відповідний аргумент можна не передавати під час виклику.
// Необов'язкові параметри мають іти після всіх обов'язкових.

class MyClass
{
    public int Calc(int a, int b = 3)
    {
        return a + b;
    }

    static void Main()
    {
        MyClass mc = new MyClass();

        int r0 = mc.Calc(5, 6);  // b задано явно => 5 + 6 = 11
        int r1 = mc.Calc(5);     // b пропущено => береться 3 => 5 + 3 = 8

        Console.WriteLine($"{r0}, {r1}");
    }
}