using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace PaymentGateway.Domain.Models.Payment;

[ExcludeFromCodeCoverage]
public abstract record PaymentBase
{
    [JsonPropertyName("currency")]
    [Required]
    [MinLength(3)]
    [MaxLength(3)]
    [Description("[Currency ISO code](https://www.checkout.com/docs/resources/codes/currency-codes)")]
    public required string Currency { get; init; }

    [JsonPropertyName("amount")]
    [Description("Amount in the [minor currency unit](https://www.checkout.com/docs/resources/calculating-the-amount)")]

    public required uint Amount { get; init; }

    [JsonPropertyName("cvv")]
    [Required]
    [RegularExpression("\\d{3,4}")]
    [MinLength(3)]
    [MaxLength(4)]
    public required string Cvv { get; init; }
}