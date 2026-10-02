# Розділ 1: Фундамент Модульного Тестування з xUnit та FluentAssertions

## 1. Філософія та Цілі Unit-тестування
**Модульне тестування (Unit Testing)** — це автоматизована перевірка мінімальних ізольованих фрагментів коду (методів, класів, інваріантів) у повній ізоляції від зовнішніх залежностей (мережі, дисків, сторонніх API).

### Основні цілі:
1. **Швидкий зворотний зв'язок (Fast Feedback):** Тисячі unit-тестів мають виконуватися за лічені секунди.
2. **Безпечний рефакторинг:** Можливість змінювати внутрішню структуру коду, будучи впевненим, що бізнес-правила не порушено.
3. **Жива документація:** Тести демонструють, як повинен поводитися клас за різних граничних умов.

---

## 2. Шаблон AAA (Arrange - Act - Assert)
Кожен модульний тест складається з трьох чітких фаз:

```csharp
[Fact]
public void Deposit_PositiveAmount_IncreasesBalance()
{
    // 1. Arrange (Підготовка даних та об'єкта під тестом)
    var account = new BankAccount("UA001", "Олександр", 500m);

    // 2. Act (Виконання досліджуваної дії)
    account.Deposit(200m);

    // 3. Assert (Перевірка очікуваного результату)
    account.Balance.Should().Be(700m);
}
```

---

## 3. Фреймворк xUnit: `[Fact]` vs `[Theory]`

В xUnit існує два головні типи тестів:
- **`[Fact]`**: тест без параметрів, що перевіряє один фіксований сценарій.
- **`[Theory]`**: параметризований тест, який запускається багаторазово з різними наборами вхідних даних.

### Передача параметрів через `[InlineData]`:
```csharp
[Theory]
[InlineData(0)]
[InlineData(-50)]
[InlineData(-1000)]
public void Deposit_InvalidAmount_ThrowsArgumentOutOfRangeException(decimal invalidAmount)
{
    var account = new BankAccount("UA001", "Тест", 100m);

    Action act = () => account.Deposit(invalidAmount);

    act.Should().Throw<ArgumentOutOfRangeException>()
        .WithParameterName("amount");
}
```

### Передача складних наборів через `[MemberData]`:
```csharp
[Theory]
[MemberData(nameof(GetTransferScenarios))]
public void Transfer_Scenarios(decimal initial, decimal transfer, decimal expected)
{
    var source = new BankAccount("UA_1", "User", initial);
    var target = new BankAccount("UA_2", "User", 0m);

    source.TransferTo(target, transfer);

    source.Balance.Should().Be(expected);
}

public static IEnumerable<object[]> GetTransferScenarios()
{
    yield return new object[] { 1000m, 100m, 900m };
    yield return new object[] { 500m, 500m, 0m };
}
```

---

## 4. Виразні твердження: Чому FluentAssertions?

Замість традиційних `Assert.Equal(expected, actual)`, бібліотека **FluentAssertions** дозволяє писати природні ланцюжки тверджень, які генерують максимально зрозумілі діагностичні повідомлення при падінні тесту:

```csharp
// FluentAssertions:
account.Balance.Should().Be(1500m);
account.Transactions.Should().HaveCount(2)
    .And.ContainSingle(t => t.Type == "Deposit");

// Перевірка винятків:
Action act = () => account.Withdraw(9999m);
act.Should().Throw<InsufficientFundsException>()
    .WithMessage("*Недостатньо коштів*");
```

---

## 5. Життєвий цикл тестів в xUnit
На відміну від NUnit чи MSTest, **xUnit створює новий екземпляр тестового класу для кожного окремого тестового методу**.
- Ініціалізація перед тестом: **Конструктор класу**.
- Очищення після тесту: реалізація **`IDisposable`** або **`IAsyncLifetime`**.
- Це гарантує 100% взаємну ізоляцію тестів і запобігає передачі стану між тестами.
