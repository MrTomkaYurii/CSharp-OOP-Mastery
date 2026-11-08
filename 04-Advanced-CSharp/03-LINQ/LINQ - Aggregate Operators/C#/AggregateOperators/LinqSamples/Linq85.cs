using System;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Max (Пошук максимального елемента)
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Повертає максимальне значення з числової або порівнюваної (IComparable) послідовності.
    //
    // Принцип роботи:
    //   - Здійснює повний лінійний прохід по колекції O(N), зберігаючи найбільший знайдений елемент.
    //   - Якщо колекція порожня, для не-nullable типів генерує виняток InvalidOperationException.
    //   - Для Nullable типів повертає null при порожній послідовності або наявності лише null значень.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 85: Знаходження максимального числа в масиві за допомогою Max.
        /// </summary>
        public void Linq85()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region Make Sure to try yourself before looking at the code

            // Знаходимо максимальне число (у даному масиві - 9)
            int maxNum = numbers.Max();

            #endregion

            Console.WriteLine("Original 101 Linq example.");
            Console.WriteLine("The maximum number is {0}.", maxNum);
        }
    }
}
