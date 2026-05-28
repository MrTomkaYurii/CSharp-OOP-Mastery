using System;

/*
 * ── Ключове слово `base` (Solis, с. 181) ─────────────────────────────
 *
 * Коли похідний клас ховає успадкований член, доступ до "затуленого"
 * члена базового класу дає префікс `base.`.
 *
 * Таким чином усередині похідного класу доступні обидві версії:
 *   Field1       → член похідного класу
 *   base.Field1  → член базового класу
 *
 * Очікуваний вивід:
 *   Field1 -- In the derived class
 *   Field1 -- In the base class
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class SomeClass
{
    public string Field1 = "Field1 -- In the base class";
}

// ── Похідний клас ────────────────────────────────────────────────────
class OtherClass : SomeClass
{
    public new string Field1 = "Field1 -- In the derived class";   // ховає SomeClass.Field1

    public void PrintField1()
    {
        Console.WriteLine(Field1);        // власне поле похідного класу
        Console.WriteLine(base.Field1);   // приховане поле базового класу
    }
}

class Program
{
    static void Main()
    {
        OtherClass oc = new OtherClass();
        oc.PrintField1();
    }
}
