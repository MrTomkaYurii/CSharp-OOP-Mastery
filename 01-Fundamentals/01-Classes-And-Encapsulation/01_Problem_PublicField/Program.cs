using System;

// ПРОБЛЕМА: дані класу відкриті (public-поле).
// Клас не контролює власний стан. Будь-який зовнішній код може:
//   * записати від'ємний баланс;
//   * "зняти" більше, ніж є на рахунку;
//   * змінити баланс, оминаючи будь-які правила.
// Через це об'єкт легко опиняється в некоректному (недопустимому) стані,
// і сам клас цьому ніяк не завадить.

namespace EncapsulationDemo
{
    public class BankAccount
    {
        // Відкрите поле — жодного захисту.
        // Інваріант "баланс >= 0" ніяк не гарантується.
        public decimal Balance;
    }

    internal class Program
    {
        private static void Main()
        {
            var account = new BankAccount();

            account.Balance = 100m;                       // поповнення
            Console.WriteLine($"Баланс: {account.Balance}");

            // Помилка 1: пряме присвоєння від'ємного балансу — клас не заперечує.
            account.Balance = -500m;
            Console.WriteLine($"Баланс після некоректного присвоєння: {account.Balance}");

            // Помилка 2: "зняти" більше, ніж є.
            // Немає ні перевірки, ні єдиного місця, де жили б правила.
            account.Balance = 100m;
            account.Balance -= 250m;
            Console.WriteLine($"Баланс після зняття 250 зі 100: {account.Balance}");

            // Об'єкт у недопустимому стані.
            Console.ReadKey();
        }
    }
}
