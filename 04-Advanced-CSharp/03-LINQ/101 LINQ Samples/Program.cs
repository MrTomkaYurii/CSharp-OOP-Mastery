using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AggregateOperators;
using ConversionOperators;
using CustomSequenceOperators;
using ElementOperators;
using GenerationOperators;
using GroupingOperators;
using JoinOperators;
using MiscellaneousOperators;
using OrderingOperators;
using PartitioningOperators;
using ProjectionOperators;
using Quantifiers;
using QueryExecution;
using RestrictionOperators;
using SetOperators;

namespace _101_LINQ_Samples
{
    /// <summary>
    /// Головна точка входу для запуску прикладів LINQ (101 LINQ Samples).
    /// Містить структурований каталог усіх 15 категорій операторів LINQ.
    /// Розкоментуйте відповідні рядки для запуску потрібних прикладів.
    /// </summary>
    class Program
    { 
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== 101 LINQ SAMPLES (.NET 10) ===");
            Console.WriteLine("Оберіть потрібний блок прикладів у коді Program.cs для запуску.\n");

            #region 1. Restriction Operators (Оператори фільтрації)

            // "Where" - Фільтрує значення послідовності на основі функції-предиката.
            // "OfType" - Фільтрує значення на основі їхньої сумісності з указаним типом даних.

            // LinqSamplesRestrictionOperators samples_a = new LinqSamplesRestrictionOperators();

            // samples_a.Linq1();
            // samples_a.Linq1A();
            // samples_a.Linq2();
            // samples_a.Linq2A();
            // samples_a.Linq3();
            // samples_a.Linq3A();
            // samples_a.Linq4();
            // samples_a.Linq4A();
            // samples_a.Linq5();
            // samples_a.OfTypeLinq5A();

            #endregion RestrictionOperators

            #region 2. Projection Operators (Оператори проекції)

            // "Select" - Проектує (трансформує) кожен елемент послідовності в нову форму.
            // "SelectMany" - Проектує послідовності послідовностей та сплощує (flatten) їх в єдиний плаский потік.

            // LinqSamplesProjectionOperators samples_b = new LinqSamplesProjectionOperators();

            // samples_b.DataSetLinq6();
            // samples_b.DataSetLinq6A();
            // samples_b.DataSetLinq7();
            // samples_b.DataSetLinq7A();
            // samples_b.DataSetLinq8();
            // samples_b.DataSetLinq8A();
            // samples_b.DataSetLinq9();
            // samples_b.DataSetLinq9A();
            // samples_b.DataSetLinq10();
            // samples_b.DataSetLinq10A();
            // samples_b.DataSetLinq11();
            // samples_b.DataSetLinq11A();
            // samples_b.DataSetLinq12();
            // samples_b.DataSetLinq13();
            // samples_b.DataSetLinq13A();
            // samples_b.DataSetLinq14();
            // samples_b.DataSetLinq14A();
            // samples_b.DataSetLinq15();
            // samples_b.DataSetLinq16();
            // samples_b.DataSetLinq16A();
            // samples_b.DataSetLinq17();
            // samples_b.DataSetLinq17A();
            // samples_b.DataSetLinq18();
            // samples_b.DataSetLinq18A();
            // samples_b.DataSetLinq19();
            // samples_b.DataSetLinq19A();

            #endregion

            #region 3. Partitioning Operators (Оператори секціонування / пагінації)

            // "Take" - Вибирає вказану кількість елементів із початку послідовності.
            // "Skip" - Пропускає вказану кількість елементів із початку та повертає решту.
            // "TakeWhile" - Вибирає елементи, доки виконується заданий булевий предикат.
            // "SkipWhile" - Пропускає елементи, доки виконується предикат, і повертає решту.

            // LinqSamplesPartitioningOperators sample_c = new LinqSamplesPartitioningOperators();

            // sample_c.Linq20();
            // sample_c.Linq21();
            // sample_c.Linq22();
            // sample_c.Linq23();
            // sample_c.Linq23A();
            // sample_c.Linq24();
            // sample_c.Linq25();
            // sample_c.Linq26();
            // sample_c.Linq27();

            #endregion PartitioningOperators

            #region 4. Ordering Operators (Оператори впорядкування / сортування)

            // "OrderBy" - Сортує елементи за зростанням (первинне сортування).
            // "OrderByDescending" - Сортує елементи за спаданням (первинне сортування).
            // "ThenBy" - Виконує вторинне сортування за зростанням.
            // "ThenByDescending" - Виконує вторинне сортування за спаданням.
            // "Reverse" - Інвертує порядок елементів у колекції на зворотний.

