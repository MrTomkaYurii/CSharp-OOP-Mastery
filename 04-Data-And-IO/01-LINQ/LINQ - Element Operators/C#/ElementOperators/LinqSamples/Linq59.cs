using System;
using System.Linq;

namespace ElementOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор First з предикатом
    // ==============================================================================================
    // Категорія: Element Operators (Оператори елементів)
    // Призначення: Знаходить перший елемент у послідовності, що задовольняє вказану умову (предикат).
    //
    // Принцип роботи:
    //   - Об'єднує операцію фільтрації та вибору першого елемента: .First(predicate) еквівалентно .Where(predicate).First().
    //   - Зупиняє ітерацію одразу після першого збігу (коротке замикання / short-circuiting).
    //   - Якщо збігів немає — генерує System.InvalidOperationException.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesElementOperators
    {
        /// <summary>
        /// Приклад 59: Пошук першого рядка в масиві, що починається з літери 'o'.
        /// </summary>
        public void Linq59()
        {
            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region Make Sure to try yourself before looking at the code

            // Шукаємо перший рядок, де перша літера 'o'
            string startsWithO = strings.First(s => s[0] == 'o');

            Console.WriteLine("A string starting with 'o': {0}", startsWithO);

            #endregion
        }
    }
}
