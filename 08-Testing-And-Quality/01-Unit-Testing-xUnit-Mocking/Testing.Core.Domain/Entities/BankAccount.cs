namespace Testing.Core.Domain.Entities;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message) : base(message) { }
}

public class AccountFrozenException : Exception
{
    public AccountFrozenException(string message) : base(message) { }
}

public record BankTransaction(Guid Id, string Type, decimal Amount, DateTime Timestamp);

public class BankAccount
{
    private readonly List<BankTransaction> _transactions = [];

    public string AccountNumber { get; }
    public string Owner { get; private set; }
    public decimal Balance { get; private set; }
    public bool IsFrozen { get; private set; }

    public IReadOnlyList<BankTransaction> Transactions => _transactions.AsReadOnly();

    public BankAccount(string accountNumber, string owner, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Номер рахунку не може бути порожнім.", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Власник рахунку не може бути порожнім.", nameof(owner));

        if (initialBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(initialBalance), "Початковий баланс не може бути від'ємним.");

        AccountNumber = accountNumber;
        Owner = owner;
        Balance = initialBalance;
    }

    public void Freeze() => IsFrozen = true;
    public void Unfreeze() => IsFrozen = false;

    public void Deposit(decimal amount)
    {
        if (IsFrozen)
            throw new AccountFrozenException($"Рахунок {AccountNumber} заморожено. Операції поповнення заборонені.");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сума поповнення повинна бути строго більшою за нуль.");

        Balance += amount;
        _transactions.Add(new BankTransaction(Guid.NewGuid(), "Deposit", amount, DateTime.UtcNow));
    }

    public void Withdraw(decimal amount)
    {
        if (IsFrozen)
            throw new AccountFrozenException($"Рахунок {AccountNumber} заморожено. Списання коштів заблоковано.");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сума зняття повинна бути строго більшою за нуль.");

        if (amount > Balance)
            throw new InsufficientFundsException($"Недостатньо коштів на рахунку {AccountNumber}. Поточний баланс: {Balance}, запитано: {amount}.");

        Balance -= amount;
        _transactions.Add(new BankTransaction(Guid.NewGuid(), "Withdrawal", amount, DateTime.UtcNow));
    }

    public void TransferTo(BankAccount targetAccount, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(targetAccount);

        if (targetAccount.AccountNumber == AccountNumber)
            throw new InvalidOperationException("Неможливо виконати переказ на той самий банківський рахунок.");

        Withdraw(amount);
        targetAccount.Deposit(amount);
    }
}
