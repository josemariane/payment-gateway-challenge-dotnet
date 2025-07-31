using System.Text.Json.Serialization;

namespace PaymentGateway.Domain.AcquiringBank;

[JsonSerializable(typeof(PaymentSubmissionRequest),
    GenerationMode = JsonSourceGenerationMode.Serialization)]
public record PaymentSubmissionRequest(
    [property: JsonPropertyName("card_number")]
    string CardNumber,
    [property: JsonPropertyName("expiry_date")]
    string ExpiryDate,
    [property: JsonPropertyName("currency")]
    string Currency,
    [property: JsonPropertyName("amount")] 
    int Amount,
    [property: JsonPropertyName("cvv")] 
    string Cvv);