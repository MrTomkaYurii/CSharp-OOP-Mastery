# Розділ 03: Покроковий посібник із мокінгу за допомогою бібліотеки Moq

Усі практичні приклади цього розділу знаходяться у проєкті [03-Testing.Moq/OrderCheckoutMoqStepByStepTests.cs](../03-Testing.Moq/OrderCheckoutMoqStepByStepTests.cs).

---

## 1. Навіщо потрібен Мокінг (Mocking)?

Уявіть, що ми тестуємо сервіс оформлення замовлення [`OrderCheckoutService`](../Testing.Core.Domain/Services/OrderCheckoutService.cs).
Для своєї роботи він викликає 4 зовнішні інтерфейси:
1. `IInventoryService` — перевіряє і резервує товар у базі складу;
2. `IPaymentGateway` — звертається до банку і списує гроші з картки клієнта;
3. `INotificationService` — відправляє клієнту Email-підтвердження;
4. `IAuditLogger` — записує подію в журнал безпеки.

**Проблема:** Ми не можемо під час запуску модульних тестів списувати справжні гроші з банківських карток, відправляти тисячі реальних листів або чекати відповіді від зовнішніх серверів!

**Рішення — бібліотека `Moq`:** Вона на льоту створює **об'єкти-імітатори (Mocks)** для будь-якого інтерфейсу C#, якими ми керуємо прямо з коду тесту!

---

## 2. Шість кроків опанування бібліотеки Moq

### Крок 1: Створення `Mock<T>` та властивість `.Object`
Коли ви пишете `new Mock<IPaymentGateway>()`, ви створюєте **пульт керування імітатором** (`Mock<IPaymentGateway>`).
А щоб передати сам імітований інтерфейс `IPaymentGateway` у конструктор вашого сервісу, використовується властивість **`.Object`**:

```csharp
// 1. Створюємо пульти керування моками
var paymentMock = new Mock<IPaymentGateway>();
var inventoryMock = new Mock<IInventoryService>();
var notificationMock = new Mock<INotificationService>();
var auditMock = new Mock<IAuditLogger>();

// 2. Передаємо .Object у конструктор досліджуваного сервісу (SUT)
var sut = new OrderCheckoutService(
    paymentMock.Object,
    inventoryMock.Object,
    notificationMock.Object,
    auditMock.Object);
```

---

### Крок 2: Налаштування поведінки через `Setup(...)` та `ReturnsAsync(...)`
За допомогою методу `.Setup(...)` ми вказуємо: *«Коли сервіс викличе ось цей метод мока — поверни йому ось таке значення»*:

```csharp
// Налаштовуємо асинхронну відповідь складу: товар є в наявності (true)
_inventoryMock
    .Setup(x => x.ReserveStockAsync(request.Items, It.IsAny<CancellationToken>()))
    .ReturnsAsync(true);

// Налаштовуємо платіжний шлюз: повернути номер транзакції "TX-MOQ-001"
_paymentMock
    .Setup(x => x.ChargeAsync("buyer@example.ua", 1000m, It.IsAny<CancellationToken>()))
    .ReturnsAsync("TX-MOQ-001");
```

---

### Крок 3: Гнучкі матчери аргументів (`It.IsAny`, `It.Is`, `It.IsInRange`)
Часто ми не знаємо точного значення аргументу або хочемо перевірити лише певну умову. Для цього клас **`It`** надає три основні матчери:

| Матчер у Moq | Що означає | Приклад використання |
| :--- | :--- | :--- |
| **`It.IsAny<T>()`** | Будь-яке значення типу `T`. | `It.IsAny<CancellationToken>()` |
| **`It.Is<T>(predicate)`** | Значення типу `T`, що задовольняє умову лямбда-виразу. | `It.Is<string>(email => email.EndsWith(".ua"))` |
| **`It.IsInRange(from, to, Range)`** | Значення у вказаному діапазоні. | `It.IsInRange(100m, 5000m, Moq.Range.Inclusive)` |

---

### Крок 4: Імітація помилок та збоїв через `ThrowsAsync(...)`
Як перевірити, чи поверне наш сервіс товар назад на склад, якщо банк відхилив картку? Дуже просто — наказуємо моку кинути виняток:

```csharp
_paymentMock
    .Setup(x => x.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
    .ThrowsAsync(new InvalidOperationException("Банк відхилив транзакцію: недостатній ліміт."));
```

---

### Крок 5: Перевірка факту та кількості викликів через `Verify(..., Times)`
Іноді головна мета тесту — переконатися, що певний зовнішній метод **був викликаний** (наприклад, повернення товару на склад при помилці оплати) або **НЕ був викликаний** (не можна списувати гроші, якщо товару немає на складі!):

```csharp
// Перевіряємо, що резервування викликалося РІВНО 1 РАЗ:
_inventoryMock.Verify(
    x => x.ReserveStockAsync(request.Items, It.IsAny<CancellationToken>()),
    Times.Once);

// Перевіряємо, що платіжний шлюз НЕ викликався ЖОДНОГО РАЗУ:
_paymentMock.Verify(
    x => x.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()),
    Times.Never);

// Перевіряємо, що при повторній спробі (Retry) шлюз викликався РІВНО 2 РАЗИ:
_paymentMock.Verify(
    x => x.ChargeAsync("retry@example.ua", 300m, It.IsAny<CancellationToken>()),
    Times.Exactly(2));
```

---

### Крок 6: Перехоплення аргументів (`Callback`) та послідовності (`SetupSequence`)

* **`.Callback<T1, T2>(...)`** дозволяє виконати довільний код у момент, коли сервіс звертається до мока (наприклад, зберегти передані аргументи у список для детальної перевірки):
  ```csharp
  _auditMock
      .Setup(x => x.LogAudit(It.IsAny<string>(), It.IsAny<string>()))
      .Callback<string, string>((eventName, details) => capturedLogs.Add((eventName, details)));
  ```

* **`.SetupSequence(...)`** дозволяє задати різні відповіді на послідовні виклики одного й того ж методу (ідеально для тестування логіки Retry):
  ```csharp
  _paymentMock
      .SetupSequence(x => x.ChargeAsync("retry@example.ua", 300m, It.IsAny<CancellationToken>()))
      .ThrowsAsync(new TimeoutException("1-ша спроба: таймаут"))
      .ReturnsAsync("TX-RETRY-OK"); // 2-га спроба: успіх!
  ```
