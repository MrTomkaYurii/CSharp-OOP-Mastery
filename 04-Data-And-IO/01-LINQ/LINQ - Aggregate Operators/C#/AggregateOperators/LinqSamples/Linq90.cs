using System;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Average з селектором
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Перетворює кожен елемент послідовності на число та обчислює їхнє середнє арифметичне.
    //
    // Принцип роботи:
    //   - Обчислює значення функції для кожного елемента (наприклад, довжину рядка w.Length).
    //   - Знаходить середнє арифметичне отриманих значень.
    //   - Результат повертається у вигляді double (або decimal/float залежно від типу поля).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 90: Обчислення середньої довжини слів у масиві за допомогою Average(w => w.Length).
        /// </summary>
        public void Linq90()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region Make Sure to try yourself before looking at the code

            // Обчислюємо середню довжину слова
            double averageLength = words.Average(w => w.Length);

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("The average word length is {0} characters.", averageLength);
        }
    }
}
