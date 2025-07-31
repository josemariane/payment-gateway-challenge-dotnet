namespace PaymentGateway.Domain.Payment;


/// <summary>
/// Represents a payment with complete user data
/// </summary>
public record struct CompletePayment
{
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public int Cvv { get; set; }
}