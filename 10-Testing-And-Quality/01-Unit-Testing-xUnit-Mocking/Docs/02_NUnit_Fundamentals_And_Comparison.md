# Розділ 02: Модульне тестування з NUnit та порівняння з xUnit

**NUnit** — один із найстаріших та найпоширеніших фреймворків тестування в екосистемі .NET. Знання як **xUnit**, так і **NUnit** є обов'язковим для кожного .NET-розробника, оскільки обидва фреймворки масово використовуються в комерційних проєктах.

Усі приклади цього розділу реалізовані у проєкті [02-Testing.NUnit](../02-Testing.NUnit/).

---

## 1. Наочна таблиця порівняння: xUnit проти NUnit

| Концепція | Як це пишеться в **xUnit** | Як це пишеться в **NUnit** |
| :--- | :--- | :--- |
| **Позначення класу тестів** | Атрибут не потрібен (`public class ...`) | `[TestFixture]` над класом |
| **Одиничний тест без параметрів** | `[Fact]` | `[Test]` |
| **Параметризований тест (inline)** | `[Theory]` + `[InlineData(1, 2)]` | `[TestCase(1, 2)]` або `[TestCase(1, ExpectedResult = 2)]` |
| **Зовнішнє джерело тестових даних** | `[MemberData(nameof(Method))]` | `[TestCaseSource(nameof(Method))]` |
| **Ініціалізація перед кожним тестом** | Конструктор класу (`public MyTests()`) | Метод з атрибутом `[SetUp]` |
| **Очищення після кожного тесту** | Реалізація `IDisposable.Dispose()` | Метод з атрибутом `[TearDown]` |
| **Створення екземпляра класу** | **Новий екземпляр** на кожен тест | **Один екземпляр** на всі тести класу (тому потрібен `[SetUp]`) |
| **Стиль перевірок (Assertions)** | `Assert.Equal(expected, actual)` | Constraint-модель: `Assert.That(actual, Is.EqualTo(expected))` |

---

## 2. Життєвий цикл NUnit: чому важливий `[SetUp]`?

На відміну від xUnit, фреймворк **NUnit** за замовчуванням створює **лише один об'єкт класу тестів** і послідовно викликає на ньому всі тестові методи.

Тому в NUnit **не можна** ініціалізувати змінювані об'єкти (наприклад, `BankAccount`) просто в конструкторі чи при оголошенні поля — інакше перший тест змінить баланс рахунку, а другий тест отримає вже змінений рахунок!

Натомість у NUnit використовують атрибут **`[SetUp]`** ([BankAccountNUnitTests.cs](../02-Testing.NUnit/BankAccountNUnitTests.cs)):

```csharp
[TestFixture]
public class BankAccountNUnitTests
{
    private BankAccount _account = null!;

    [SetUp]
    public void SetUpBeforeEachTest()
    {
        // Цей метод NUnit гарантовано викликає ПЕРЕД КОЖНИМ тестом!
        _account = new BankAccount("UA-NUNIT-01", "Іван Франко", initialBalance: 1000m);
    }

    [Test]
    public void Deposit_PositiveAmount_UpdatesBalance()
    {
        _account.Deposit(250m);
        Assert.That(_account.Balance, Is.EqualTo(1250m));
    }
}
```

---

## 3. Фірмова особливість NUnit: Constraint Model (`Assert.That`)

У сучасному NUnit (версії 3 та 4) усі перевірки будуються навколо єдиного методу **`Assert.That(actual, constraint)`**, який читається як звичайне речення англійською мовою:

```csharp
// 1. Перевірка рівності та діапазону
Assert.That(finalPrice, Is.EqualTo(900m));
Assert.That(finalPrice, Is.InRange(800m, 1000m));

// 2. Перевірка колекцій
Assert.That(_account.Transactions, Has.Count.EqualTo(1));
Assert.That(_account.Transactions, Has.One.Matches<BankTransaction>(t => t.Type == "Withdrawal"));

// 3. Групова перевірка Assert.Multiple (виконує ВСІ перевірки у блоці, навіть якщо перша впала!)
Assert.Multiple(() => {
    Assert.That(_account.Balance, Is.EqualTo(1250m));
    Assert.That(_account.IsFrozen, Is.False);
});

// 4. Перевірка винятків та їхніх властивостей
Assert.That(
    () => _sut.ApplyPercentageDiscount(1000m, 75m),
    Throws.TypeOf<ArgumentOutOfRangeException>()
          .With.Property("ParamName").EqualTo("discountPercent")
          .And.Message.Contain("від 0% до 50%"));
```

---

## 4. Елегантна властивість `ExpectedResult` в `[TestCase]`

У NUnit параметризований тест може сам повертати обчислене значення, а NUnit автоматично порівняє його з `ExpectedResult`:

```csharp
[TestCase("WELCOME10",    ExpectedResult = 10)]
[TestCase("  student20 ", ExpectedResult = 20)]
[TestCase("VIP30",        ExpectedResult = 30)]
public decimal GetPromoCodeDiscountPercent_ValidCodes_ReturnsExpectedPercent(string? promoCode)
{
    return _sut.GetPromoCodeDiscountPercent(promoCode);
}
```
