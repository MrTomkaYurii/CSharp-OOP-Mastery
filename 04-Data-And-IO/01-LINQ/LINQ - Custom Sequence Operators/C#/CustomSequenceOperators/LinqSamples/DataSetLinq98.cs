using LinqSamplesCommon.LinqSamplesHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomSequenceOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Створення власних LINQ-операторів (Custom Sequence Operators)
    // ==============================================================================================
    // Категорія: Custom Sequence Operators (Користувацькі оператори послідовностей)
    // Призначення: Демонстрація створення власного методу розширення для IEnumerable<T>
    //              із лінивою ітерацією (yield return).
    //
    // Принцип роботи:
    //   - Будь-який LINQ-оператор — це публічний статичний метод розширення (extension method)
    //     для інтерфейсу IEnumerable<T>, оголошений у статичному класі.
    //   - Ключове слово yield return дозволяє будувати повноцінний ітераторний конвеєр
    //     із підтримкою відкладеного виконання (Deferred Execution / Streaming).
    //   - У цьому прикладі створено метод Combine, який синхронно перебирає дві послідовності паралельно
    //     та застосовує функцію поєднання пар (аналог стандартного Enumerable.Zip, введеного в .NET 4.0).
    //   - Обчислення скалярного добутку векторів: dotProduct = Sum(A[i] * B[i]).
    //
    // Тип виконання:
    //   - Combine: Відкладене виконання (Deferred Execution, Streaming).
    //   - Sum: Негайне виконання (Immediate Execution).
    // ==============================================================================================

    /// <summary>
    /// Користувацький метод розширення Combine для паралельного зшивання двох послідовностей DataRow (прототип Enumerable.Zip).
    /// </summary>
    public static class CombineExtension
    {
        public static IEnumerable<S> Combine<S>(this IEnumerable<DataRow> first, IEnumerable<DataRow> second, System.Func<DataRow, DataRow, S> func)
        {
            using (IEnumerator<DataRow> e1 = first.GetEnumerator(), e2 = second.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return func(e1.Current, e2.Current);
                }
            }
        }
    }

    public class LinqSamplesCustomSequenceOperators
    {
        private DataSet testDS;

        public LinqSamplesCustomSequenceOperators()
        {
            testDS = TestHelper.CreateTestDataset();
        }

        /// <summary>
        /// Приклад 98: Обчислення скалярного добутку векторів (dot product) за допомогою власного оператора Combine та методу Sum.
        /// </summary>
        public void DataSetLinq98()
        {
            var numbersA = testDS.Tables["NumbersA"].AsEnumerable();
            var numbersB = testDS.Tables["NumbersB"].AsEnumerable();

            #region Make Sure to try yourself before looking at the code

            // Синхронно множимо відповідні елементи двох послідовностей через Combine і підсумовуємо результат через Sum
            int dotProduct = numbersA.Combine(
                                numbersB, 
                                (a, b) => a.Field<int>("number") * b.Field<int>("number")).Sum();

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("Dot product: {0}", dotProduct);
        }
    }
}
