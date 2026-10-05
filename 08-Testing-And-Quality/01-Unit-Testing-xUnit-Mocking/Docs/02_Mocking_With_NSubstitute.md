# Розділ 2: Ізоляція Залежностей та Мокінг через NSubstitute

## 1. Концепція Test Doubles (Дублери тестів)

У реальних бізнес-сервісах клас часто співпрацює з іншими компонентами: платіжними шлюзами, відправниками email, базами даних. Для перевірки класу в ізоляції використовують дублери тестів (Test Doubles):

- **Dummy:** об'єкти-заглушки для заповнення списку параметрів.
- **Stub (Заглушка):** повертає заздалегідь заготовлені відповіді на виклики.
- **Mock (Макет):** об'єкт, що перевіряє факт, порядок та аргументи викликів своїх методів.

---

## 2. Чому саме NSubstitute?
У сучасному .NET-розробленні **NSubstitute** витіснив застарілі Moq/RhinoMocks завдяки:
- Лаконічному синтаксису без громіздких лямбда-виразів (`It.IsAny<T>()`).
- Відсутності прихованої телеметрії та ліцензійних скандалів.
- Безшовній інтеграції з C# async/await та nullable types.

---

## 3. Практичні патерни роботи з NSubstitute

### А. Створення моку
```csharp
var paymentGateway = Substitute.For<IPaymentGateway>();
var notificationService = Substitute.For<INotificationService>();
```

### Б. Налаштування повернення значень (Stubbing)
```csharp
// Налаштування фіксованого результату для будь-яких аргументів
paymentGateway.ChargeAsync("user@ex.ua", 1000m, Arg.Any<CancellationToken>())
    .Returns(Task.FromResult("tx_12345"));

// Налаштування помилки
paymentGateway.ChargeAsync("bad@ex.ua", Arg.Any<decimal>(), Arg.Any<CancellationToken>())
    .Returns<Task<string>>(_ => throw new InvalidOperationException("Картка заблокована"));
```

### В. Верифікація викликів (Assertions on Mocks)
```csharp
// Перевірка, що метод було викликано РІВНО 1 РАЗ з конкретними аргументами:
await paymentGateway.Received(1).ChargeAsync("user@ex.ua", 1000m, Arg.Any<CancellationToken>());

// Перевірка, що метод НЕ викликався взагалі:
await notificationService.DidNotReceive().SendOrderConfirmationAsync(
    Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<CancellationToken>());
```

### Г. Зіставлення аргументів через `Arg.Is<T>`
```csharp
await inventoryService.Received(1).ReserveStockAsync(
    Arg.Is<IEnumerable<OrderItemDto>>(items => items.Any(i => i.Sku == "SKU-01")),
    Arg.Any<CancellationToken>());
```

---

## 4. Тестування компенсуючих транзакцій (Saga / Rollback logic)

Коли зовнішня дія (наприклад, списання коштів) дає збій, надійна система повинна виконати відкат (наприклад, повернути зарезервований товар на склад). NSubstitute дозволяє ідеально протестувати такий сценарій:

```csharp
[Fact]
public async Task Checkout_WhenPaymentFails_ReleasesReservedStock()
{
    // Імітуємо падіння платіжного шлюзу
    paymentGateway.ChargeAsync(Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<CancellationToken>())
        .Returns<Task<string>>(_ => throw new Exception("Network timeout"));

    Func<Task> act = async () => await sut.ProcessOrderAsync(request);

    await act.Should().ThrowAsync<PaymentFailedException>();

    // Переконуємося, що компенсуючий метод повернення товару на склад викликано!
    await inventoryService.Received(1).ReleaseStockAsync(request.Items, Arg.Any<CancellationToken>());
}
```
