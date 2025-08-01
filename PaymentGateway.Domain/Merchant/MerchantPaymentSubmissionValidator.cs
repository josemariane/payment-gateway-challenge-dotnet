using System.Text.RegularExpressions;

using PaymentGateway.Domain.Payment;

namespace PaymentGateway.Domain.Merchant;

public static class MerchantPaymentSubmissionValidator
{
    private static Regex _cardRegex = new Regex("\\d{14,19}");
    private static Regex _cvvRegex = new Regex("\\d{3,4}");

    public static bool ValidatePaymentSubmission(MerchantPaymentSubmission submission)
    {
        return ValidateCardNumber(submission.CardNumber) &&
               ValidateExpiryDate(submission.ExpiryMonth, submission.ExpiryYear) &&
               ValidateCurrencySupported(submission.Currency) &&
               ValidateCvv(submission.Cvv);
    }

    private static bool ValidateCardNumber(string cardNumber) => _cardRegex.IsMatch(cardNumber);

    private static bool ValidateExpiryDate(string expiryMonth, string expiryYear)
    {
        var today = DateTime.Today;
        return (Convert.ToInt32(expiryMonth), Convert.ToInt32(expiryYear)) switch
        {
            var (_, year) when year > today.Year => true,
            var (month, year) when year == today.Year => month >= today.Month,
            var (_, year) when year < today.Year => false,
        };
    }

    private static bool ValidateCurrencySupported(string currency) => Currency.SupportedCurrencies.Contains(currency);

    private static bool ValidateCvv(string cvv) => _cvvRegex.IsMatch(cvv);
}