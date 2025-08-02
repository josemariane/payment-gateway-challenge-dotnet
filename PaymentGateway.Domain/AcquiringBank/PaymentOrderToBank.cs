using System.Text.Json.Serialization;

using JetBrains.Annotations;

using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Domain.AcquiringBank;

[JsonSerializable(typeof(PaymentOrderToBank),
    GenerationMode = JsonSourceGenerationMode.Serialization)]
public record PaymentOrderToBank : PaymentSensitiveBase
{
    [JsonPropertyName("expiry_date")]
    [UsedImplicitly]
    public required string ExpiryDate { get; init; }
}