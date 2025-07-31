using System.Text.Json.Serialization;

namespace PaymentGateway.Domain.AcquiringBank;

[JsonSerializable(typeof(SubmittedPaymentResponse), GenerationMode = JsonSourceGenerationMode.Serialization)]
public record SubmittedPaymentResponse(
    [property: JsonPropertyName("authorized")] bool Authorized,
    [property: JsonPropertyName("authorization_code")] Guid AuthorizationCode);