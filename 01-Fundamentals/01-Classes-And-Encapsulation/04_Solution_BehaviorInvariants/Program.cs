using System;

// РІШЕННЯ 3 (повна інкапсуляція): клас надає ПОВЕДІНКУ, а не дані.
// Баланс можна лише читати (публічного set немає взагалі).
// Змінюється він тільки через Deposit/Withdraw, які тримають усі
// бізнес-правила в одному місці:
//   * поповнювати й знімати можна лише додатною сумою;
//   * зняти більше за наявний баланс не можна.
// Отже, некоректний стан рахунку неможливий "за побудовою".

namespace EncapsulationDemo
{
    public class BankAccount
    {
        private decimal _balance;

        public BankAccount(decimal openingBalance = 0m)
        {
            if (openingBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(openingBalance),
                    "Початковий баланс не може бути від'ємним.");

            _balance = openingBalance;
        }

        // Лише для читання — ззовні змінити баланс напряму не можна.
        public decimal Balance => _balance;

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Сума поповнення має бути додатною.");

            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Сума зняття має бути додатною.");

            if (amount > _balance)
                throw new InvalidOperationException("Недостатньо коштів на рахунку.");

            _balance -= amount;
        }
    }

    internal class Program
    {
        private static void Main()
        {
            var account = new BankAccount(100m);
            Console.WriteLine($"Відкрито рахунок. Баланс: {account.Balance}");

            account.Deposit(50m);
            Console.WriteLine($"Після поповнення на 50: {account.Balance}");

            account.Withdraw(30m);
            Console.WriteLine($"Після зняття 30: {account.Balance}");

            // account.Balance = -500m;   // <- помилка компіляції: властивість лише для читання

            try
            {
                account.Withdraw(1000m);   // правило "не більше за баланс" — в одному місці
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Відхилено: {ex.Message}");
            }

            Console.WriteLine($"Баланс лишився коректним: {account.Balance}");
            Console.ReadKey();
        }
    }
}
