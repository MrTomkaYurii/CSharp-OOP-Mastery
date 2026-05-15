// Демонструє порядок виконання при вкладених викликах (стек викликів):
// Main -> MethodA -> MethodB, а повернення відбувається у зворотному порядку.
// Очікуваний вивід:
//   Enter Main / Enter MethodA / Enter MethodB / Exit MethodB / Exit MethodA / Exit Main

class Program
{
    static void MethodA(int par1, int par2)
    {
        Console.WriteLine($"Enter MethodA: {par1}, {par2}");
        MethodB(11, 18);                        // Керування переходить у MethodB і повертається сюди після нього.
        Console.WriteLine("Exit MethodA");
    }

    static void MethodB(int par1, int par2)
    {
        Console.WriteLine($"Enter MethodB: {par1}, {par2}");
        Console.WriteLine("Exit MethodB");
    }

    static void Main()
    {
        Console.WriteLine("Enter Main");
        MethodA(15, 30);                        // Виклик MethodA.
        Console.WriteLine("Exit Main");
    }
}