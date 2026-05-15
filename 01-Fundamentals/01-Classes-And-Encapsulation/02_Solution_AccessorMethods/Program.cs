using System;

// РІШЕННЯ 1: приховати поле (private) і дати доступ через методи.
// Тепер будь-який запис проходить лише через SetBalance, де є перевірка.
// Некоректне значення відхиляється, тож інваріант "баланс >= 0" зберігається.
// Це класичний перший крок (getter/setter), від якого потім переходять
// до властивостей (див. 03) і до поведінкового API (див. 04).

namespace EncapsulationDemo
{
    public class BankAccount
    {
        private decimal _balance;   // приховане поле: напряму ззовні недоступне

        // Getter: лише повертає значення.
        public decimal GetBalance() => _balance;

        // Setter: єдина "точка входу" для запису — тут і живе перевірка.
        public void SetBalance(decimal value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Баланс не може бути від'ємним.");

            _balance = value;
        }
    }

    internal class Program
    {
        private static void Main()
        {
            var account = new BankAccount();

            account.SetBalance(100m);
            Console.WriteLine($"Баланс: {account.GetBalance()}");

            // account._balance = -500m;   // <- помилка компіляції: поле private

            try
            {
                account.SetBalance(-500m);   // перевірка спрацює й відхилить значення
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Відхилено: {ex.Message}");
            }

            Console.WriteLine($"Баланс лишився коректним: {account.GetBalance()}");
            Console.ReadKey();
        }
    }
}
