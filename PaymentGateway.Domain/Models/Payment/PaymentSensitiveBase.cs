using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace PaymentGateway.Domain.Models.Payment;

[JsonSerializable(typeof(PaymentSensitiveBase),
    GenerationMode = JsonSourceGenerationMode.Serialization)]
[ExcludeFromCodeCoverage]
public abstract record PaymentSensitiveBase : PaymentBase
{
    [JsonPropertyName("card_number")]
    [Required]
    [RegularExpression("\\d{14,19}")]
    [MinLength(14)]
    [MaxLength(19)]
    public required string CardNumberSensitive { get; init; }

}