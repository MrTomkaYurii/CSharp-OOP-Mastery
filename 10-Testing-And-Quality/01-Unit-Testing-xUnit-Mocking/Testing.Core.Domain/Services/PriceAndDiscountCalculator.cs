namespace Testing.Core.Domain.Services;

/// <summary>
/// Чистий сервіс (без зовнішніх залежностей) для розрахунку вартості замовлення,
/// знижок за промокодами та податку (ПДВ).
/// Ідеально підходить для першого знайомства з модульними тестами (xUnit та NUnit).
/// </summary>
public class PriceAndDiscountCalculator
{
    /// <summary>
    /// Обчислює вартість товару зі знижкою у відсотках (від 0 до 50%).
    /// </summary>
    public decimal ApplyPercentageDiscount(decimal originalPrice, decimal discountPercent)
    {
        if (originalPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(originalPrice), "Початкова ціна не може бути від'ємною.");

        if (discountPercent is < 0 or > 50)
            throw new ArgumentOutOfRangeException(nameof(discountPercent), "Знижка повинна бути в межах від 0% до 50%.");

        decimal discountAmount = originalPrice * (discountPercent / 100m);
        return Math.Round(originalPrice - discountAmount, 2);
    }

    /// <summary>
    /// Повертає відсоток знижки за промокодом.
    /// </summary>
    public decimal GetPromoCodeDiscountPercent(string? promoCode)
    {
        if (string.IsNullOrWhiteSpace(promoCode))
            return 0m;

        return promoCode.Trim().ToUpperInvariant() switch
        {
            "WELCOME10" => 10m,
            "STUDENT20" => 20m,
            "VIP30"     => 30m,
            _ => throw new ArgumentException($"Невідомий промокод: {promoCode}", nameof(promoCode))
        };
    }

    /// <summary>
    /// Додає ПДВ (за замовчуванням 20%) до суми.
    /// </summary>
    public decimal CalculatePriceWithVat(decimal netPrice, decimal vatPercent = 20m)
    {
        if (netPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(netPrice), "Сума без ПДВ не може бути від'ємною.");

        if (vatPercent < 0)
            throw new ArgumentOutOfRangeException(nameof(vatPercent), "Ставка ПДВ не може бути від'ємною.");

        return Math.Round(netPrice * (1m + vatPercent / 100m), 2);
    }
}
