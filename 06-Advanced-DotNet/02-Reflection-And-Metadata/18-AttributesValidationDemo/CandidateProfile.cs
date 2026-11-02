namespace AttributesValidationDemo;

/// <summary>
/// Доменна модель профілю кандидата на вакансію.
/// Поєднує валідаційні атрибути на рівні самого КЛАСУ та на рівні окремих ВЛАСТИВОСТЕЙ.
/// </summary>
[RangeConsistency("ExpectedMinSalary", "ExpectedMaxSalary",
    ErrorMessage = "[Помилка рівня класу]: Очікувана мінімальна зарплата не може бути більшою за максимальну.")]
[ExperienceAgeConsistency("Age", "YearsOfExperience", MinimumWorkingAge = 18,
    ErrorMessage = "[Помилка рівня класу]: Заявлений стаж роботи не узгоджується з віком кандидата (праця з 18 років).")]
public class CandidateProfile
{
    [RequiredField(ErrorMessage = "Ім'я кандидата є обов'язковим для заповнення.")]
    [StringLengthRange(2, 50, ErrorMessage = "Ім'я повинно містити від 2 до 50 символів.")]
    public string FullName { get; set; } = string.Empty;

    [RequiredField(ErrorMessage = "Email адреса є обов'язковою.")]
    [StringLengthRange(5, 100)]
    public string Email { get; set; } = string.Empty;

    [NumberRange(18, 70, ErrorMessage = "Вік кандидата має бути в межах від 18 до 70 років.")]
    public int Age { get; set; }

    [NumberRange(0, 50, ErrorMessage = "Стаж роботи має бути в межах від 0 до 50 років.")]
    public int YearsOfExperience { get; set; }

    [NumberRange(500, 20000, ErrorMessage = "Мінімальна очікувана зарплата має бути від 500 до 20000 USD.")]
    public int ExpectedMinSalary { get; set; }

    [NumberRange(500, 25000, ErrorMessage = "Максимальна очікувана зарплата має бути від 500 до 25000 USD.")]
    public int ExpectedMaxSalary { get; set; }
}
