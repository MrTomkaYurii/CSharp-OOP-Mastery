using LinqSamplesCommon.LinqSamplesHelpers;
using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор ThenByDescending з користувацьким компаратором
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Вторинне сортування за спаданням із застосуванням спеціального компаратора
    //              (наприклад, без урахування регістру символів CaseInsensitiveComparer).
    //
    // Принцип роботи:
    //   - Первинне сортування: OrderBy(a => a.Length) сортує слова за зростанням довжини.
    //   - Вторинне сортування: ThenByDescending(a => a, comparer) для слів однакової довжини
    //     виконує зворотне алфавітне сортування (Я-А) без урахування регістру літер.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 38: Сортування слів спочатку за довжиною (зростання), а потім за спаданням
        /// без урахування регістру літер (Method Syntax).
        /// </summary>
        public void Linq38()
        {
            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            #region LINQ Method Syntax (OrderBy + ThenByDescending з IComparer)

            var sortedWords =
                words.OrderBy(a => a.Length)
                     .ThenByDescending(a => a, new CaseInsensitiveComparer());

            Console.WriteLine("Слова: за довжиною (зростання), потім спадання без урахування регістру:");
            ObjectDumper.Write(sortedWords);

            #endregion
        }
    }
}
