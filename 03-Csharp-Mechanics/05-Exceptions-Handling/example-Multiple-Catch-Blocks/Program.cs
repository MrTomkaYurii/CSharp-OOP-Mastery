namespace example_Multiple_Catch_Blocks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int Number1, Number2, Result;
            try
            {
                Console.WriteLine("Enter First Number");
                Number1 = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Second Number");
                Number2 = int.Parse(Console.ReadLine());
                Result = Number1 / Number2;
                Console.WriteLine($"Result: {Result}");
            }

            catch (DivideByZeroException DBZE)
            {
                Console.WriteLine("Second Number Should Not Be Zero");
            }
            catch (FormatException FE)
            {
                Console.WriteLine("Enter Only Integer Numbers");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Catch Block...");
            }
            finally
            {
                Console.WriteLine("Hello this is finally block...");
            }
            Console.ReadKey();
        }
    }
}
