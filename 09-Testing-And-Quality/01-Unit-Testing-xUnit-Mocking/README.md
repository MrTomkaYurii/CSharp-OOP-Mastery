# Модуль: Unit Testing & Mocking у .NET 10 (xUnit, NSubstitute, FluentAssertions)

Цей модуль присвячений інженерії якості, практикам модульного тестування чистої об'єктно-орієнтованої бізнес-логіки та інтеграційному тестуванню шар доступу до даних (EF Core) з використанням **xUnit**, **NSubstitute**, **FluentAssertions** та генератора даних **Bogus**.

---

## Структура проєктів

```
01-Unit-Testing-xUnit-Mocking/
├── UnitTestingAndMocking.slnx          # Файл рішення .NET 10
├── Testing.Core.Domain/                # Бізнес-логіка (Domain Models & Services)
│   ├── Entities/
│   │   └── BankAccount.cs              # Бізнес-інваріанти, винятки, події
│   ├── Services/
│   │   └── OrderCheckoutService.cs     # Сервіс оформлення замовлення (компенсуючі транзакції)
│   └── Data/
│       └── CustomerRepository.cs       # Інтерфейс репозиторію
├── Testing.UnitTests/                  # Модульні тести на xUnit + NSubstitute + FluentAssertions
│   ├── BankAccountTests.cs             # [Fact], [Theory], [InlineData], [MemberData]
│   └── OrderCheckoutServiceTests.cs    # Mocking через NSubstitute (Received, DidNotReceive, Throw)
├── Testing.EFCore.InMemoryTests/       # Тестування EF Core з реальною реляційною базою
│   ├── Data/
│   │   └── TestShopDbContext.cs        # SQLite In-Memory контекст із Unique Constraints
│   ├── Repositories/
│   │   └── CustomerRepository.cs
│   └── CustomerRepositoryTests.cs     # IAsyncLifetime, SQLite In-Memory, генерація фікстур через Bogus
└── Docs/                               # Повний посібник українською
    ├── 01_xUnit_Fundamentals.md
    ├── 02_Mocking_With_NSubstitute.md
    └── 03_Testing_EFCore_And_Domain.md
```

---

## Навчальні матеріали
1. [Розділ 1: Фундамент тестування xUnit та FluentAssertions](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/08-Testing-And-Quality/01-Unit-Testing-xUnit-Mocking/Docs/01_xUnit_Fundamentals.md)
2. [Розділ 2: Ізоляція залежностей та мокінг з NSubstitute](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/08-Testing-And-Quality/01-Unit-Testing-xUnit-Mocking/Docs/02_Mocking_With_NSubstitute.md)
3. [Розділ 3: Тестування EF Core, SQLite In-Memory та Bogus фікстури](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/08-Testing-And-Quality/01-Unit-Testing-xUnit-Mocking/Docs/03_Testing_EFCore_And_Domain.md)

---

## Як запустити тести

Запуск усіх 21 тестів рішення:
```powershell
dotnet test UnitTestingAndMocking.slnx
```

Запуск тестів з детальним звітом:
```powershell
dotnet test --logger "console;verbosity=normal"
```
