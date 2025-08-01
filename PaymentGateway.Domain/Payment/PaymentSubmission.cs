using System.Text.Json.Serialization;

namespace PaymentGateway.Domain.Payment;


/// <summary>
/// Represents a payment with complete user data, including card number
/// </summary>
[JsonSerializable(typeof(PaymentSubmission),
    GenerationMode = JsonSourceGenerationMode.Serialization)]
public record PaymentSubmission(
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