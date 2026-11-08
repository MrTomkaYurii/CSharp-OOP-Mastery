using FluentAssertions;
using NSubstitute;
using Testing.Core.Domain.Services;
using Xunit;

namespace Testing.UnitTests;

public class OrderCheckoutServiceTests
{
    private readonly IPaymentGateway _paymentGateway = Substitute.For<IPaymentGateway>();
    private readonly IInventoryService _inventoryService = Substitute.For<IInventoryService>();
    private readonly INotificationService _notificationService = Substitute.For<INotificationService>();
    private readonly OrderCheckoutService _sut; // System Under Test

    public OrderCheckoutServiceTests()
    {
        _sut = new OrderCheckoutService(_paymentGateway, _inventoryService, _notificationService);
    }

    [Fact]
    public async Task ProcessOrder_WhenStockAvailableAndPaymentSucceeds_ReturnsSuccessAndNotifiesCustomer()
    {
        // Arrange
        var request = new OrderCheckoutRequest(
            OrderId: Guid.NewGuid(),
            CustomerEmail: "customer@example.ua",
            Items:
            [
                new OrderItemDto("SKU-01", 2, 500m),
                new OrderItemDto("SKU-02", 1, 1200m)
            ]);

        decimal expectedTotal = 2200m;
        string expectedTxId = "tx_987654";

        _inventoryService.ReserveStockAsync(request.Items, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        _paymentGateway.ChargeAsync(request.CustomerEmail, expectedTotal, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedTxId));

        // Act
        var response = await _sut.ProcessOrderAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.TransactionId.Should().Be(expectedTxId);
        response.TotalAmount.Should().Be(expectedTotal);

        // Перевіряємо виклики залежностей через NSubstitute
        await _inventoryService.Received(1).ReserveStockAsync(request.Items, Arg.Any<CancellationToken>());
        await _paymentGateway.Received(1).ChargeAsync(request.CustomerEmail, expectedTotal, Arg.Any<CancellationToken>());
        await _notificationService.Received(1).SendOrderConfirmationAsync(request.CustomerEmail, request.OrderId.ToString(), expectedTotal, Arg.Any<CancellationToken>());
        await _inventoryService.DidNotReceive().ReleaseStockAsync(Arg.Any<IEnumerable<OrderItemDto>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessOrder_WhenStockUnavailable_ThrowsOutOfStockException_AndDoesNotChargeCard()
    {
        // Arrange
        var request = new OrderCheckoutRequest(
            OrderId: Guid.NewGuid(),
            CustomerEmail: "client@example.ua",
            Items: [new OrderItemDto("OUT-OF-STOCK", 1, 500m)]);

        _inventoryService.ReserveStockAsync(request.Items, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // Act
        Func<Task> act = async () => await _sut.ProcessOrderAsync(request);

        // Assert
        await act.Should().ThrowAsync<OutOfStockException>()
            .WithMessage("*Недостатньо залишків*");

        // Платіжний шлюз та сервіс сповіщень взагалі НЕ повинні викликатися
        await _paymentGateway.DidNotReceive().ChargeAsync(Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<CancellationToken>());
        await _notificationService.DidNotReceive().SendOrderConfirmationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessOrder_WhenPaymentFails_ReleasesReservedStock_AndThrowsPaymentFailedException()
    {
        // Arrange
        var request = new OrderCheckoutRequest(
            OrderId: Guid.NewGuid(),
            CustomerEmail: "bad-card@example.ua",
            Items: [new OrderItemDto("SKU-99", 1, 999m)]);

        _inventoryService.ReserveStockAsync(request.Items, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        _paymentGateway.ChargeAsync(request.CustomerEmail, 999m, Arg.Any<CancellationToken>())
            .Returns<Task<string>>(_ => throw new InvalidOperationException("Картка відхилена банком-емітентом."));

        // Act
        Func<Task> act = async () => await _sut.ProcessOrderAsync(request);

        // Assert
        await act.Should().ThrowAsync<PaymentFailedException>()
            .WithMessage("*Помилка проведення оплати: Картка відхилена*");

        // Перевіряємо спрацювання компенсуючої дії: повернення зарезервованого товару на склад!
        await _inventoryService.Received(1).ReleaseStockAsync(request.Items, Arg.Any<CancellationToken>());
        await _notificationService.DidNotReceive().SendOrderConfirmationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<CancellationToken>());
    }
}
