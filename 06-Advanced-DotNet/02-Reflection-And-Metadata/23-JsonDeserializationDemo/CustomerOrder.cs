namespace JsonDeserializationDemo;

/// <summary>
/// Цільова модель клієнтського замовлення, у яку ми десеріалізуємо вхідний JSON.
/// </summary>
public class CustomerOrder
{
    [JsonField("order_id")]
    public int OrderId { get; set; }

    [JsonField("customer_name")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonField("total_amount")]
    public double TotalAmount { get; set; }

    [JsonField("is_express_delivery")]
    public bool IsExpressDelivery { get; set; }

    [JsonField("items")]
    public string[] Items { get; set; } = Array.Empty<string>();

    // Захищене поле: навіть якщо зловмисник передасть його у JSON, десеріалізатор його проігнорує
    [JsonExclude]
    public bool IsAdminVerified { get; set; }
}
