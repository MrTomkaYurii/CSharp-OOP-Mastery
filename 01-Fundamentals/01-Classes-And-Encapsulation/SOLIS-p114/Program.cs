// ref return: метод повертає ПОСИЛАННЯ на своє приватне поле, а не його копію.
// Зовнішній код через ref-змінну може напряму змінювати це поле.
// Це потужно, але порушує інкапсуляцію — використовувати обережно.

class Simple
{
    private int Score = 5;

    public ref int RefToValue()
    {
        return ref Score;   // повертаємо посилання на поле Score
    }

    public void Display()
    {
        Console.WriteLine($"Value inside class object: {Score}");
    }
}
class Program
{
    static void Main()
    {
        Simple s = new Simple();
        s.Display();                                // 5
        ref int v1Outside = ref s.RefToValue();     // v1Outside — псевдонім для s.Score

        v1Outside = 10;   // Пишемо у v1Outside — фактично змінюємо приватне поле s.Score.
        s.Display();      // 10 — поле справді змінилося ззовні.
    }
}