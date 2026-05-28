using System;

/*
 * ── Приховування (маскування) членів базового класу (Solis, с. 180) ───
 *
 * Похідний клас може оголосити член з тим самим іменем, що й у базовому.
 * Модифікатор `new` явно повідомляє компілятору: "я навмисно ховаю
 * успадкований член". Без `new` код теж скомпілюється, але з попередженням.
 *
 * Приховування — це НЕ поліморфізм: який саме член викличеться,
 * визначає ТИП ЗМІННОЇ, а не реальний тип об'єкта (див. приклад на с. 183).
 *
 * Очікуваний вивід:
 *   OtherClass.Method1: OtherClass Field1
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class SomeClass
{
    public string Field1 = "SomeClass Field1";

    public void Method1(string value)
    {
        Console.WriteLine($"SomeClass.Method1: {value}");
    }
}

// ── Похідний клас ────────────────────────────────────────────────────
class OtherClass : SomeClass
{
    public new string Field1 = "OtherClass Field1";   // ховає SomeClass.Field1

    public new void Method1(string value)             // ховає SomeClass.Method1
    {
        Console.WriteLine($"OtherClass.Method1: {value}");
    }
}

class Program
{
    static void Main()
    {
        OtherClass oc = new OtherClass();

        // Тип змінної — OtherClass, тому звертаємося до маскувальних членів.
        oc.Method1(oc.Field1);
    }
}
