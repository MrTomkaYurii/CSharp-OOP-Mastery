using Moq;
using Testing.Core.Domain.Services;
using Xunit;

namespace Testing.MoqTests;

/// <summary>
/// УРОК 5: Покрокове опанування бібліотеки мокінгу Moq (від нуля до профі).
/// 
/// Навіщо потрібен Мокінг (Mocking)?
/// Клас OrderCheckoutService залежить від 4 зовнішніх сервісів:
///   1) IInventoryService (склад у БД),
///   2) IPaymentGateway (реальний банківський API списання грошей з картки),
///   3) INotificationService (SMTP-сервер відправки листів),
///   4) IAuditLogger (система логування).
/// 
/// Під час модульного тесту ми НЕ ХОЧЕМО списувати реальні гроші з картки чи відправляти реальні Email!
/// Бібліотека Moq створює "об'єкти-імітатори" (Mock<T>), яким ми самі задаємо сценарій:
/// що повернути (Setup / ReturnsAsync), яку помилку імітувати (ThrowsAsync)
/// і як перевірити, чи дійсно наш сервіс викликав потрібні методи (Verify / Times).
/// </summary>
public class OrderCheckoutMoqStepByStepTests
{
    // КРОК 1: Оголошуємо обгортки Mock<T> для кожної зовнішньої залежності інтерфейсу
    private readonly Mock<IPaymentGateway> _paymentMock;
    private readonly Mock<IInventoryService> _inventoryMock;
    private readonly Mock<INotificationService> _notificationMock;
    private readonly Mock<IAuditLogger> _auditMock;

    // Система під тестуванням (System Under Test)
    private readonly OrderCheckoutService _sut;

    public OrderCheckoutMoqStepByStepTests()
    {
        // Створюємо екземпляри моків
        _paymentMock = new Mock<IPaymentGateway>();
        _inventoryMock = new Mock<IInventoryService>();
        _notificationMock = new Mock<INotificationService>();
        _auditMock = new Mock<IAuditLogger>();

        // ВАЖЛИВО: У конструктор сервісу передаємо властивість .Object кожного мока!
        // (_paymentMock — це "пульт керування" імітатором, а _paymentMock.Object — сам екземпляр IPaymentGateway)
        _sut = new OrderCheckoutService(
            _paymentMock.Object,
            _inventoryMock.Object,
            _notificationMock.Object,
            _auditMock.Object);
    }

    // =========================================================================
    // КРОК 2: Налаштування поведінки через Setup(...) та ReturnsAsync(...)
    // =========================================================================

