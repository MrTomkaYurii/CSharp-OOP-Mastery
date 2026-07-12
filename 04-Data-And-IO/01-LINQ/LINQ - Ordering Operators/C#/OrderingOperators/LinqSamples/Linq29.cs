using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор OrderBy за властивістю (Селектор ключа)
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Сортування об'єктів або рядків за конкретною властивістю (наприклад, довжиною слова word.Length).
    //
    // Принцип роботи:
    //   - Функція keySelector (word => word.Length) витягує числовий ключ сортування для кожного елемента.
    //   - LINQ сортує елементи за отриманим числовим значенням.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 29: Сортування слів за їхньою довжиною (word.Length) за допомогою Query Syntax.
        /// </summary>
        public void Linq29()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region LINQ Query Syntax

            var sortedWords =
                from word in words
                orderby word.Length
                select word;

            Console.WriteLine("Query Syntax — Слова, відсортовані за довжиною:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine("{0} (довжина: {1})", w, w.Length);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 29A: Сортування слів за їхньою довжиною за допомогою Method Syntax.
        /// </summary>
        public void Linq29A()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region LINQ Method Syntax (Lambda)

            var sortedWords = words.OrderBy(word => word.Length);

            Console.WriteLine();
            Console.WriteLine("**********************************");
            Console.WriteLine("Lambda-вираз — Слова, відсортовані за довжиною:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine("{0} (довжина: {1})", w, w.Length);
            }

            #endregion
        }
    }
}
