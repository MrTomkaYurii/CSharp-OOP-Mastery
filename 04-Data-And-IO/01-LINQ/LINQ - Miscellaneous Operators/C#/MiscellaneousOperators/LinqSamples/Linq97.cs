using System;
using System.Linq;

namespace MiscellaneousOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Чутливість SequenceEqual до порядку елементів
    // ==============================================================================================
    // Категорія: Miscellaneous Operators (Різні допоміжні оператори)
    // Призначення: Демонструє, що SequenceEqual вимагає точного збігу не лише значень, але й порядку слідування.
    //
    // Принцип роботи:
    //   - Якщо дві послідовності містять однакові елементи, але в іншому порядку (наприклад, анаграми або невідсортовані списки),
    //     SequenceEqual повертає false.
    //   - Для перевірки рівності невпорядкованих мультимножин використовуються інші підходи
    //     (наприклад, сортування перед порівнянням OrderBy або групування за значеннями).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesMiscellaneousOperators
    {
        /// <summary>
        /// Приклад 97: Порівняння двох масивів з однаковими елементами в різному порядку за допомогою SequenceEqual (результат false).
        /// </summary>
        public void Linq97()
        {
            var wordsA = new string[] { "cherry", "apple", "blueberry" };
            var wordsB = new string[] { "apple", "blueberry", "cherry" };

            #region Make Sure to try yourself before looking at the code

            // Елементи однакові, але порядок слідування відрізняється -> false
            bool match = wordsA.SequenceEqual(wordsB);

            #endregion

            Console.WriteLine("The sequences match: {0}", match);
        }
    }
}
