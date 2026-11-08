using NUnit.Framework;
using Testing.Core.Domain.Services;

namespace Testing.NUnit;

/// <summary>
/// УРОК 3: Модульне тестування у фреймворку NUnit (порівняння з xUnit).
/// 
/// Головні відмінності NUnit від xUnit:
/// 1) Клас тесту позначається атрибутом [TestFixture] (в xUnit не потрібен).
/// 2) Одиничний тест позначається [Test] замість [Fact].
/// 3) Параметризований тест позначається [TestCase(...)] замість [Theory] + [InlineData(...)].
/// 4) Джерело даних позначається [TestCaseSource(nameof(...))] замість [MemberData(nameof(...))].
/// 5) Замість конструктора перед кожним тестом виконується метод [SetUp], а після — [TearDown].
/// 6) Фірмова Constraint-модель перевірок: Assert.That(actual, Is.EqualTo(expected))!
/// </summary>
[TestFixture]
public class PriceAndDiscountCalculatorNUnitTests
{
    private PriceAndDiscountCalculator _sut = null!;

    /// <summary>
    /// В NUnit метод [SetUp] автоматично запускається перед кожним тестом [Test] або [TestCase].
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _sut = new PriceAndDiscountCalculator();
    }

    [TearDown]
    public void TearDown()
    {
        // Виконується після кожного тесту для очищення ресурсів
    }

    // =========================================================================
    // 1. ОДИНИЧНИЙ ТЕСТ [Test] ТА CONSTRAINT-МОДЕЛЬ Assert.That(...)
    // =========================================================================

    [Test]
    public void ApplyPercentageDiscount_Valid10Percent_Returns900For1000()
    {
        // Arrange
        decimal originalPrice = 1000m;
        decimal discountPercent = 10m;

        // Act
        decimal finalPrice = _sut.ApplyPercentageDiscount(originalPrice, discountPercent);

        // Assert (Constraint Model: читається як англійське речення!)
        Assert.That(finalPrice, Is.EqualTo(900m));
        Assert.That(finalPrice, Is.InRange(800m, 1000m));
    }

    // =========================================================================
    // 2. ПАРАМЕТРИЗОВАНІ ТЕСТИ [TestCase] (аналог [Theory] + [InlineData] в xUnit)
    //    Зверніть увагу: NUnit підтримує властивість ExpectedResult прямо в атрибуті!
    // =========================================================================

    [TestCase(1000, 0,  1000)]
    [TestCase(1000, 15, 850)]
    [TestCase(200,  50, 100)]
    [TestCase(99.99, 10, 89.99)]
    public void ApplyPercentageDiscount_VariousDiscounts_ReturnsExpectedPrice(
        decimal originalPrice,
        decimal discountPercent,
        decimal expectedPrice)
    {
        // Act
        decimal actualPrice = _sut.ApplyPercentageDiscount(originalPrice, discountPercent);

        // Assert
        Assert.That(actualPrice, Is.EqualTo(expectedPrice));
    }

    [TestCase("WELCOME10",    ExpectedResult = 10)]
    [TestCase("  student20 ", ExpectedResult = 20)]
    [TestCase("VIP30",        ExpectedResult = 30)]
    [TestCase("",             ExpectedResult = 0)]
    [TestCase(null,           ExpectedResult = 0)]
    public decimal GetPromoCodeDiscountPercent_ValidCodes_ReturnsExpectedPercent(string? promoCode)
    {
        // У NUnit якщо вказано ExpectedResult, достатньо просто повернути значення з методу тесту!
        return _sut.GetPromoCodeDiscountPercent(promoCode);
    }

    // =========================================================================
    // 3. ПАРАМЕТРИЗОВАНІ ТЕСТИ З ДЖЕРЕЛА [TestCaseSource] (аналог [MemberData])
    // =========================================================================

    public static IEnumerable<TestCaseData> GetVatCases()
    {
        yield return new TestCaseData(100m, 20m, 120m).SetName("ПДВ 20% (стандартна ставка)");
        yield return new TestCaseData(100m, 7m,  107m).SetName("ПДВ 7% (пільгова ставка)");
        yield return new TestCaseData(250m, 0m,  250m).SetName("ПДВ 0% (експорт)");
    }

    [TestCaseSource(nameof(GetVatCases))]
    public void CalculatePriceWithVat_FromTestCaseSource_ReturnsCorrectAmount(
        decimal netPrice,
        decimal vatPercent,
        decimal expectedTotal)
    {
        decimal actualTotal = _sut.CalculatePriceWithVat(netPrice, vatPercent);

        Assert.That(actualTotal, Is.EqualTo(expectedTotal));
    }

    // =========================================================================
    // 4. ПЕРЕВІРКА ВИНЯТКІВ У NUNIT ЧЕРЕЗ Throws.TypeOf<T>()
    // =========================================================================

    [TestCase(-5)]
    [TestCase(55)]
    public void ApplyPercentageDiscount_InvalidDiscount_ThrowsArgumentOutOfRangeException(decimal invalidDiscount)
    {
        // У NUnit перевірка винятків теж записується через Assert.That(..., Throws.TypeOf<...>())
        Assert.That(
            () => _sut.ApplyPercentageDiscount(1000m, invalidDiscount),
            Throws.TypeOf<ArgumentOutOfRangeException>()
                  .With.Property("ParamName").EqualTo("discountPercent")
                  .And.Message.Contain("від 0% до 50%"));
    }

    [Test]
    public void GetPromoCodeDiscountPercent_UnknownPromoCode_ThrowsArgumentException()
    {
        Assert.That(
            () => _sut.GetPromoCodeDiscountPercent("UNKNOWN_CODE"),
            Throws.ArgumentException.With.Message.Contain("UNKNOWN_CODE"));
    }
}