    [Fact]
    public async Task Step02_SetupAndReturnsAsync_WhenStockAndPaymentSucceed_ReturnsCompletedOrder()
    {
        // Arrange
        var request = CreateSampleOrderRequest("buyer@example.ua", quantity: 2, unitPrice: 500m); // Разом 1000 грн

        // Налаштовуємо склад: на виклик ReserveStockAsync повернути true (товар є в наявності)
        _inventoryMock
            .Setup(x => x.ReserveStockAsync(request.Items, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Налаштовуємо платіжний шлюз: при списанні 1000 грн повернути ID транзакції "TX-MOQ-001"
        _paymentMock
            .Setup(x => x.ChargeAsync("buyer@example.ua", 1000m, It.IsAny<CancellationToken>()))
            .ReturnsAsync("TX-MOQ-001");

        // Act
        OrderCheckoutResponse response = await _sut.ProcessOrderAsync(request);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("TX-MOQ-001", response.TransactionId);
        Assert.Equal(1000m, response.TotalAmount);
    }

    // =========================================================================
    // КРОК 3: Гнучкі матчери аргументів: It.IsAny<T>(), It.Is<T>(предикат), It.IsInRange(...)
    // =========================================================================

    [Fact]
    public async Task Step03_ArgumentMatchers_ItIsAny_ItIsPredicate_ItIsInRange()
    {
        // Arrange
        var request = CreateSampleOrderRequest("vip@company.ua", quantity: 3, unitPrice: 400m); // 1200 грн

        // 1) It.IsAny<T>() — підходить будь-яке значення зазначеного типу
        _inventoryMock
            .Setup(x => x.ReserveStockAsync(
                It.IsAny<IEnumerable<OrderItemDto>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // 2) It.Is<T>(умова) та It.IsInRange(...) — спрацює лише якщо email закінчується на ".ua",
        //    а сума платежу знаходиться в діапазоні від 100 до 5000 грн!
        _paymentMock
            .Setup(x => x.ChargeAsync(
                It.Is<string>(email => email.EndsWith(".ua")),
                It.IsInRange(100m, 5000m, Moq.Range.Inclusive),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("TX-MATCHED-UA");

        // Act
        var response = await _sut.ProcessOrderAsync(request);

        // Assert
        Assert.Equal("TX-MATCHED-UA", response.TransactionId);
    }

    // =========================================================================
    // КРОК 4: Імітація збоїв через ThrowsAsync(...) та перевірка компенсуючої логіки
    // =========================================================================

    [Fact]
    public async Task Step04_ThrowsAsync_WhenPaymentGatewayFails_ReleasesStockAndThrowsPaymentFailedException()
    {
        // Arrange
        var request = CreateSampleOrderRequest("declined@example.ua", quantity: 1, unitPrice: 999m);

        // Склад успішно зарезервував товар
        _inventoryMock
            .Setup(x => x.ReserveStockAsync(It.IsAny<IEnumerable<OrderItemDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Імітуємо падіння банківського шлюзу через .ThrowsAsync(...)
        _paymentMock
            .Setup(x => x.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Банк відхилив транзакцію: недостатній ліміт."));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<PaymentFailedException>(() => _sut.ProcessOrderAsync(request));
        Assert.Contains("Банк відхилив транзакцію", ex.Message);

        // Перевіряємо, що сервіс виконав компенсуючу дію — повернув зарезервований товар на склад!
        _inventoryMock.Verify(
            x => x.ReleaseStockAsync(request.Items, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // =========================================================================
    // КРОК 5: Верифікація викликів через Verify(..., Times.Once / Times.Never)
    // =========================================================================

    [Fact]
    public async Task Step05_VerifyAndTimes_WhenOutOfStock_NeverCallsPaymentOrNotification()
    {
        // Arrange
        var request = CreateSampleOrderRequest("client@example.ua", quantity: 10, unitPrice: 100m);

        // Товарів на складі НЕМАЄ (повертаємо false)
        _inventoryMock
            .Setup(x => x.ReserveStockAsync(request.Items, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<OutOfStockException>(() => _sut.ProcessOrderAsync(request));

        // Перевіряємо, що резервування складу викликалося рівно 1 раз (Times.Once)
        _inventoryMock.Verify(
            x => x.ReserveStockAsync(request.Items, It.IsAny<CancellationToken>()),
            Times.Once);

        // Перевіряємо, що списання грошей і відправка Email НЕ викликалися ЖОДНОГО РАЗУ (Times.Never)!
        _paymentMock.Verify(
            x => x.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _notificationMock.Verify(
            x => x.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // =========================================================================
    // КРОК 6: Просунуті можливості Moq — Callback(...) та SetupSequence(...)
    // =========================================================================

    [Fact]
    public async Task Step06_Callback_CapturesAuditLogsPassedToMock()
    {
        // Arrange
        var request = CreateSampleOrderRequest("audit@example.ua", quantity: 1, unitPrice: 750m);
        var capturedLogs = new List<(string EventName, string Details)>();

        _inventoryMock
            .Setup(x => x.ReserveStockAsync(It.IsAny<IEnumerable<OrderItemDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _paymentMock
            .Setup(x => x.ChargeAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("TX-777");

        // .Callback(...) дозволяє "перехопити" аргументи, з якими наш сервіс викликав мок!
        _auditMock
            .Setup(x => x.LogAudit(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((eventName, details) =>
            {
                capturedLogs.Add((eventName, details));
            });

        // Act
        await _sut.ProcessOrderAsync(request);

        // Assert
        Assert.Single(capturedLogs);
        Assert.Equal("OrderCompleted", capturedLogs[0].EventName);
        Assert.Contains("TX-777", capturedLogs[0].Details);
    }

    [Fact]
    public async Task Step06_SetupSequence_WhenFirstAttemptFailsAndSecondSucceeds_CallsGatewayExactlyTwice()
    {
        // Arrange
        var request = CreateSampleOrderRequest("retry@example.ua", quantity: 1, unitPrice: 300m);

        _inventoryMock
            .Setup(x => x.ReserveStockAsync(It.IsAny<IEnumerable<OrderItemDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // SetupSequence задає різну поведінку на 1-й, 2-й і наступні виклики одного й того самого методу:
        // 1-ша спроба -> кидає помилку мережі,
        // 2-га спроба -> успішно повертає "TX-RETRY-OK"!
        _paymentMock
            .SetupSequence(x => x.ChargeAsync("retry@example.ua", 300m, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TimeoutException("Тимчасовий таймаут мережі при 1-й спробі"))
            .ReturnsAsync("TX-RETRY-OK");

        // Act
        var response = await _sut.ProcessOrderWithRetryAsync(request, maxPaymentAttempts: 2);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("TX-RETRY-OK", response.TransactionId);

        // Перевіряємо через Times.Exactly(2), що платіжний шлюз було викликано рівно 2 рази!
        _paymentMock.Verify(
            x => x.ChargeAsync("retry@example.ua", 300m, It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    private static OrderCheckoutRequest CreateSampleOrderRequest(string email, int quantity, decimal unitPrice)
    {
        return new OrderCheckoutRequest(
            OrderId: Guid.NewGuid(),
            CustomerEmail: email,
            Items: [new OrderItemDto("SKU-BOOK-01", quantity, unitPrice)]);
    }
}
