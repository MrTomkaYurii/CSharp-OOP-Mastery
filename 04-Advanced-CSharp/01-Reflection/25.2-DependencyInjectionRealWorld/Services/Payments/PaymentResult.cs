namespace DependencyInjection.Services.Payments;

/// <summary>
/// Результат виконання платіжної транзакції.
/// </summary>
public record PaymentResult(bool Success, string TransactionId, string Message);
