using System;
using System.Collections;

namespace Csharp8Features
{
    public class ArrayListDemo
    {
        public static void Main()
        {
            // Додавання елементів до ArrayList за допомогою методу Add()
            ArrayList arrayList1 = new ArrayList();
            arrayList1.Add(101);       // Додавання цілого числа (int)
            arrayList1.Add("James");   // Додавання рядка (string)
            arrayList1.Add(true);      // Додавання логічного значення (bool)
            arrayList1.Add(4.5);       // Додавання числа з плаваючою крапкою (double)

            // Отримання окремих елементів з ArrayList за допомогою індексатора
            int firstElement = (int)arrayList1[0];        // повертає 101
            string secondElement = (string)arrayList1[1]; // повертає "James"
            //int secondElement = (int)arrayList1[1];     // Помилка: неможливо перетворити string у int

            Console.WriteLine($"First Element: {firstElement}, Second Element: {secondElement}");

            // Використання ключового слова var без явного приведення типів
            var firsItem = arrayList1[0];   // повертає 101 (тип визначається автоматично як object)
            var secondItem = arrayList1[1]; // повертає "James"
            //var fifthElement = arrayList1[5]; // Помилка: індекс поза межами масиву

            Console.WriteLine($"First Item: {firsItem}, Second Item: {secondItem}");

            // Оновлення елементів у колекції
            arrayList1[0] = "Smith"; // заміна 101 на "Smith"
            arrayList1[1] = 1010;    // заміна "James" на 1010
            //arrayList1[5] = 500;   // Помилка: індекс поза межами масиву

            // Виведення всіх елементів колекції
            foreach (var item in arrayList1)
            {
                Console.Write($"{item} ");
            }

            Console.ReadKey();
        }
    }
}
