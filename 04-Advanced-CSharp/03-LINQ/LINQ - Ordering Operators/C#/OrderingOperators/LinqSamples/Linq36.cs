using LinqSamplesCommon.LinqSamplesHelpers;
using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор ThenBy з користувацьким компаратором
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Вторинне сортування за зростанням із застосуванням спеціального компаратора
    //              (наприклад, без урахування регістру літер CaseInsensitiveComparer).
    //
    // Принцип роботи:
    //   - Первинне сортування: OrderBy(a => a.Length) сортує слова за довжиною.
    //   - Вторинне сортування: ThenBy(a => a, comparer) всередині слів однакової довжини
    //     впорядковує їх за абеткою без урахування регістру символів.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 36: Сортування слів спочатку за довжиною, а потім за абеткою без урахування регістру
        /// за допомогою методів OrderBy та ThenBy із передачею CaseInsensitiveComparer.
        /// </summary>
        public void Linq36()
        {
            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            #region LINQ Method Syntax (OrderBy + ThenBy з компаратором)

            var sortedWords =
                words.OrderBy(a => a.Length)
                     .ThenBy(a => a, new CaseInsensitiveComparer());

            Console.WriteLine("Слова: за довжиною, потім без урахування регістру:");
            ObjectDumper.Write(sortedWords);

            #endregion
        }
    }
}
