using System;
using System.Collections.Generic;

namespace SystemObjectAndEquality;

/*
 * ── 01. Корінь ієрархії System.Object та семантика рівності ──────────────
 *
 * У .NET кожен клас (навіть якщо після його імені не вказано `: BaseClass`)
 * неявно успадковує базовий клас `System.Object` (`object`).
 *
 * Від `System.Object` кожен об'єкт отримує:
 *   • Віртуальні методи:
 *       1. `virtual string ToString()` — текстове представлення об'єкта;
 *       2. `virtual bool Equals(object? obj)` — перевірка рівності;
 *       3. `virtual int GetHashCode()` — числовий хеш-код для HashSet/Dictionary;
 *   • Невіртуальні та захищені методи:
 *       4. `Type GetType()` — отримання метаданих реального типу під час виконання;
 *       5. `protected object MemberwiseClone()` — поверхневе копіювання полів;
 *       6. `static bool ReferenceEquals(object? a, object? b)` — порівняння адрес у купі.
 */

public sealed class BankNote : IEquatable<BankNote>
{
    public string SerialNumber { get; }
    public string Currency { get; }
    public int Denomination { get; }

    public BankNote(string serialNumber, string currency, int denomination)
    {
        SerialNumber = serialNumber.ToUpperInvariant();
        Currency = currency.ToUpperInvariant();
        Denomination = denomination;
    }

    // 1. Перевизначення ToString() замість дефолтного імені типу
    public override string ToString()
        => $"[{Currency} {Denomination} | Серія: {SerialNumber}]";

    // 2. Типізоване порівняння за значенням (IEquatable<BankNote>)
    public bool Equals(BankNote? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return SerialNumber == other.SerialNumber
            && Currency == other.Currency
            && Denomination == other.Denomination;
    }

    // 3. Перевизначення віртуального Object.Equals(object?)
    public override bool Equals(object? obj)
        => obj is BankNote other && Equals(other);

    // 4. Обов'язкове правило: якщо Equals повертає true, GetHashCode МАЄ бути однаковим!
    public override int GetHashCode()
        => HashCode.Combine(SerialNumber, Currency, Denomination);

    // 5. Демонстрація захищеного методу Object.MemberwiseClone()
    public BankNote ShallowCopy()
        => (BankNote)this.MemberwiseClone();
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== 1. System.Object: ToString, Equals, GetHashCode ===");

        var note1 = new BankNote("AA1020304", "UAH", 1000);
        var note2 = new BankNote("AA1020304", "UAH", 1000);

        Console.WriteLine($"  note1.ToString()          : {note1}");
        Console.WriteLine($"  note1.GetType().FullName  : {note1.GetType().FullName}");
        Console.WriteLine($"  ReferenceEquals(n1, n2)   : {ReferenceEquals(note1, note2)} (різні об'єкти в купі)");
        Console.WriteLine($"  note1.Equals(note2)       : {note1.Equals(note2)} (однакові за вмістом)");
        Console.WriteLine($"  HashCode note1 vs note2   : {note1.GetHashCode()} == {note2.GetHashCode()}");

        Console.WriteLine("\n=== 2. Робота у HashSet<BankNote> завдяки GetHashCode + Equals ===");
        var vault = new HashSet<BankNote> { note1 };
        bool addedDuplicate = vault.Add(note2);
        Console.WriteLine($"  Чи додався дублікат note2 у HashSet? {addedDuplicate} (Кількість у сховищі: {vault.Count})");

        Console.WriteLine("\n=== 3. Поверхневе копіювання через protected MemberwiseClone() ===");
        BankNote clone = note1.ShallowCopy();
        Console.WriteLine($"  Клон: {clone}, ReferenceEquals(note1, clone) = {ReferenceEquals(note1, clone)}");
    }
}
