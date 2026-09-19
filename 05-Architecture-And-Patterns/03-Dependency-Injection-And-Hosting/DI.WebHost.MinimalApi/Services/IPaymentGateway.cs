namespace DI.WebHost.MinimalApi.Services;

public record PaymentResult(bool Success, string TransactionId, string Message);

public interface IPaymentGateway
{
    string ProviderName { get; }
    Task<PaymentResult> ProcessPaymentAsync(decimal amount, string currency, CancellationToken ct);
}

public class StripePaymentGateway : IPaymentGateway
{
    public string ProviderName => "Stripe";

    public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string currency, CancellationToken ct)
    {
        await Task.Delay(50, ct);
        return new PaymentResult(true, $"str_{Guid.NewGuid():N}", $"Оплата {amount} {currency} успішно здійснена через Stripe.");
    }
}

public class LiqPayPaymentGateway : IPaymentGateway
{
    public string ProviderName => "LiqPay";

    public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string currency, CancellationToken ct)
    {
        await Task.Delay(50, ct);
        return new PaymentResult(true, $"lp_{Guid.NewGuid():N}", $"Оплата {amount} {currency} успішно здійснена через LiqPay.");
    }
}
