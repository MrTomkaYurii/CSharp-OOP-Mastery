# Розділ 01: Фундамент модульного тестування з xUnit (`[Fact]`, `[Theory]`, `AAA`, `Assert`)

## 1. Що таке модульне тестування (Unit Testing)?
**Модульне тестування (Unit Testing)** — це автоматизована перевірка найменших логічних одиниць коду (окремих методів і класів) у повній ізоляції від зовнішніх систем (баз даних, платіжних шлюзів, файлової системи чи мережі).

### Навіщо ми пишемо модульні тести?
1. **Миттєвий зворотний зв'язок:** Сотні тестів виконуються за частки секунди після натискання `Run Tests`.
2. **Безпечний рефакторинг:** Ви можете змінювати внутрішній код класу, знаючи, що зелені тести гарантують збереження бізнес-правил.
3. **Жива документація:** Тести наочно показують, як клас має працювати у звичайних та граничних ситуаціях.

---

## 2. Анатомія тесту: Патерн AAA (Arrange – Act – Assert)

Кожен хороший модульний тест складається з трьох візуально відокремлених блоків ([PriceAndDiscountCalculatorTests.cs](../01-Testing.xUnit/PriceAndDiscountCalculatorTests.cs)):

```csharp
[Fact]
public void ApplyPercentageDiscount_Valid10Percent_Returns900For1000()
{
    // 1. Arrange (Підготовка даних та створення об'єкта під тестом — SUT)
    var sut = new PriceAndDiscountCalculator();
    decimal originalPrice = 1000m;
    decimal discountPercent = 10m;

    // 2. Act (Виклик одного досліджуваного методу)
    decimal finalPrice = sut.ApplyPercentageDiscount(originalPrice, discountPercent);

    // 3. Assert (Перевірка, що фактичний результат збігається з очікуваним)
    Assert.Equal(900m, finalPrice);
}
```

> **Правило найменування тестів:** `НазваМетоду_СценарійАбоВхідніУмови_ОчікуванийРезультат` (наприклад, `Withdraw_MoreThanBalance_ThrowsInsufficientFundsException`).

---

## 3. Два головні атрибути xUnit: `[Fact]` та `[Theory]`

| Атрибут | Коли використовується | Приклад |
| :--- | :--- | :--- |
| **`[Fact]`** | Тест без параметрів, який перевіряє один конкретний сценарій. | Перевірка створення рахунку з початковим балансом. |
| **`[Theory]`** | Параметризований тест, який запускається кілька разів із різними вхідними наборами даних. | Перевірка розрахунку знижки для `0%`, `15%`, `50%`. |

### 3.1. Передача даних через `[InlineData]`
```csharp
[Theory]
[InlineData(1000, 0,  1000)]
[InlineData(1000, 15, 850)]
[InlineData(200,  50, 100)]
public void ApplyPercentageDiscount_VariousValidDiscounts_CalculatesExpectedPrice(
    decimal originalPrice,
    decimal discountPercent,
    decimal expectedFinalPrice)
{
    decimal actualPrice = _sut.ApplyPercentageDiscount(originalPrice, discountPercent);
    Assert.Equal(expectedFinalPrice, actualPrice);
}
```

### 3.2. Передача даних через `[MemberData]`
Коли вхідні дані є складними об'єктами або типами `decimal`/`DateTime`, які не завжди зручно передавати у константних атрибутах, використовують статичний метод-генератор:
```csharp
public static IEnumerable<object[]> GetVatTestCases()
{
    yield return new object[] { 100m, 20m, 120m };
    yield return new object[] { 100m, 7m,  107m };
}

[Theory]
[MemberData(nameof(GetVatTestCases))]
public void CalculatePriceWithVat_MemberDataScenarios_ReturnsCorrectTotal(
    decimal netPrice, decimal vatPercent, decimal expectedTotal)
{
    Assert.Equal(expectedTotal, _sut.CalculatePriceWithVat(netPrice, vatPercent));
}
```

---

## 4. Перевірка генерації винятків (`Assert.Throws<T>`)

Перевірка того, що клас вчасно зупиняє некоректну операцію винятком — не менш важлива, ніж перевірка успішного сценарію:

```csharp
[Theory]
[InlineData(-1)]
[InlineData(51)]
public void ApplyPercentageDiscount_DiscountOutOfRange_ThrowsArgumentOutOfRangeException(decimal invalidDiscount)
{
    var ex = Assert.Throws<ArgumentOutOfRangeException>(
        () => _sut.ApplyPercentageDiscount(1000m, invalidDiscount));

    Assert.Equal("discountPercent", ex.ParamName);
}
```

---

## 5. Життєвий цикл та ізоляція тестів в xUnit ([BankAccountXUnitTests.cs](../01-Testing.xUnit/BankAccountXUnitTests.cs))

У фреймворку **xUnit** немає атрибутів `[SetUp]` чи `[TearDown]`. Замість цього xUnit використовує стандартні механізми ООП мови C#:
- **Перед кожним тестом** xUnit створює **новий екземпляр класу тестів** (викликається **Конструктор**).
- **Після кожного тесту** xUnit викликає метод **`Dispose()`** (якщо клас реалізує `IDisposable`).

Це гарантує, що жоден тест не може випадково зіпсувати стан об'єкта для наступного тесту!
