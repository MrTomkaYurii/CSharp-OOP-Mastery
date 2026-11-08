# Модуль 10: Модульне тестування в C# — xUnit, NUnit, Moq, NSubstitute та EF Core SQLite In-Memory

Цей навчальний модуль розроблений спеціально для послідовної, наочної демонстрації сучасних інструментів тестування у .NET:
1. **`xUnit`** (`01-Testing.xUnit`) — патерн AAA, одиничні тести `[Fact]`, параметризовані тести `[Theory]` з `[InlineData]` та `[MemberData]`, перевірка винятків `Assert.Throws<T>`, ізоляція стану через конструктор та `IDisposable`.
2. **`NUnit`** (`02-Testing.NUnit`) — тестування тих самих бізнес-класів на фреймворку NUnit для прямого порівняння: `[TestFixture]`, `[Test]`, `[TestCase]`, `[TestCaseSource]`, життєвий цикл `[SetUp]` / `[TearDown]` та декларативна Constraint-модель `Assert.That(actual, Is.EqualTo(expected))`.
3. **`Moq`** (`03-Testing.Moq`) — 6 покрокових уроків із найпопулярнішої бібліотеки мокінгу `Moq`: створення `new Mock<T>()` та передача `.Object`, налаштування `Setup(...).ReturnsAsync(...)`, матчери аргументів `It.IsAny<T>()`, `It.Is<T>(...)`, `It.IsInRange(...)`, імітація аварій через `ThrowsAsync(...)`, верифікація кількості викликів `Verify(..., Times.Once / Times.Never / Times.Exactly(2))`, перехоплення аргументів через `Callback(...)` та послідовності `SetupSequence(...)`.
4. **`NSubstitute` + `FluentAssertions` + `EF Core SQLite In-Memory`** (`Testing.UnitTests`, `Testing.EFCore.InMemoryTests`) — сучасні альтернативи мокінгу, виразні ланцюжки перевірок `.Should().Be(...)` та інтеграційне тестування репозиторіїв БД у пам'яті з генератором `Bogus`.

---

## 1. Навігація по теоретичних матеріалах (папка `Docs`)

| № | Назва документа | Ключові теми |
| :-: | :--- | :--- |
| **01** | [Docs/01_xUnit_Fundamentals.md](Docs/01_xUnit_Fundamentals.md) | Фундамент `xUnit`: патерн AAA, `[Fact]`, `[Theory]`, `[InlineData]`, `[MemberData]`, життєвий цикл. |
| **02** | [Docs/02_NUnit_Fundamentals_And_Comparison.md](Docs/02_NUnit_Fundamentals_And_Comparison.md) | Фреймворк `NUnit` та детальна таблиця порівняння `xUnit vs NUnit`, `[SetUp]`, `Assert.That(...)`. |
| **03** | [Docs/03_Mocking_With_Moq_StepByStep.md](Docs/03_Mocking_With_Moq_StepByStep.md) | **Покроковий гайд по бібліотеці `Moq`**: 6 кроків від `new Mock<T>()` до `Verify`, `Callback` і `SetupSequence`. |
| **04** | [Docs/04_Mocking_With_NSubstitute_And_FluentAssertions.md](Docs/04_Mocking_With_NSubstitute_And_FluentAssertions.md) | Порівняння `Moq vs NSubstitute` та декларативні твердження з `FluentAssertions`. |
| **05** | [Docs/05_Testing_EFCore_SQLite_InMemory.md](Docs/05_Testing_EFCore_SQLite_InMemory.md) | Інтеграційне тестування репозиторіїв `EF Core` на базі справжнього `SQLite In-Memory` та `Bogus`. |

---

## 2. Структура рішення `OOP-Unit-Testing-Masterclass.sln` (5 навчальних Solution-тек)

Відкрийте файл **`OOP-Unit-Testing-Masterclass.sln`** (або `UnitTestingAndMocking.slnx`) у Visual Studio. Проєкти згруповані у 5 логічних кроків для проведення заняття:

* **`01. Система під тестуванням (System Under Test - Бізнес-логіка та контракти)`**
  * [`Testing.Core.Domain`](Testing.Core.Domain/) — бізнес-класи та інтерфейси, які ми тестуємо:
    * [`PriceAndDiscountCalculator`](Testing.Core.Domain/Services/PriceAndDiscountCalculator.cs) — чисті методи розрахунку знижок, промокодів і ПДВ;
    * [`BankAccount`](Testing.Core.Domain/Entities/BankAccount.cs) — банківський рахунок зі станом, історією транзакцій, замороженням і переказами;
    * [`OrderCheckoutService`](Testing.Core.Domain/Services/OrderCheckoutService.cs) — сервіс оформлення замовлення із 4 зовнішніми залежностями (`IInventoryService`, `IPaymentGateway`, `INotificationService`, `IAuditLogger`).
* **`02. Модульне тестування з xUnit (Fact, Theory, InlineData, MemberData)`**
  * [`01-Testing.xUnit`](01-Testing.xUnit/) — класичні модульні тести на чистому `xUnit` ([`PriceAndDiscountCalculatorTests.cs`](01-Testing.xUnit/PriceAndDiscountCalculatorTests.cs), [`BankAccountXUnitTests.cs`](01-Testing.xUnit/BankAccountXUnitTests.cs)).
* **`03. Модульне тестування з NUnit (TestFixture, Test, TestCase, SetUp, Constraint Model)`**
  * [`02-Testing.NUnit`](02-Testing.NUnit/) — ті самі бізнес-сценарії, реалізовані на фреймворку `NUnit` ([`PriceAndDiscountCalculatorNUnitTests.cs`](02-Testing.NUnit/PriceAndDiscountCalculatorNUnitTests.cs), [`BankAccountNUnitTests.cs`](02-Testing.NUnit/BankAccountNUnitTests.cs)).
* **`04. Ізоляція залежностей та Мокінг з бібліотекою Moq`**
  * [`03-Testing.Moq`](03-Testing.Moq/) — 6 покрокових уроків із бібліотеки `Moq` ([`OrderCheckoutMoqStepByStepTests.cs`](03-Testing.Moq/OrderCheckoutMoqStepByStepTests.cs)).
* **`05. Альтернативний мокінг (NSubstitute), FluentAssertions та тестування EF Core (SQLite In-Memory)`**
  * [`Testing.UnitTests`](Testing.UnitTests/) — поєднання `xUnit` + `NSubstitute` + `FluentAssertions`.
  * [`Testing.EFCore.InMemoryTests`](Testing.EFCore.InMemoryTests/) — тестування `CustomerRepository` на базі `SQLite In-Memory` (`DataSource=:memory:`) та `Bogus`.

---

## 3. Як запустити всі тести

Через консоль .NET CLI (або через вікно **Test Explorer** у Visual Studio):

```bash
dotnet test OOP-Unit-Testing-Masterclass.sln
```
