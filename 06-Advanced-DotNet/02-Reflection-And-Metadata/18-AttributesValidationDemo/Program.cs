using AttributesValidationDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 18. ДЕКЛАРАТИВНА ВАЛІДАЦІЯ ДАНИХ НА РІВНІ КЛАСУ ТА ВЛАСТИВОСТЕЙ");
Console.WriteLine(" Як рефлексія перевіряє окремі поля та цілісну узгодженість об'єкта");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Сценарій 1: Порушення правил окремих властивостей (Property-Level)
// -----------------------------------------------------------------------
Console.WriteLine("--- Сценарій 1: Кандидат із помилками в окремих властивостях ---");
var invalidPropertiesCandidate = new CandidateProfile
{
    FullName = "Я",           // Занадто коротке ім'я (< 2 символів)
    Email = "",               // Порожній обов'язковий Email
    Age = 16,                 // Неповнолітній (< 18 років)
    YearsOfExperience = 0,
    ExpectedMinSalary = 1000,
    ExpectedMaxSalary = 2000
};

PrintValidationResult("Кандидат #1 (Помилки властивостей)", invalidPropertiesCandidate);

// -----------------------------------------------------------------------
// Сценарій 2: Усі окремі властивості валідні, але порушено правила КЛАСУ!
// -----------------------------------------------------------------------
Console.WriteLine("--- Сценарій 2: Властивості коректні окремо, але порушена логіка КЛАСУ ---");
Console.WriteLine("Уявимо: Вік = 22 роки (валідно), Стаж = 10 років (валідно окремо),");
Console.WriteLine("MinSalary = 4500 (валідно), MaxSalary = 3000 (валідно окремо).");
Console.WriteLine("Проте разом у межах класу це суперечить здоровому глузду!\n");

var inconsistentClassCandidate = new CandidateProfile
{
    FullName = "Дмитро Бондаренко",
    Email = "dmytro@example.com",
    Age = 22,                 // Окремо валідно (18..70)
    YearsOfExperience = 10,   // Окремо валідно (0..50), але у 22 роки стаж не може бути 10 років!
    ExpectedMinSalary = 4500, // Окремо валідно (500..20000)
    ExpectedMaxSalary = 3000  // Окремо валідно, але Min (4500) > Max (3000)!
};

PrintValidationResult("Кандидат #2 (Порушення інваріантів рівня класу)", inconsistentClassCandidate);

// -----------------------------------------------------------------------
// Сценарій 3: Повністю валідний об'єкт (і властивості, і рівень класу)
// -----------------------------------------------------------------------
Console.WriteLine("--- Сценарій 3: Повністю коректний профіль кандидата ---");
var validCandidate = new CandidateProfile
{
    FullName = "Олена Шевченко",
    Email = "olena.shevchenko@example.com",
    Age = 29,
    YearsOfExperience = 7,
    ExpectedMinSalary = 3500,
    ExpectedMaxSalary = 5000
};

PrintValidationResult("Кандидат #3 (Валідний профіль)", validCandidate);

static void PrintValidationResult(string label, CandidateProfile candidate)
{
    Console.WriteLine($"Перевірка: {label}");
    if (ClassAndPropertyValidator.Validate(candidate, out List<string> errors))
    {
        Console.WriteLine("  [УСПІХ]: Об'єкт успішно пройшов усі перевірки властивостей та класу!\n");
    }
    else
    {
        Console.WriteLine($"  [ВІДХИЛЕНО]: Знайдено порушень: {errors.Count}");
        foreach (string err in errors)
        {
            Console.WriteLine($"    - {err}");
        }
        Console.WriteLine();
    }
}
