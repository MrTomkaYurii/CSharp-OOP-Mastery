using System;
namespace ArayDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Створення та ініціалізація масиву цілих чисел
            // Розмір масиву — 10
            int[] Numbers = { 17, 23, 4, 59, 27, 36, 96, 9, 1, 3 };

            // Виведення елементів масиву за допомогою циклу for
            Console.WriteLine("Початкові елементи масиву:");
            for (int i = 0; i < Numbers.Length; i++)
            {
                Console.Write(Numbers[i] + " ");
            }
            Console.WriteLine();

            // Сортування елементів масиву за допомогою методу Sort класу Array
            Array.Sort(Numbers);
            // Виведення елементів масиву після сортування за допомогою циклу foreach
            Console.WriteLine("\nЕлементи масиву після сортування:");
            foreach (int i in Numbers)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            // Реверс (перевернення) елементів масиву за допомогою методу Reverse класу Array
            Array.Reverse(Numbers);
            // Виведення елементів масиву у зворотному порядку
            Console.WriteLine("\nЕлементи масиву у зворотному порядку:");
            foreach (int i in Numbers)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            // Створення нового масиву
            int[] NewNumbers = new int[10];
            // Копіювання деяких елементів зі старого масиву в новий
            // Оголошено масив розміром 10, але копіюємо лише 5 елементів.
            // Тому решта 5 елементів матимуть значення за замовчуванням (для int це 0)
            Array.Copy(Numbers, NewNumbers, 5);

            // Виведення елементів нового масиву за допомогою foreach
            Console.WriteLine("\nЕлементи нового масиву:");
            foreach (int i in NewNumbers)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine();
            Console.WriteLine($"\nДовжина нового масиву (властивість Length): {NewNumbers.Length}");
            Console.WriteLine($"Довжина нового масиву (метод GetLength): {NewNumbers.GetLength(0)}");
            Console.ReadKey();
        }
    }
}
