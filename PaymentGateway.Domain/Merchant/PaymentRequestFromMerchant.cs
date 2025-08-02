using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Domain.Merchant;

[JsonSerializable(typeof(PaymentRequestFromMerchant),
    GenerationMode = JsonSourceGenerationMode.Serialization)]
public record PaymentRequestFromMerchant : PaymentSensitiveBase
{
    [JsonPropertyName("expiry_year")]
    [Required]
    [Range(1, 9999)]
    public required int ExpiryYear { get; init; }

    [JsonPropertyName("expiry_month")]
    [Required]
    [Range(1, 12)]
    public required int ExpiryMonth { get; init; }
}