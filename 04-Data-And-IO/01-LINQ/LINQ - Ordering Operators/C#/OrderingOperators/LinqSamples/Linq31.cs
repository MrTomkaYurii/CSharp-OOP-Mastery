using LinqSamplesCommon.LinqSamplesHelpers;
using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор OrderBy з користувацьким компаратором (IComparer<T>)
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Сортування елементів із застосуванням власної логіки порівняння (наприклад,
    //              без урахування регістру символів Case-Insensitive).
    //
    // Принцип роботи:
    //   - Метод приймає реалізацію інтерфейсу System.Collections.Generic.IComparer<TKey>.
    //   - Замість стандартного двійкового або культурно-залежного порівняння рядків викликається метод
    //     IComparer.Compare(x, y).
    //
    // Важлива особливість:
    //   - Передача користувацького компаратора безпосередньо підтримується ТІЛЬКИ в Method Syntax.
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 31: Сортування слів без урахування регістру літер за допомогою OrderBy та власного CaseInsensitiveComparer.
        /// </summary>
        public void Linq31()
        {
            // Масив слів із різним регістром літер
            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            #region LINQ Method Syntax (OrderBy з IComparer)

            // Сортування без урахування регістру
            var sortedWords = words.OrderBy(a => a, new CaseInsensitiveComparer());

            Console.WriteLine("Слова, відсортовані без урахування регістру (Case-insensitive):");
            ObjectDumper.Write(sortedWords);

            #endregion
        }
    }
}
