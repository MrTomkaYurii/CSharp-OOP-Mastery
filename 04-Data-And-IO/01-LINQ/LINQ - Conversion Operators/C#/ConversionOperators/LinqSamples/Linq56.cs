using System;
using System.Collections.Generic;
using System.Linq;

namespace ConversionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор ToDictionary (Матеріалізація у хеш-таблицю Dictionary<TKey, TValue>)
    // ==============================================================================================
    // Категорія: Conversion Operators (Оператори перетворення типів / Матеріалізації)
    // Призначення: Перетворює послідовність на словник для надшвидкого доступу до записів за ключем (O(1)).
    //
    // Принцип роботи:
    //   - Приймає функцію keySelector (наприклад, sr => sr.Name).
    //   - Опціонально може приймати elementSelector для вибору значення (за замовчуванням зберігає весь об'єкт).
    //   - УВАГА: Усі ключі, згенеровані keySelector, мають бути УНІКАЛЬНИМИ!
    //     Якщо зустрінеться дублікат ключа, метод викине виняток ArgumentException.
    //     (Якщо дублікати можливі, слід використовувати ToLookup() замість ToDictionary()).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesConversionOperators
    {
        /// <summary>
        /// Приклад 56: Перетворення списку оцінок студентів на словник Dictionary з ключем за іменем (sr.Name).
        /// </summary>
        public void Linq56()
        {
            var scoreRecords = new[]
            {
                new { Name = "Alice", Score = 50 },
                new { Name = "Bob",   Score = 40 },
                new { Name = "Cathy", Score = 45 }
            };

            #region LINQ Method Syntax (ToDictionary)

            // Створюємо словник Dictionary<string, TAnon>, де ключем є ім'я студента
            var scoreRecordsDict = scoreRecords.ToDictionary(sr => sr.Name);

            // Швидкий доступ O(1) за ключем "Bob"
            Console.WriteLine("Оцінка Боба (через ключ у словнику): {0}", scoreRecordsDict["Bob"].Score);

            #endregion
        }
    }
}
