# Розділ 04: Альтернативний мокінг із NSubstitute та виразні перевірки FluentAssertions

У проєкті [Testing.UnitTests](../Testing.UnitTests/) продемонстровано поєднання двох популярних бібліотек сучасної екосистеми .NET:
1. **NSubstitute** — лаконічна альтернатива бібліотеці `Moq` без лямбда-виразів `Setup(x => ...)` та `.Object`;
2. **FluentAssertions** — бібліотека для запису перевірок у вигляді читабельних ланцюжків `.Should().Be(...)`.

---

## 1. Порівняння синтаксису: Moq проти NSubstitute

| Дія | Як пишеться в **Moq** ([03-Testing.Moq](../03-Testing.Moq/)) | Як пишеться в **NSubstitute** ([Testing.UnitTests](../Testing.UnitTests/)) |
| :--- | :--- | :--- |
| **Створення мока** | `var mock = new Mock<IPaymentGateway>();` | `var sub = Substitute.For<IPaymentGateway>();` |
| **Передача у конструктор** | `new Service(mock.Object)` | `new Service(sub)` (сам об'єкт вже є інтерфейсом!) |
| **Налаштування відповіді** | `mock.Setup(x => x.ChargeAsync(...)).ReturnsAsync("tx");` | `sub.ChargeAsync(...).Returns("tx");` |
| **Матчер будь-якого значення** | `It.IsAny<CancellationToken>()` | `Arg.Any<CancellationToken>()` |
| **Матчер за умовою** | `It.Is<decimal>(a => a > 0)` | `Arg.Is<decimal>(a => a > 0)` |
| **Перевірка 1 виклику** | `mock.Verify(x => x.ChargeAsync(...), Times.Once);` | `await sub.Received(1).ChargeAsync(...);` |
| **Перевірка відсутності виклику** | `mock.Verify(x => x.ChargeAsync(...), Times.Never);` | `await sub.DidNotReceive().ChargeAsync(...);` |

---

## 2. Практичні патерни роботи з NSubstitute ([OrderCheckoutServiceTests.cs](../Testing.UnitTests/OrderCheckoutServiceTests.cs))

### А. Створення моків та налаштування поведінки
```csharp
private readonly IPaymentGateway _paymentGateway = Substitute.For<IPaymentGateway>();
private readonly IInventoryService _inventoryService = Substitute.For<IInventoryService>();
private readonly INotificationService _notificationService = Substitute.For<INotificationService>();

// Налаштування повернення значень:
_inventoryService.ReserveStockAsync(request.Items, Arg.Any<CancellationToken>())
    .Returns(Task.FromResult(true));

_paymentGateway.ChargeAsync(request.CustomerEmail, expectedTotal, Arg.Any<CancellationToken>())
    .Returns(Task.FromResult("tx_987654"));
```

### Б. Верифікація викликів
```csharp
await _inventoryService.Received(1).ReserveStockAsync(request.Items, Arg.Any<CancellationToken>());
await _paymentGateway.Received(1).ChargeAsync(request.CustomerEmail, expectedTotal, Arg.Any<CancellationToken>());
await _inventoryService.DidNotReceive().ReleaseStockAsync(Arg.Any<IEnumerable<OrderItemDto>>(), Arg.Any<CancellationToken>());
```

---

## 3. Виразні твердження з FluentAssertions ([BankAccountTests.cs](../Testing.UnitTests/BankAccountTests.cs))

Замість стандартного `Assert.Equal(expected, actual)` бібліотека **FluentAssertions** дозволяє будувати природні ланцюжки перевірок з надзвичайно детальними повідомленнями у разі падіння тесту:

```csharp
// Перевірка властивостей та колекцій:
account.Balance.Should().Be(750m);
account.Transactions.Should().HaveCount(1)
    .And.ContainSingle(t => t.Type == "Deposit" && t.Amount == 250m);

// Перевірка синхронних та асинхронних винятків:
Action act = () => account.Withdraw(500m);
act.Should().Throw<InsufficientFundsException>()
    .WithMessage("*Недостатньо коштів*");
```
