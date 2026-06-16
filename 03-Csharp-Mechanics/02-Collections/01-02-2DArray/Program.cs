using System;
namespace TwoDimensionalArayDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Увімкнення підтримки української мови
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Введіть кількість рядків та стовпців матриць: ");
            int Rows = Convert.ToInt32(Console.ReadLine());
            int Columns = Convert.ToInt32(Console.ReadLine());

            // Створюємо три 2D масиви заданого розміру
            int[,] Matrix1 = new int[Rows, Columns];
            int[,] Matrix2 = new int[Rows, Columns];
            int[,] ResultMatrix = new int[Rows, Columns];

            // Введення елементів першої матриці
            Console.WriteLine("\nВведіть елементи 1-ї матриці:");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Matrix1[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            // Введення елементів другої матриці
            Console.WriteLine("\nВведіть елементи 2-ї матриці:");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Matrix2[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }

            // Додавання обох матриць і запис результату в ResultMatrix
            Console.WriteLine("\nСума обох матриць:");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    ResultMatrix[i, j] = Matrix1[i, j] + Matrix2[i, j];
                    Console.Write($"{ResultMatrix[i, j]} ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
