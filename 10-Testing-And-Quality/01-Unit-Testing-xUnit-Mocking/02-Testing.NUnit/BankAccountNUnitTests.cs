using NUnit.Framework;
using Testing.Core.Domain.Entities;

namespace Testing.NUnit;

/// <summary>
/// УРОК 4: Тестування класу BankAccount у фреймворку NUnit.
/// 
/// Чому в NUnit ОБОВ'ЯЗКОВО використовувати [SetUp] для ініціалізації стану?
/// На відміну від xUnit, NUnit за замовчуванням створює ЛИШЕ ОДИН екземпляр класу BankAccountNUnitTests
/// для всіх методів [Test] всередині нього!
/// Тому якби ми створили _account у конструкторі, зміни балансу з одного тесту потекли б у наступний тест.
/// А метод [SetUp] викликається перед кожним тестом і гарантовано перестворює рахунок з балансом 1000 грн!
/// </summary>
[TestFixture]
public class BankAccountNUnitTests
{
    private BankAccount _account = null!;

    [SetUp]
    public void SetUpBeforeEachTest()
    {
        // Скидаємо стан перед кожним тестом!
        _account = new BankAccount("UA-NUNIT-01", "Іван Франко", initialBalance: 1000m);
    }

    [TearDown]
    public void TearDownAfterEachTest()
    {
        // Виконується після кожного тесту
    }

    [Test]
    public void Deposit_PositiveAmount_UpdatesBalanceAndRecordsTransaction()
    {
        // Act
        _account.Deposit(250m);

        // Assert (Можна групувати кілька перевірок через Assert.Multiple, щоб побачити всі помилки одразу!)
        Assert.Multiple(() =>
        {
            Assert.That(_account.Balance, Is.EqualTo(1250m));
            Assert.That(_account.Transactions, Has.Count.EqualTo(1));
            Assert.That(_account.Transactions[0].Type, Is.EqualTo("Deposit"));
            Assert.That(_account.Transactions[0].Amount, Is.EqualTo(250m));
        });
    }

    [Test]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        // Завдяки [SetUp] тут баланс знову 1000m, а не 1250m!
        _account.Withdraw(400m);

        Assert.That(_account.Balance, Is.EqualTo(600m));
        Assert.That(_account.Transactions, Has.One.Matches<BankTransaction>(t => t.Type == "Withdrawal" && t.Amount == 400m));
    }

    [Test]
    public void Withdraw_ExceedingBalance_ThrowsInsufficientFundsException()
    {
        Assert.That(
            () => _account.Withdraw(2000m),
            Throws.TypeOf<InsufficientFundsException>()
                  .With.Message.Contain("Недостатньо коштів"));
    }

    [Test]
    public void TransferTo_SameAccount_ThrowsInvalidOperationException()
    {
        Assert.That(
            () => _account.TransferTo(_account, 100m),
            Throws.TypeOf<InvalidOperationException>()
                  .With.Message.Contain("той самий банківський рахунок"));
    }
}
