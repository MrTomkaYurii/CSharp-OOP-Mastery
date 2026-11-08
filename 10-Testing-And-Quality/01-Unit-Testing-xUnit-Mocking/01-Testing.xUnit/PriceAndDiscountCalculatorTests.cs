using Testing.Core.Domain.Services;
using Xunit;

namespace Testing.xUnit;

/// <summary>
/// УРОК 1: Основи модульного тестування у фреймворку xUnit.
/// 
/// Тут ми тестуємо чистий клас PriceAndDiscountCalculator (без зовнішніх залежностей)
/// за допомогою стандартного класу перевірок Assert із xUnit:
/// - Структура тесту AAA (Arrange – Act – Assert);
/// - Атрибут [Fact] для поодиноких сценаріїв;
/// - Атрибут [Theory] + [InlineData] та [MemberData] для параметризованих тестів;
/// - Перевірка очікуваних винятків через Assert.Throws<T>.
/// </summary>
public class PriceAndDiscountCalculatorTests
{
    private readonly PriceAndDiscountCalculator _sut; // System Under Test (Система під тестуванням)

    /// <summary>
    /// В xUnit конструктор викликається ПЕРЕД КОЖНИМ тестовим методом!
    /// Це гарантує, що кожен тест отримує абсолютно новий, "чистий" екземпляр калькулятора.
    /// </summary>
    public PriceAndDiscountCalculatorTests()
    {
        _sut = new PriceAndDiscountCalculator();
    }

    // =========================================================================
    // 1. ОДИНИЧНІ ТЕСТИ [Fact] ТА ПАТЕРН AAA (Arrange - Act - Assert)
    // =========================================================================

    [Fact]
    public void ApplyPercentageDiscount_Valid10Percent_Returns900For1000()
    {
        // 1. Arrange (Підготовка вхідних даних)
        decimal originalPrice = 1000m;
        decimal discountPercent = 10m;

        // 2. Act (Виконання дії, яку ми тестуємо)
        decimal finalPrice = _sut.ApplyPercentageDiscount(originalPrice, discountPercent);

        // 3. Assert (Перевірка результату: очікуване vs фактичне)
        Assert.Equal(900m, finalPrice);
    }

    [Fact]
    public void CalculatePriceWithVat_Default20PercentVat_Adds20PercentToNetPrice()
    {
        // Arrange
        decimal netPrice = 500m;

        // Act
        decimal priceWithVat = _sut.CalculatePriceWithVat(netPrice);

        // Assert
        Assert.Equal(600m, priceWithVat);
        Assert.InRange(priceWithVat, 500m, 1000m);
    }

    // =========================================================================
    // 2. ПАРАМЕТРИЗОВАНІ ТЕСТИ [Theory] + [InlineData]
    //    Замість копіювання 4 однакових тестів — передаємо таблицю параметрів!
    // =========================================================================

    [Theory]
    [InlineData(1000, 0,  1000)] // 0% знижки -> ціна не змінюється
    [InlineData(1000, 15, 850)]  // 15% знижки -> 850 грн
    [InlineData(200,  50, 100)]  // Максимальна дозволена знижка 50% -> 100 грн
    [InlineData(99.99, 10, 89.99)] // Перевірка округлення до копійок
    public void ApplyPercentageDiscount_VariousValidDiscounts_CalculatesExpectedPrice(
        decimal originalPrice,
        decimal discountPercent,
        decimal expectedFinalPrice)
    {
        // Act
        decimal actualPrice = _sut.ApplyPercentageDiscount(originalPrice, discountPercent);

        // Assert
        Assert.Equal(expectedFinalPrice, actualPrice);
    }

    [Theory]
    [InlineData("WELCOME10",   10)]
    [InlineData("  student20 ", 20)] // Перевірка нечутливості до регістру та пробілів
    [InlineData("VIP30",       30)]
    [InlineData("",            0)]
    [InlineData(null,          0)]
    public void GetPromoCodeDiscountPercent_KnownOrEmptyCodes_ReturnsExpectedPercent(
        string? promoCode,
        decimal expectedPercent)
    {
        // Act
        decimal actualPercent = _sut.GetPromoCodeDiscountPercent(promoCode);

        // Assert
        Assert.Equal(expectedPercent, actualPercent);
    }

    // =========================================================================
    // 3. ПАРАМЕТРИЗОВАНІ ТЕСТИ З МЕТОДУ-ДЖЕРЕЛА [MemberData]
    // =========================================================================

    public static IEnumerable<object[]> GetVatTestCases()
    {
        yield return new object[] { 100m, 20m, 120m };
        yield return new object[] { 100m, 7m,  107m };
        yield return new object[] { 250m, 0m,  250m };
    }

    [Theory]
    [MemberData(nameof(GetVatTestCases))]
    public void CalculatePriceWithVat_MemberDataScenarios_ReturnsCorrectTotal(
        decimal netPrice,
        decimal vatPercent,
        decimal expectedTotal)
    {
        // Act
        decimal actualTotal = _sut.CalculatePriceWithVat(netPrice, vatPercent);

        // Assert
        Assert.Equal(expectedTotal, actualTotal);
    }

    // =========================================================================
    // 4. ПЕРЕВІРКА ГЕНЕРАЦІЇ ВИНЯТКІВ (Assert.Throws<T>)
    // =========================================================================

    [Theory]
    [InlineData(-1)]  // Від'ємна знижка заборонена
    [InlineData(51)]  // Знижка понад 50% заборонена бізнес-правилом
    [InlineData(100)]
    public void ApplyPercentageDiscount_DiscountOutOfRange_ThrowsArgumentOutOfRangeException(decimal invalidDiscount)
    {
        // Act & Assert: перевіряємо, що виклик кидає саме ArgumentOutOfRangeException
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => _sut.ApplyPercentageDiscount(1000m, invalidDiscount));

        // Додатково можемо перевірити ім'я параметра та текст помилки
        Assert.Equal("discountPercent", exception.ParamName);
        Assert.Contains("від 0% до 50%", exception.Message);
    }

    [Fact]
    public void GetPromoCodeDiscountPercent_UnknownCode_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(
            () => _sut.GetPromoCodeDiscountPercent("FAKE_PROMO_99"));

        Assert.Contains("FAKE_PROMO_99", ex.Message);
    }
}
