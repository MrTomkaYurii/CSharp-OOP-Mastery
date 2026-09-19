using System.ComponentModel.DataAnnotations;

namespace DI.GenericHost.Worker.Configuration;

/// <summary>
/// Строго типізована конфігурація фонового воркера з валідацією.
/// </summary>
public class WorkerOptions
{
    public const string SectionName = "WorkerSettings";

    [Required(ErrorMessage = "WorkerName є обов'язковим параметром конфігурації.")]
    public string WorkerName { get; set; } = string.Empty;

    [Range(1, 60, ErrorMessage = "IntervalSeconds повинен бути в діапазоні від 1 до 60 секунд.")]
    public int IntervalSeconds { get; set; } = 5;

    [Range(1, 100, ErrorMessage = "BatchSize повинен бути в межах від 1 до 100.")]
    public int BatchSize { get; set; } = 10;

    public int MaxIterations { get; set; } = 3;
}
