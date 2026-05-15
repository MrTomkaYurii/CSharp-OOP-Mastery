// Модифікатор params дозволяє передавати змінну кількість аргументів,
// які метод отримує як один масив.
// Тут аргументи — окремі змінні (first, second, third): компілятор сам
// створює з них тимчасовий масив, тому зміни всередині методу НЕ впливають
// на first/second/third у Main (порівняйте з p112).

class MyClass
{
    public void ListInts(params int[] inVals)
    {
        if ((inVals != null) && (inVals.Length != 0))
            for (int i = 0; i < inVals.Length; i++) // Обробка масиву.
            {
                inVals[i] = inVals[i] * 10;
                Console.WriteLine($"{inVals[i]}"); // Виведення нового значення.
            }
    }
}

class Program
{
    static void Main()
    {
        int first = 5, second = 6, third = 7;        // Оголошення трьох змінних int.

        MyClass mc = new MyClass();
        mc.ListInts(first, second, third);         // Виклик методу.

        // Виведе 5, 6, 7 — вихідні змінні не змінилися (метод працював із копією в масиві).
        Console.WriteLine($"{first}, {second}, {third}");
    }
}