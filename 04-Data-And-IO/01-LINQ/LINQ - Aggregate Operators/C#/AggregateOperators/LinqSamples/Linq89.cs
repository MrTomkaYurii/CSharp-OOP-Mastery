using System;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Average (Середнє арифметичне)
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Обчислює середнє арифметичне значення послідовності чисел.
    //
    // Принцип роботи:
    //   - Підсумовує всі елементи й ділить на їхню загальну кількість: Sum / Count.
    //   - Для int-послідовностей результат автоматично приводиться до double.
    //   - Якщо послідовність порожня, для не-nullable типів викидає InvalidOperationException.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 89: Обчислення середнього арифметичного значення чисел у масиві за допомогою Average.
        /// </summary>
        public void Linq89()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region Make Sure to try yourself before looking at the code

            // Обчислюємо середнє значення (повертає double)
            double averageNum = numbers.Average();

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("The average number is {0}.", averageNum);
        }
    }
}
