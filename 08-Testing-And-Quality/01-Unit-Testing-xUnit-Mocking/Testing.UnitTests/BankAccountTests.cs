using FluentAssertions;
using Testing.Core.Domain.Entities;
using Xunit;

namespace Testing.UnitTests;

public class BankAccountTests
{
    [Fact]
    public void Constructor_ValidArguments_InitializesAccountSuccessfully()
    {
        // Arrange & Act
        var account = new BankAccount("UA12345678", "Тарас Шевченко", 1000m);

        // Assert
        account.AccountNumber.Should().Be("UA12345678");
        account.Owner.Should().Be("Тарас Шевченко");
        account.Balance.Should().Be(1000m);
        account.IsFrozen.Should().BeFalse();
        account.Transactions.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyAccountNumber_ThrowsArgumentException(string invalidNumber)
    {
        // Act
        Action act = () => new BankAccount(invalidNumber, "Іван Франко", 100m);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("accountNumber");
    }

    [Fact]
    public void Deposit_PositiveAmount_IncreasesBalanceAndRecordsTransaction()
    {
        // Arrange
        var account = new BankAccount("UA001", "Леся Українка", 500m);

        // Act
        account.Deposit(250m);

        // Assert
        account.Balance.Should().Be(750m);
        account.Transactions.Should().HaveCount(1);
        var tx = account.Transactions[0];
        tx.Type.Should().Be("Deposit");
        tx.Amount.Should().Be(250m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    [InlineData(-1000)]
    public void Deposit_ZeroOrNegativeAmount_ThrowsArgumentOutOfRangeException(decimal invalidAmount)
    {
        // Arrange
        var account = new BankAccount("UA001", "Григорій Сковорода", 200m);

        // Act
        Action act = () => account.Deposit(invalidAmount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("amount");
    }

    [Fact]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        // Arrange
        var account = new BankAccount("UA001", "Богдан Хмельницький", 1000m);

        // Act
        account.Withdraw(400m);

        // Assert
        account.Balance.Should().Be(600m);
        account.Transactions.Should().ContainSingle(t => t.Type == "Withdrawal" && t.Amount == 400m);
    }

    [Fact]
    public void Withdraw_AmountGreaterThanBalance_ThrowsInsufficientFundsException()
    {
        // Arrange
        var account = new BankAccount("UA001", "Петро Могила", 300m);

        // Act
        Action act = () => account.Withdraw(500m);

        // Assert
        act.Should().Throw<InsufficientFundsException>()
            .WithMessage("*Недостатньо коштів*");
    }

    [Fact]
    public void Freeze_WhenFrozen_BlocksDepositAndWithdrawal()
    {
        // Arrange
        var account = new BankAccount("UA001", "Пилип Орлик", 500m);
        account.Freeze();

        // Act & Assert
        Action depositAct = () => account.Deposit(100m);
        depositAct.Should().Throw<AccountFrozenException>();

        Action withdrawAct = () => account.Withdraw(100m);
        withdrawAct.Should().Throw<AccountFrozenException>();
    }

    [Fact]
    public void TransferTo_DifferentAccount_TransfersFundsCorrectly()
    {
        // Arrange
        var source = new BankAccount("UA_SRC", "Відправник", 1000m);
        var target = new BankAccount("UA_TGT", "Отримувач", 200m);

        // Act
        source.TransferTo(target, 400m);

        // Assert
        source.Balance.Should().Be(600m);
        target.Balance.Should().Be(600m);
        source.Transactions.Should().ContainSingle(t => t.Type == "Withdrawal");
        target.Transactions.Should().ContainSingle(t => t.Type == "Deposit");
    }

    [Fact]
    public void TransferTo_SameAccount_ThrowsInvalidOperationException()
    {
        // Arrange
        var account = new BankAccount("UA_SAME", "Користувач", 500m);

        // Act
        Action act = () => account.TransferTo(account, 100m);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Неможливо виконати переказ на той самий банківський рахунок*");
    }

    [Theory]
    [MemberData(nameof(GetTransferTestCases))]
    public void TransferTo_ParameterizedScenarios_VerifySourceBalance(decimal initial, decimal transferAmount, decimal expectedSourceBalance)
    {
        // Arrange
        var source = new BankAccount("UA_SRC", "Тест", initial);
        var target = new BankAccount("UA_TGT", "Ціль", 0m);

        // Act
        source.TransferTo(target, transferAmount);

        // Assert
        source.Balance.Should().Be(expectedSourceBalance);
        target.Balance.Should().Be(transferAmount);
    }

    public static IEnumerable<object[]> GetTransferTestCases()
    {
        yield return new object[] { 1000m, 100m, 900m };
        yield return new object[] { 500m, 500m, 0m };
        yield return new object[] { 1500m, 250.50m, 1249.50m };
    }
}
