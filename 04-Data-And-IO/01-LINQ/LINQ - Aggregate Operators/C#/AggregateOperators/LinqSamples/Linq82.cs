using System;
using System.ComponentModel;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Min з селектором
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Знаходить мінімальне значення проектованої властивості елементів (наприклад, довжини рядка).
    //
    // Принцип роботи:
    //   - Застосовує селектор до кожного елемента та повертає мінімум із отриманих результатів.
    //   - Зверніть увагу: Min(w => w.Length) повертає числове значення мінімальної довжини (int), а НЕ сам об'єкт рядка.
    //   - У сучасному .NET 6+ для пошуку самого об'єкта з мінімальною властивістю використовується метод MinBy(keySelector).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        [Category("Aggregate Operators")]
        [Description("This sample uses Min to get the length of the shortest word in an array.")]
        /// <summary>
        /// Приклад 82: Знаходження довжини найкоротшого слова в масиві за допомогою Min(w => w.Length).
        /// </summary>
        public void Linq82()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region Make Sure to try yourself before looking at the code

            // Знаходимо мінімальну довжину серед слів (довжина слова "apple" - 5)
            int shortestWord = words.Min(w => w.Length);

            #endregion

            Console.WriteLine("The shortest word is {0} characters long.", shortestWord);
        }
    }
}
