namespace Exception_Handling_Abuse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please Enter First Number");
                int FirstNumber;
                //int.TryParse() will not throw an exception, instead returns false
                //if the entered value cannot be converted to integer
                bool IsValidFirstNumber = int.TryParse(Console.ReadLine(), out FirstNumber);
                if (IsValidFirstNumber)
                {
                    Console.WriteLine("Please Enter Second Number");
                    int SecondNumber;
                    bool isValidSNO = int.TryParse(Console.ReadLine(), out SecondNumber);

                    if (isValidSNO && SecondNumber != 0)
                    {
                        int Result = FirstNumber / SecondNumber;
                        Console.WriteLine($"Result = {Result}");
                    }
                    else
                    {
                        //Check if the second number is zero, print a friendly error
                        //message instead of allowing DivideByZeroException exception 
                        //to be thrown.
                        if (isValidSNO && SecondNumber == 0)
                        {
                            Console.WriteLine("Second Number cannot be zero");
                        }
                        else
                        {
                            Console.WriteLine($"Only numbers between {Int32.MinValue} && {Int32.MaxValue} are allowed");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Only numbers between {Int32.MinValue} && {Int32.MaxValue} are allowed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
