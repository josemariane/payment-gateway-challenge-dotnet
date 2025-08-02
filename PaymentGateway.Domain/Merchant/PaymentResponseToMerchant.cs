using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Domain.Merchant;

[JsonSerializable(typeof(PaymentRequestFromMerchant),
    GenerationMode = JsonSourceGenerationMode.Serialization)]
public record PaymentResponseToMerchant : PaymentBase
{
    [JsonPropertyName("id")] public required Guid Id { get; init; }

    [JsonPropertyName("status")] public required PaymentStatus Status { get; init; }

    [JsonPropertyName("last_four_card_digits")]
    public required string CardNumberLastFour { get; init; }

    [JsonPropertyName("expiry_year")]
    [Required]
    [Range(1, 9999)]
    public required int ExpiryYear { get; init; }

    [JsonPropertyName("expiry_month")]
    [Required]
    [Range(1, 12)]
    public required int ExpiryMonth { get; init; }

}