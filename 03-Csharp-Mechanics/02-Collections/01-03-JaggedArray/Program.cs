using System;
namespace TwoDimensionalArayDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створення зубчастого масиву (jagged array) з чотирма рядками
            int[][] arr = new int[4][];

            // Ініціалізація кожного рядка різною кількістю стовпців
            // Використовуються одновимірні масиви
            arr[0] = new int[5];
            arr[1] = new int[6];
            arr[2] = new int[4];
            arr[3] = new int[5];

            // Виведення значень зубчастого масиву за допомогою вкладених циклів for
            // Будуть виведені значення за замовчуванням (0), бо ми ще не задавали значення
            // GetLength(0): повертає кількість рядків (4)
            Console.WriteLine("Виведення значень зубчастого масиву за замовчуванням:");
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                // arr[i].Length: повертає довжину конкретного рядка
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.Write(arr[i][j] + " ");
                }
            }

            // Присвоєння значень елементам зубчастого масиву
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                int num = 10;
                for (int j = 0; j < arr[i].Length; j++)
                {
                    num++;
                    arr[i][j] = num;
                }
            }

            // Виведення значень зубчастого масиву за допомогою foreach всередині for
            Console.WriteLine("\n\nВиведення значень зубчастого масиву після присвоєння:");
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                foreach (int x in arr[i])
                {
                    Console.Write(x + " ");
                }
            }

            // Неможливо просто перебирати arr foreach-ем і друкувати значення,
            // оскільки arr містить масиви, а не окремі числа.
            //foreach (var item in arr)
            //{
            //    Console.Write(item); // item — це масив, а не елемент масиву
            //}

            Console.ReadKey();
        }
    }
}
