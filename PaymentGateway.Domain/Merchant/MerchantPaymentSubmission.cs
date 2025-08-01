using System.Text.Json.Serialization;

namespace PaymentGateway.Domain.Merchant;


[JsonSerializable(typeof(MerchantPaymentSubmission),
    GenerationMode = JsonSourceGenerationMode.Serialization)]
public record MerchantPaymentSubmission(
    [property: JsonPropertyName("card_number")]
    string CardNumber,
    [property: JsonPropertyName("expiry_month")]
    string ExpiryYear,
    [property: JsonPropertyName("expiry_year")]
    string ExpiryMonth,
    [property: JsonPropertyName("currency")]
    string Currency,
    [property: JsonPropertyName("amount")] uint Amount,
    [property: JsonPropertyName("cvv")] string Cvv)
{
    
}
