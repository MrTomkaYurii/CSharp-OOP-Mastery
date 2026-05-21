// ============================================================================
// Тема: record проти class — коли що використовувати
// ----------------------------------------------------------------------------
// record (C# 9) — це посилальний тип (як class), але «заточений» під зберігання
// ДАНИХ. Компілятор автоматично генерує те, що для class довелося б писати руками:
//   * порівняння ЗА ЗНАЧЕННЯМ (Equals/GetHashCode по всіх полях);
//   * зручний ToString() з переліком властивостей;
//   * деконструктор (var (a, b) = obj);
//   * with-вираз — копія об'єкта зі зміненими окремими полями;
//   * для позиційного record — конструктор і init-властивості.
//
// class порівнюється ЗА ПОСИЛАННЯМ (два різні об'єкти з однаковими даними !=),
// і ToString() за замовчуванням друкує лише назву типу.
//
// Правило вибору: дані без поведінки, бажано незмінні -> record.
// Об'єкт із логікою, ідентичністю, змінним станом -> class.
// ============================================================================

using System;

namespace Ex_Record_vs_Class
{
    // Звичайний клас: усе «за замовчуванням»
    public class PersonClass
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }

    // Позиційний record: один рядок замість десятків. Властивості { get; init; }
    // (задаються при створенні, потім незмінні). Порівняння — за значенням.
    public record PersonRecord(string FirstName, string LastName);

    // record теж може мати тіло: додаткові члени, обчислення, методи
    public record Employee(string FirstName, string LastName, decimal Salary)
    {
        public string FullName => $"{FirstName} {LastName}";

        // Можна додати власну валідацію в тілі
        public decimal AnnualSalary => Salary * 12;
    }

    internal static class Program
    {
        private static void Main()
        {
            // --- 1. Порівняння: посилання проти значення ---------------------
            var c1 = new PersonClass { FirstName = "Ада", LastName = "Лавлейс" };
            var c2 = new PersonClass { FirstName = "Ада", LastName = "Лавлейс" };
            Console.WriteLine("КЛАС:");
            Console.WriteLine($"  c1.Equals(c2) = {c1.Equals(c2)}   // False — різні об'єкти в пам'яті");
            Console.WriteLine($"  c1.ToString() = {c1}");            // назва типу

            var r1 = new PersonRecord("Ада", "Лавлейс");
            var r2 = new PersonRecord("Ада", "Лавлейс");
            Console.WriteLine("RECORD:");
            Console.WriteLine($"  r1 == r2      = {r1 == r2}");       // True — однакові дані
            Console.WriteLine($"  r1.ToString() = {r1}");            // PersonRecord { FirstName = Ада, LastName = Лавлейс }

            // --- 2. Незмінність + with-вираз --------------------------------
            var promoted = new Employee("Грейс", "Гоппер", 1000m);
            var withRaise = promoted with { Salary = 1200m };   // копія зі зміненим полем
            Console.WriteLine("\nWITH-вираз (нова копія, оригінал не змінюється):");
            Console.WriteLine($"  оригінал: {promoted}");
            Console.WriteLine($"  копія:    {withRaise}");

            // --- 3. Деконструкція record ----------------------------------
            var (first, last) = r1;
            Console.WriteLine($"\nДеконструкція: first='{first}', last='{last}'");

            // --- 4. record із поведінкою ---------------------------------
            Console.WriteLine($"\n{withRaise.FullName}: річна зарплата = {withRaise.AnnualSalary}");
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * record struct — значимий record (для маленьких незмінних даних).
//  * Успадкування record від record і як with працює з ієрархією.
//  * Кастомний конструктор record із валідацією (кинути ArgumentException).
//  * Порівняння продуктивності Equals для record і class.
//  * Показати, що record із set-властивістю (не init) втрачає незмінність.
// ----------------------------------------------------------------------------