            // LinqSamplesOrderingOperators linqSamples_d = new LinqSamplesOrderingOperators();

            // linqSamples_d.Linq28();
            // linqSamples_d.Linq28A();
            // linqSamples_d.Linq29();
            // linqSamples_d.Linq29A();
            // linqSamples_d.Linq30();
            // linqSamples_d.Linq30A();
            // linqSamples_d.Linq31();
            // linqSamples_d.Linq32();
            // linqSamples_d.Linq32A();
            // linqSamples_d.Linq33();
            // linqSamples_d.Linq33A();
            // linqSamples_d.Linq34();
            // linqSamples_d.Linq35();
            // linqSamples_d.Linq35A();
            // linqSamples_d.Linq36();
            // linqSamples_d.Linq37();
            // linqSamples_d.Linq37A();
            // linqSamples_d.Linq38();
            // linqSamples_d.Linq39();
            // linqSamples_d.Linq39A();

            #endregion

            #region 5. Grouping Operators (Оператори групування)

            // "GroupBy" - Групує елементи за спільним ключем і повертає групи типу IGrouping<TKey, TElement>.

            // LinqSamplesGroupingOperators samples_e = new LinqSamplesGroupingOperators();

            // samples_e.DataSetLinq40();
            // samples_e.DataSetLinq40A();
            // samples_e.DataSetLinq41();
            // samples_e.DataSetLinq41A();
            // samples_e.DataSetLinq42();
            // samples_e.DataSetLinq42A();
            // samples_e.DataSetLinq43();
            // samples_e.DataSetLinq43A();
            // samples_e.DataSetLinq44();
            // samples_e.DataSetLinq45();

            #endregion

            #region 6. Set Operators (Оператори над множинами)

            // "Distinct" - Видаляє повтори та повертає лише унікальні елементи.
            // "Union" - Об'єднує дві множини, усуваючи дублікати.
            // "Intersect" - Повертає перетин двох множин (спільні елементи).
            // "Except" - Повертає різницю множин (елементи першої, яких немає в другій).

            // LinqSamplesSetOperators sample_f = new LinqSamplesSetOperators();

            // sample_f.Linq46();
            // sample_f.Linq47();
            // sample_f.Linq47A();
            // sample_f.Linq48();
            // sample_f.Linq49();
            // sample_f.Linq49A();
            // sample_f.Linq50();
            // sample_f.Linq51();
            // sample_f.Linq51A();
            // sample_f.Linq52();
            // sample_f.Linq53();
            // sample_f.Linq53A();

            #endregion

            #region 7. Conversion Operators (Оператори перетворення та матеріалізації)

            // "ToArray" - Примусово виконує запит і записує результат у масив T[].
            // "ToList" - Примусово виконує запит і створює список List<T>.
            // "ToDictionary" - Створює словник Dictionary<TKey, TValue> на основі селекторів ключа/значення.
            // "OfType" - Фільтрує колекцію та повертає лише елементи вказаного типу.

            // LinqSamplesConversionOperators samples_g = new LinqSamplesConversionOperators();

            // samples_g.Linq54();
            // samples_g.Linq54A();
            // samples_g.Linq55();
            // samples_g.Linq55A();
            // samples_g.Linq56();
            // samples_g.Linq57();

            #endregion

            #region 8. Element Operators (Оператори вибору окремих елементів)

            // "First" - Повертає перший елемент; викидає виняток, якщо послідовність порожня.
            // "FirstOrDefault" - Повертає перший елемент або дефолтне значення (null/0), якщо елементів немає.
            // "ElementAt" - Повертає елемент за числовим індексом (нумерація з 0).

            // LinqSamplesElementOperators samples_h = new LinqSamplesElementOperators();

            // samples_h.Linq58();
            // samples_h.Linq58A();
            // samples_h.Linq59();
            // samples_h.Linq61();
            // samples_h.Linq62();
            // samples_h.Linq64();
            // samples_h.Linq64A();

            #endregion

            #region 9. Generation Operators (Оператори генерації)

            // "Range" - Ліниво генерує послідовність цілих чисел у заданому діапазоні.
            // "Repeat" - Ліниво генерує послідовність із багаторазовим повторенням одного значення.

            // LinqSamplesGenerationOperators samples_i = new LinqSamplesGenerationOperators();

            // samples_i.Linq65();
            // samples_i.Linq65A();
            // samples_i.Linq66();

            #endregion

