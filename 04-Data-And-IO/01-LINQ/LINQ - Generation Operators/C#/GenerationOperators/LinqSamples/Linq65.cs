using System;
using System.Linq;

namespace GenerationOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Статичний метод Enumerable.Range (Генерація числового діапазону)
    // ==============================================================================================
    // Категорія: Generation Operators (Оператори генерації послідовностей)
    // Призначення: Генерує послідовність цілих чисел (int) у заданому інтервалі [start, start + count - 1].
    //
    // Принцип роботи:
    //   - Приймає два параметри: start (початкове число) та count (кількість чисел, що генеруються).
    //   - Працює ліниво (on-demand): генерує числа по одному за допомогою yield return без виділення пам'яті під весь масив.
    //   - Викидає ArgumentOutOfRangeException, якщо count < 0 або діапазон виходить за межі int.MaxValue.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesGenerationOperators
    {
        /// <summary>
        /// Приклад 65: Генерація чисел від 100 до 149 за допомогою Range та класифікація на парні/непарні (Query Syntax).
        /// </summary>
        public void Linq65()
        {
            // Генеруємо 50 чисел починаючи зі 100 (100..149) та проектуємо у новий анонімний тип
            var numbers =
                from n in Enumerable.Range(100, 50)
                select new { Number = n, OddEven = n % 2 == 1 ? "odd" : "even" };

            // Виводимо перші 10 згенерованих елементів
            foreach (var n in numbers.Take(10))
            {
                Console.WriteLine("The number {0} is {1}.", n.Number, n.OddEven);
            }
        }

        /// <summary>
        /// Приклад 65A: Генерація послідовності та проекція за допомогою Enumerable.Range(...).Select(...) (Method Syntax).
        /// </summary>
        public void Linq65A()
        {
            #region Linq with Lambda - Make Sure to try yourself before looking at the code

            // Функціональний стиль виклику через Select
            var numbers = Enumerable.Range(100, 50)
                .Select(n => new { Number = n, OddEven = n % 2 == 1 ? "odd" : "even" });

            #endregion

            Console.WriteLine();
            Console.WriteLine("***********************");
            Console.WriteLine("Linq with Lambda.");
            foreach (var n in numbers.Take(10))
            {
                Console.WriteLine("The number {0} is {1}.", n.Number, n.OddEven);
            }
        }
    }
}
