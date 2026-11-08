using System;
using System.Linq;

namespace QueryExecution
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Повторне виконання запиту та зміни даних (Query Re-evaluation)
    // ==============================================================================================
    // Категорія: Query Execution (Механізми виконання запитів)
    // Призначення: Демонструє, що відкладений запит не кешує результати, а щоразу звертається
    //              до актуального стану базового джерела даних.
    //
    // Принцип роботи:
    //   - Якщо базовий масив або колекція зазнає мутацій (зміни значень), наступна ітерація
    //     того ж самого об'єкта запиту автоматично врахує всі нові дані.
    //   - Запити можна компонувати один на одному (Query Chaining): запит lowEvenNumbers
    //     побудований поверх запиту lowNumbers і виконується разом із ним без проміжних масивів.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution) при кожному проході.
    // ==============================================================================================

    public partial class LinqSamplesQueryExecution
    {
        /// <summary>
        /// Приклад 101: Багаторазове використання запиту після мутації вхідного масиву чисел.
        /// </summary>
        public void Linq101()
        {
            // Оголошуємо початковий масив
            int[] numbers = new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            // Оголошуємо базовий запит (числа <= 3)
            var lowNumbers =
                from num in numbers
                where num <= 3
                select num;

            Console.WriteLine("First run numbers <= 3:");
            foreach (int n in lowNumbers)
            {
                Console.WriteLine(n);
            }

            // Побудова запиту поверх іншого запиту (композиція запитів)
            var lowEvenNumbers =
                from num in lowNumbers
                where num % 2 == 0
                select num;

            Console.WriteLine("Run lowEvenNumbers query:");
            foreach (int n in lowEvenNumbers)
            {
                Console.WriteLine(n);
            }

            // Мутуємо вихідні дані: робимо всі числа від'ємними
            for (int i = 0; i < 10; i++)
            {
                numbers[i] = -numbers[i];
            }

            // Другий пропуск того самого об'єкта lowNumbers:
            // Оскільки виконання ліниве, запит читає вже оновлений стан масиву numbers[]:
            Console.WriteLine("Second run numbers <= 3:");
            foreach (int n in lowNumbers)
            {
                Console.WriteLine(n);
            }
        }
    }
}
