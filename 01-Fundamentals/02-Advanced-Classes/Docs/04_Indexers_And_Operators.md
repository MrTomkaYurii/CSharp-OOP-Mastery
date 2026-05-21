# Лекція 4: Індексатори та Перевантаження Операторів

## 1. Індексатори (`Indexers`)

**Індексатор** — це параметризована властивість, яка дозволяє звертатися до елементів об'єкта за допомогою квадратних дужок `[]`, немов це звичайний масив або асоціативний словник:

```csharp
public class SmartBuffer
{
    private readonly string[] _storage = new string[10];

    // Індексатор за числовим ключем:
    public string this[int index]
    {
        get => _storage[index];
        set => _storage[index] = value;
    }

    // Перевантажений індексатор за рядковим ключем:
    public string this[string key]
    {
        get => FindByKey(key);
        set => SetByKey(key, value);
    }
}
```

---

## 2. Перевантаження операторів (Operator Overloading)

C# дозволяє надавати стандартним операторам (`+`, `-`, `*`, `==`, `!=`) нову поведінку для власних типів.

### Правила перевантаження:
1. Метод оператора **завжди** має бути `public static`.
2. Оператори порівняння перевантажуються **виключно парами**:
   - `==` та `!=`
   - `<` та `>`
   - `<=` та `>=`
3. Якщо ви перевантажуєте `==` та `!=`, ви **зобов'язані** перевизначити `Equals()` та `GetHashCode()`.

```csharp
public readonly struct Money(decimal amount, string currency) : IEquatable<Money>
{
    public decimal Amount { get; } = amount;
    public string Currency { get; } = currency;

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Неможливо складати кошти з різними валютами!");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static bool operator ==(Money a, Money b) => a.Equals(b);
    public static bool operator !=(Money a, Money b) => !a.Equals(b);

    public bool Equals(Money other) => Amount == other.Amount && Currency == other.Currency;
    public override bool Equals(object? obj) => obj is Money other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
}
```

---

## 3. Оператори приведення типів (`implicit` та `explicit`)

* **`implicit` (неявне перетворення):** використовується тоді, коли перетворення є безпечним, детермінованим і ніколи не призводить до втрати точності чи виникнення винятку.
* **`explicit` (явне перетворення):** вимагає явного приведення `(Type)val`, якщо можлива втрата даних або помилка.
