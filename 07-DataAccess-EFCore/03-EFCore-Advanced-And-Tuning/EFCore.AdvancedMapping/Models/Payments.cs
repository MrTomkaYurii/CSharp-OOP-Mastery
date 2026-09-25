namespace EFCore.AdvancedMapping.Models;

/// <summary>
/// Базовий абстрактний клас для демонстрації TPH (Table-per-Hierarchy) успадкування.
/// </summary>
public abstract class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}

public class CreditCardPayment : Payment
{
    public string CardMask { get; set; } = string.Empty;
    public string CardHolder { get; set; } = string.Empty;
}

public class CryptoPayment : Payment
{
    public string CryptoCurrency { get; set; } = "BTC";
    public string TxHash { get; set; } = string.Empty;
}

public class BankTransferPayment : Payment
{
    public string Iban { get; set; } = string.Empty;
    public string SwiftCode { get; set; } = string.Empty;
}
