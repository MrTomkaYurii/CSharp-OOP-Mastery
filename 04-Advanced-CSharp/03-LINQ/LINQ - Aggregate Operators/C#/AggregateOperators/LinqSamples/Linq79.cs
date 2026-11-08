using System;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Sum з функцією селектора
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Перетворює кожен елемент послідовності на числове значення та обчислює їхню суму.
    //
    // Принцип роботи:
    //   - Еквівалентний виклику .Select(selector).Sum(), але оптимізований в один прохід.
    //   - Дозволяє сумувати довжини рядків, вартості замовлень, вагу товарів тощо без створення
    //     проміжної проектованої колекції в пам'яті.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 79: Обчислення сумарної кількості символів у всіх словах масиву за допомогою Sum(w => w.Length).
        /// </summary>
        public void Linq79()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region Make Sure to try yourself before looking at the code

            // Сумуємо значення властивості Length для кожного рядка
            double totalChars = words.Sum(w => w.Length);

            #endregion

            Console.WriteLine("There are a total of {0} characters in these words.", totalChars);
        }
    }
}
