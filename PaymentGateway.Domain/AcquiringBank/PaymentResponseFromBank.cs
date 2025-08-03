using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace PaymentGateway.Domain.AcquiringBank;

[JsonSerializable(typeof(PaymentResponseFromBank), GenerationMode = JsonSourceGenerationMode.Serialization)]
public record PaymentResponseFromBank(
    [property: JsonPropertyName("authorized")]
    bool Authorized,
    [property: JsonPropertyName("authorization_code")]
    [UsedImplicitly]
    Guid? AuthorizationCode);