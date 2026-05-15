// params-параметру можна передати і готовий масив одним аргументом.
// У цьому випадку новий масив НЕ створюється — метод отримує посилання на myArr,
// тому зміни елементів усередині методу видно і в Main (порівняйте з p110).

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
        int[] myArr = new int[] { 5, 6, 7 };      // Створення та ініціалізація масиву.
        MyClass mc = new MyClass();
        mc.ListInts(myArr);                     // Передаємо весь масив як один аргумент.

        // Виведе 50, 60, 70 — метод змінив елементи того самого масиву.
        foreach (int x in myArr)
            Console.WriteLine($"{x}");         // Виведення кожного елемента.
    }
}