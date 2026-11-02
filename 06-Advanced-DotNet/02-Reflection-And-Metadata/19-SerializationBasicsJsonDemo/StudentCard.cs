using System.Text.Json.Serialization;

namespace SerializationBasicsJsonDemo;

/// <summary>
/// Навчальна модель картки студента для знайомства зі структурою JSON.
/// </summary>
public class StudentCard
{
    [JsonPropertyName("student_id")]
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("average_score")]
    public double AverageScore { get; set; }

    [JsonPropertyName("is_scholarship_holder")]
    public bool HasScholarship { get; set; }

    [JsonPropertyName("courses")]
    public List<string> Courses { get; set; } = new();

    // Конфіденційне поле, яке НЕ повинно потрапляти у JSON при передачі по мережі
    [JsonIgnore]
    public string TemporaryPinCode { get; set; } = string.Empty;
}
