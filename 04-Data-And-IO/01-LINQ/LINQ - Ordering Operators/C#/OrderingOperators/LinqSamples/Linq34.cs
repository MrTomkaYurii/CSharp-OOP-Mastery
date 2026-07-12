using LinqSamplesCommon.LinqSamplesHelpers;
using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор OrderByDescending з користувацьким компаратором
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Сортування елементів за спаданням із застосуванням власної логіки порівняння
    //              (наприклад, без урахування регістру літер CaseInsensitiveComparer).
    //
    // Принцип роботи:
    //   - Метод OrderByDescending приймає селектор ключа та екземпляр IComparer<TKey>.
    //   - Дозволяє реалізувати нетривіальні правила спадного сортування для рядків чи бізнес-об'єктів.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 34: Сортування слів за спаданням без урахування регістру літер (Method Syntax).
        /// </summary>
        public void Linq34()
        {
            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            #region LINQ Method Syntax (OrderByDescending з IComparer)

            var sortedWords = words.OrderByDescending(a => a, new CaseInsensitiveComparer());

            Console.WriteLine("Слова, відсортовані за спаданням без урахування регістру:");
            ObjectDumper.Write(sortedWords);

            #endregion
        }
    }
}
