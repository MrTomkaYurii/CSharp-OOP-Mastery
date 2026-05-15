using System;

// Рекурсія: метод викликає сам себе.
// if (inVal == 0) return; — це базовий випадок, який зупиняє рекурсію
// (без нього був би нескінченний виклик і StackOverflowException).
// WriteLine стоїть ПІСЛЯ рекурсивного виклику, тому числа друкуються
// при "розкручуванні" стека: 1, 2, 3.

class Program
{
    public void Count(int inVal)
    {
        if (inVal == 0)
            return;                    // базовий випадок — вихід із рекурсії
        Count(inVal - 1);              // спершу занурюємося до 0...

        Console.WriteLine($"{inVal}"); // ...а друкуємо вже на зворотному шляху
    }

    static void Main()
    {
        Program pr = new Program();
        pr.Count(3);
    }
}