            #region 10. Quantifiers (Оператори-квантори)

            // "Any" - Перевіряє, чи задовольняє хоча б один елемент умову (квантор існування).
            // "All" - Перевіряє, чи задовольняють абсолютно всі елементи умову (квантор загальності).

            // LinqSamplesQuantifiers samples_j = new LinqSamplesQuantifiers();

            // samples_j.Linq67();
            // samples_j.Linq69();
            // samples_j.Linq69A();
            // samples_j.Linq70();
            // samples_j.Linq72();
            // samples_j.Linq72A();

            #endregion

            #region 11. Aggregate Operators (Агрегатні оператори)

            // "Count" - Рахує кількість елементів у послідовності.
            // "Sum" - Обчислює суму числових значень.
            // "Min" - Знаходить мінімальне значення.
            // "Max" - Знаходить максимальне значення.
            // "Average" - Обчислює середнє арифметичне значення.
            // "Aggregate" - Виконує довільну згортку послідовності (Fold/Reduce).

            // LinqSamplesAggregateOperators samples_k = new LinqSamplesAggregateOperators();

            // samples_k.Linq73();
            // samples_k.Linq74();
            // samples_k.Linq76();
            // samples_k.Linq76A();
            // samples_k.Linq77();
            // samples_k.Linq77A();
            // samples_k.Linq78();
            // samples_k.Linq79();
            // samples_k.Linq80();
            // samples_k.Linq80A();
            // samples_k.Linq81();
            // samples_k.Linq82();
            // samples_k.Linq83();
            // samples_k.Linq83A();
            // samples_k.Linq84();
            // samples_k.Linq84A();
            // samples_k.Linq85();
            // samples_k.Linq86();
            // samples_k.Linq87();
            // samples_k.Linq87A();
            // samples_k.Linq88();
            // samples_k.Linq88A();
            // samples_k.Linq89();
            // samples_k.Linq90();
            // samples_k.Linq91();
            // samples_k.Linq91A();
            // samples_k.Linq92();
            // samples_k.Linq93();

            #endregion

            #region 12. Miscellaneous Operators (Різні допоміжні оператори)

            // "Concat" - З'єднує дві послідовності одну за одною без видалення дублікатів.
            // "SequenceEqual" - Поелементно перевіряє рівність двох послідовностей та точний порядок елементів.

            // LinqSamplesMiscellaneousOperators samples_l = new LinqSamplesMiscellaneousOperators();

            // samples_l.Linq94();
            // samples_l.Linq95();
            // samples_l.Linq95A();
            // samples_l.Linq96();
            // samples_l.Linq97();

            #endregion

            #region 13. Custom Sequence Operators (Користувацькі оператори)

            // "Combine" - Власний метод розширення для синхронного зшивання двох послідовностей (прототип Zip).

            // LinqSamplesCustomSequenceOperators samples_m = new LinqSamplesCustomSequenceOperators();

            // samples_m.DataSetLinq98();

            #endregion

            #region 14. Query Execution (Механізми виконання запитів)

            // "Deferred Execution" - Запит виконується лише під час ітерації.
            // "Immediate Execution" - Примусове виконання через ToList().
            // "Query Reuse" - Повторне обчислення запиту над оновленими джерелами даних.

            // LinqSamplesQueryExecution samples_n = new LinqSamplesQueryExecution();

            // samples_n.Linq99();
            // samples_n.Linq100();
            // samples_n.Linq101();

            #endregion

            #region 15. Join Operators (Оператори з'єднання)

            // "Join" - Внутрішнє з'єднання двох послідовностей за спільним ключем (Inner Equijoin).
            // "Group Join" - Ієрархічне з'єднання один-до-багатьох (1:N).
            // "Cross Join / Flattened Group Join" - Сплощення групового з'єднання.
            // "Left Outer Join" - Ліве зовнішнє з'єднання (збереження всіх записів лівої таблиці через DefaultIfEmpty).
            // "Composite Keys" - З'єднання за складеними ключами (City, Country).

            // LinqSamplesJoinOperators samples_o = new LinqSamplesJoinOperators();

            // samples_o.Linq102();
            // samples_o.Linq102A();
            // samples_o.Linq103();
            // samples_o.Linq103A();
            // samples_o.Linq104();
            // samples_o.Linq104A();
            // samples_o.Linq105();
            // samples_o.Linq105A();
            // samples_o.Linq106();
            // samples_o.Linq106A();
            // samples_o.Linq107();
            // samples_o.Linq107A();

            #endregion

            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }
        }
    }
}
