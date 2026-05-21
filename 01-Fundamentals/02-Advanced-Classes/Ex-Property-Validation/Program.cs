// ============================================================================
// Тема: Валідація у властивостях і конструкторі — захист інваріантів об'єкта
// ----------------------------------------------------------------------------
// Головна перевага властивості перед відкритим полем — можливість ПЕРЕВІРИТИ
// значення перед записом. Це і є справжня інкапсуляція: об'єкт сам стежить,
// щоб його стан завжди був коректним («інваріант класу»).
//
// Прийоми, показані в прикладі:
//   * перевірка в set-акcесорі з киданням спеціалізованих винятків
//     (ArgumentNullException, ArgumentException, ArgumentOutOfRangeException);
//   * «сторожові умови» (guard clauses) на початку конструктора;
//   * конструктор, що присвоює ЧЕРЕЗ властивості — щоб не дублювати перевірки;
//   * нормалізація значення (Trim) перед збереженням;
//   * взаємозалежні поля (дата народження vs вік).
//
// Порівняйте з SOLIS-p145/p150, де властивості нічого не перевіряють.
// ============================================================================

using System;

namespace Ex_Property_Validation
{
    public class Person
    {
        private string _name = null!;
        private int _age;
        private string _email = null!;

        public Person(string name, int age, string email)
        {
            // Присвоюємо ЧЕРЕЗ властивості => перевірки з set-акcесорів
            // виконуються автоматично, дублювати їх у конструкторі не треба.
            Name = name;
            Age = age;
            Email = email;
        }

        public string Name
        {
            get => _name;
            set
            {
                // null і порожній рядок — різні помилки, тож і винятки різні
                ArgumentException.ThrowIfNullOrWhiteSpace(value);

                if (value.Trim().Length < 2)
                    throw new ArgumentException("Ім'я має містити щонайменше 2 символи", nameof(value));

                _name = value.Trim();   // нормалізуємо: прибираємо зайві пробіли
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                // Діапазон 0..130 — інваріант, який об'єкт нікому не дасть порушити
                if (value is < 0 or > 130)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "Вік має бути в межах 0..130");

                _age = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(value);

                // Навчальна спрощена перевірка (не повний стандарт RFC)
                if (!value.Contains('@') || !value.Contains('.'))
                    throw new ArgumentException($"Некоректна адреса: '{value}'", nameof(value));

                _email = value.Trim().ToLowerInvariant();
            }
        }

        // Властивість тільки для читання, похідна від інших даних
        public bool IsAdult => Age >= 18;

        public override string ToString() => $"{Name}, {Age} р., {Email} (повнолітній: {IsAdult})";
    }

    internal static class Program
    {
        private static void Main()
        {
            // 1) Коректний об'єкт
            var p = new Person("  Іван  ", 25, "Ivan@Example.COM");
            Console.WriteLine($"Створено: {p}");   // ім'я обрізане, email у нижньому регістрі

            // 2) Валідна зміна через властивість
            p.Age = 17;
            Console.WriteLine($"Після зміни віку: {p}");

            // 3) Кожна невдала спроба — зрозумілий виняток, стан об'єкта не псується
            TryChange("порожнє ім'я", () => p.Name = "   ");
            TryChange("вік -5",        () => p.Age = -5);
            TryChange("вік 200",       () => p.Age = 200);
            TryChange("email без @",   () => p.Email = "not-an-email");

            Console.WriteLine($"\nОб'єкт залишився коректним: {p}");

            // 4) Конструктор теж захищений
            TryChange("конструктор із віком 999", () => _ = new Person("Тест", 999, "t@t.t"));
        }

        private static void TryChange(string what, Action action)
        {
            try
            {
                action();
                Console.WriteLine($"[OK]    {what}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[БЛОК]  {what}: {ex.GetType().Name} — {ex.Message.Split('(')[0].Trim()}");
            }
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * required-властивості (C# 11) + init замість конструктора.
//  * Патерн «Result/TryParse» замість винятків, коли невалідні дані — норма.
//  * Атрибути DataAnnotations ([Range], [EmailAddress]) + Validator.
//  * Метод Validate() для перевірки взаємозв'язків кількох полів одночасно.
//  * Незмінний об'єкт: усе перевірити в конструкторі, властивості лише get.
// ----------------------------------------------------------------------------
