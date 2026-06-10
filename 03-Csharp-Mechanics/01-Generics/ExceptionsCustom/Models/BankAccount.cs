using ExceptionsCustom.Exceptions;

namespace ExceptionsCustom.Models;

// Банківський рахунок. Тут поєднано обидва види винятків:
//   - ВБУДОВАНІ (ArgumentNullException, ArgumentOutOfRangeException, InvalidOperationException...)
//     — коли викликач ПОРУШИВ правила використання методу (помилка програміста);
//   - ВЛАСНІ (InsufficientFundsException, AccountFrozenException)
//     — коли порушено правило ПРЕДМЕТНОЇ ОБЛАСТІ (це нормальна бізнес-ситуація).
public class BankAccount
{
    public string Number { get; }
    public decimal Balance { get; private set; }
    public bool IsFrozen { get; set; }
    public bool IsClosed { get; private set; }

    public BankAccount(string number, decimal initialBalance)
    {
        if (number is null)
            throw new ArgumentNullException(nameof(number));                 // null там, де його не можна

        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Номер рахунку не може бути порожнім.", nameof(number));

        if (initialBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(initialBalance), initialBalance,
                "Початковий баланс не може бути від'ємним.");

        Number = number;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        CheckCanOperate();

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Сума має бути більшою за нуль.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        CheckCanOperate();

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Сума має бути більшою за нуль.");

        if (amount > Balance)
            throw new InsufficientFundsException(Balance, amount);           // ВЛАСНИЙ виняток з даними

        Balance -= amount;
    }

    public void Close()
    {
        CheckCanOperate();
        IsClosed = true;
    }

    // Спільні перевірки стану для Deposit / Withdraw / Close
    private void CheckCanOperate()
    {
        if (IsClosed)
            throw new InvalidOperationException($"Рахунок {Number} закрито — операції неможливі.");   // стан об'єкта не дозволяє

        if (IsFrozen)
            throw new AccountFrozenException(Number);
    }
}
