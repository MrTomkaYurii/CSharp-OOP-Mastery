using Testing.Core.Domain.Entities;
using Xunit;

namespace Testing.xUnit;

/// <summary>
/// УРОК 2: Тестування стану об'єкта (State-based Testing) та життєвий цикл xUnit.
/// 
/// Особливість xUnit:
/// Для КОЖНОГО методу [Fact] або рядка [InlineData] xUnit створює НОВИЙ екземпляр класу BankAccountXUnitTests:
///   1) Викликається Конструктор (замість [SetUp] в NUnit) -> готує початковий рахунок;
///   2) Виконується тестовий метод;
///   3) Викликається Dispose() (замість [TearDown] в NUnit) -> очищує ресурси.
/// Завдяки цьому тести ніколи не впливають на баланс один одного!
/// </summary>
public sealed class BankAccountXUnitTests : IDisposable
{
    private readonly BankAccount _account;

    public BankAccountXUnitTests()
    {
        // Ініціалізація перед кожним окремим тестом: стартовий баланс завжди 1000 грн
        _account = new BankAccount("UA-1000-01", "Тарас Шевченко", initialBalance: 1000m);
    }

    public void Dispose()
    {
        // Викликається автоматично після кожного тесту (тут можна закривати потоки/файли)
    }

    [Fact]
    public void Deposit_ValidAmount_IncreasesBalanceAndAddsTransactionRecord()
    {
        // Act
        _account.Deposit(500m);

        // Assert
        Assert.Equal(1500m, _account.Balance);
        Assert.Single(_account.Transactions);
        Assert.Equal("Deposit", _account.Transactions[0].Type);
        Assert.Equal(500m, _account.Transactions[0].Amount);
    }

    [Fact]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        // Навіть якщо попередній тест додав 500 грн, тут _account знову має рівно 1000 грн!
        // Act
        _account.Withdraw(300m);

        // Assert
        Assert.Equal(700m, _account.Balance);
        Assert.Single(_account.Transactions);
        Assert.Equal("Withdrawal", _account.Transactions[0].Type);
    }

    [Fact]
    public void Withdraw_MoreThanBalance_ThrowsInsufficientFundsException()
    {
        // Act & Assert
        Assert.Throws<InsufficientFundsException>(() => _account.Withdraw(5000m));

        // Переконуємося, що баланс не змінився після невдалої спроби
        Assert.Equal(1000m, _account.Balance);
        Assert.Empty(_account.Transactions);
    }

    [Fact]
    public void Freeze_WhenAccountIsFrozen_BlocksAllOperations()
    {
        // Arrange
        _account.Freeze();

        // Act & Assert
        Assert.True(_account.IsFrozen);
        Assert.Throws<AccountFrozenException>(() => _account.Deposit(100m));
        Assert.Throws<AccountFrozenException>(() => _account.Withdraw(100m));
    }

    [Fact]
    public void TransferTo_AnotherAccount_MovesMoneyBetweenAccounts()
    {
        // Arrange
        var receiver = new BankAccount("UA-2000-02", "Леся Українка", initialBalance: 200m);

        // Act
        _account.TransferTo(receiver, 400m);

        // Assert
        Assert.Equal(600m, _account.Balance);
        Assert.Equal(600m, receiver.Balance);
    }
}
