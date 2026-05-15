using System;

// РІШЕННЯ 2: те саме керування доступом, але ідіоматично для C# — через властивість.
// Ззовні виглядає як звичайне поле (account.Balance = 100), але насправді
// get і set — це методи. Перевірка ховається в set.
//   * value       — неявний параметр set (присвоюване значення);
//   * _balance    — приховане поле-сховище (backing field).

namespace EncapsulationDemo
{
    public class BankAccount
    {
        private decimal _balance;

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Баланс не може бути від'ємним.");

                _balance = value;
            }
        }
    }

    internal class Program
    {
        private static void Main()
        {
            var account = new BankAccount();

            account.Balance = 100m;                            // виклик set (з перевіркою)
            Console.WriteLine($"Баланс: {account.Balance}");   // виклик get

            try
            {
                account.Balance = -500m;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Відхилено: {ex.Message}");
            }

            Console.WriteLine($"Баланс лишився коректним: {account.Balance}");
            Console.ReadKey();
        }
    }
}
