using System.Text.RegularExpressions;

using PaymentGateway.Domain.AcquiringBank;

namespace PaymentGateway.Domain.Models.Payment;

public record PaymentEntity
{
    private static readonly Regex CardRegex = new("\\d{14,19}");
    private static readonly Regex CvvRegex = new("\\d{3,4}");
    public Guid Id { get; } = Guid.CreateVersion7();
    public string CardNumberSensitive { get; set; } = String.Empty;

    public string CardNumberLastFour { get; set; } = String.Empty;

    public ExpiryDate ExpiryDate { get; init; }

    public required string Currency { get; init; }

    public uint Amount { get; init; }

    public required string Cvv { get; init; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Received;

    public void RegisterResponseStatusAndSanitize(PaymentResponseFromBank? response, bool isSuccessStatusCode)
    {
        Status = isSuccessStatusCode && response?.Authorized == true ? PaymentStatus.Authorized : PaymentStatus.Declined;
        SanitizeCardNumber();
    }

    public bool ValidateAndRejectIfInvalid()
    {
        if (ValidateCardNumber(CardNumberSensitive) &&
            ExpiryDate.IsInFuture() &&
            ValidateCurrencySupported(Currency) &&
            ValidateCvv(Cvv))
        {
            return true;
        }
        Status = PaymentStatus.Rejected;
        SanitizeCardNumber();
        return false;
    }

    private void SanitizeCardNumber()
    {
        CardNumberSensitive = string.Empty;
    }

    private static bool ValidateCardNumber(string? cardNumber) => cardNumber != null && CardRegex.IsMatch(cardNumber);

    private static bool ValidateCurrencySupported(string currency) => Currencies.SupportedCurrencies.Contains(currency);

    private static bool ValidateCvv(string cvv) => CvvRegex.IsMatch(cvv);
}