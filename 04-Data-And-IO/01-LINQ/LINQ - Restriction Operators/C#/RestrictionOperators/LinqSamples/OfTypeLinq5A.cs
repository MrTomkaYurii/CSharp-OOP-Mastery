using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RestrictionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор OfType<T> (Фільтрація за типом)
    // ==============================================================================================
    // Категорія: Restriction Operators / Filtering Operators (Оператори фільтрації)
    // Призначення: Фільтрує елементи послідовності на основі їхньої здатності бути зведеними
    //              до вказаного типу TResult.
    //
    // Принцип роботи:
    //   - Застосовується як до узагальнених (generic), так і до неузагальнених колекцій (System.Collections.IEnumerable,
    //     наприклад ArrayList).
    //   - Елементи, які не є екземплярами типу TResult або не можуть бути приведені до нього, ігноруються
    //     (без викидання InvalidCastException, на відміну від Enumerable.Cast<T>).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesRestrictionOperators
    {
        /// <summary>
        /// Приклад 5A: Використання OfType<string>() для фільтрації елементів заданого типу з нетипізованого ArrayList,
        /// а також комбінування з наступним оператором Where().
        /// </summary>
        public void OfTypeLinq5A()
        {
            // Вхідні дані: гетерогенний (змішаний) список, що містить рядки та числа (3.0)
            ArrayList fruits = new ArrayList(5) { "Mango", "Orange", "Apple", 3.0, "Banana" };

            #region Застосування OfType<string>()

            // 1. Відбираємо тільки елементи типу string. Число 3.0 буде відсіяне без винятків.
            IEnumerable<string> query1 = fruits.OfType<string>();

            Console.WriteLine("Елементи типу 'string' у колекції:");
            foreach (string fruit in query1)
            {
                Console.WriteLine(fruit);
            }

            // 2. Демонстрація поєднання OfType() зі стандартним оператором Where()
            // Знаходимо всі рядки, які містять літеру 'n'
            IEnumerable<string> query2 =
                fruits.OfType<string>().Where(fruit => fruit.ToLower().Contains("n"));

            Console.WriteLine("\nРядки, що містять літеру 'n':");
            foreach (string fruit in query2)
            {
                Console.WriteLine(fruit);
            }

            #endregion
        }
    }
}
