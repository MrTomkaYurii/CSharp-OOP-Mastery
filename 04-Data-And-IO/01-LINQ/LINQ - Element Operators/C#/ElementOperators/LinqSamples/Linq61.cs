using System;
using System.Linq;

namespace ElementOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор FirstOrDefault (Безпечне отримання першого елемента)
    // ==============================================================================================
    // Категорія: Element Operators (Оператори елементів)
    // Призначення: Повертає перший елемент послідовності, або значення за замовчуванням (default(T)),
    //              якщо послідовність порожня або жоден елемент не відповідає умові.
    //
    // Принцип роботи:
    //   - На відміну від First(), НЕ генерує виняток при відсутності збігів або порожній колекції.
    //   - Для посилальних типів (reference types, class) повертає null.
    //   - Для структур і примітивних типів (value types) повертає дефолтне значення (0 для int, false для bool тощо).
    //   - Починаючи з C# 10 / .NET 6, підтримує перевантаження FirstOrDefault(defaultValue) для явного значення.
    //   - Критично важливий при емуляції Left Outer Join у LINQ.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesElementOperators
    {
        /// <summary>
        /// Приклад 61: Використання FirstOrDefault для безпечного отримання елемента з порожнього масиву чисел.
        /// </summary>
        public void Linq61()
        {
            int[] numbers = { };

            #region Make Sure to try yourself before looking at the code

            // Для порожнього масиву int[] FirstOrDefault повертає default(int), тобто 0
            int firstNumOrDefault = numbers.FirstOrDefault();

            Console.WriteLine(firstNumOrDefault);

            #endregion
        }
    }
}
