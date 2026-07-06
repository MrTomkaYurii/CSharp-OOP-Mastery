# Користувацькі виключення (Custom Exceptions) та Throw Expressions

## Вступ
Незважаючи на багату стандартну бібліотеку виключень .NET (`ArgumentException`, `InvalidOperationException`, `TimeoutException`), для специфічних бізнес-правил доменної моделі рекомендується створювати власні типізовані виключення.

---

## 1. Канонічна структура користувацького виключення

За угодами платформи .NET власний клас виключення:
1. Повинен мати суфікс `Exception` у назві (наприклад, `TemperatureOutOfRangeException`).
2. Повинен успадковуватися від базового класу `System.Exception` (або `ApplicationException` в окремих доменах).
3. Повинен реалізовувати **три стандартні конструктори**:

```csharp
public class TemperatureIsZeroException : Exception
{
    // 1. Конструктор за замовчуванням
    public TemperatureIsZeroException() 
        : base("Температура не може дорівнювати нулю!") 
    { }

    // 2. Конструктор з користувацьким повідомленням
    public TemperatureIsZeroException(string message) 
        : base(message) 
    { }

    // 3. Конструктор з повідомленням та вкладеним виключенням
    public TemperatureIsZeroException(string message, Exception innerException) 
        : base(message, innerException) 
    { }

    // Додаткові доменні властивості для деталізації контексту помилки
    public double RecordedTemperature { get; init; }
    public DateTime MeasurementTime { get; init; } = DateTime.UtcNow;
}
```

---

## 2. Вирази викидання виключень (Throw Expressions)

Починаючи з C# 7.0, оператор `throw` може використовуватися як вираз (expression), а не лише як інструкція (statement). Це дозволяє компактно валідувати вхідні дані в один рядок.

```csharp
public class User
{
    private string _name;

    // Валідація у тернарному операторі
    public User(string? name, int age)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        Age = age >= 18 ? age : throw new ArgumentOutOfRangeException(nameof(age), "Вік має бути >= 18");
    }

    // Валідація у виразному геттері
    public string Name => _name ?? throw new InvalidOperationException("Ім'я не ініціалізоване");
}
```

---

## 3. Проекти для практики

- [example-user-defined-exception](file:///03-Csharp-Mechanics/05-Exceptions-Handling/example-user-defined-exception/Program.cs) — Створення та обробка власного класу виключення `TempIsZeroException`.
- [p679[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p679[SOLIS]/Program.cs) — Використання `throw` виразів умовного вибору в тернарному операторі `? :`.
