using FluentAssertions;

using JetBrains.Annotations;

using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Mappers;
using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Api.Tests.Models.Payment;

[TestSubject(typeof(PaymentEntity))]
public class PaymentEntityTest
{
    private static readonly string cardNumberSensitive = "123456789";

    private readonly PaymentEntity _payment = new PaymentRequestFromMerchant
    {
        ExpiryYear = DateTime.Today.Year + 1,
        ExpiryMonth = DateTime.Today.Month,
        Amount = 1234,
        CardNumberSensitive = cardNumberSensitive,
        Currency = "GBP",
        Cvv = "123",
    }.ToPaymentEntity();

    [Fact]
    public void ValidateAndRejectShouldSetStatusToRejectedWhenBadData()
    {
        _payment.ValidateAndRejectIfInvalid();
        _payment.Status.Should().Be(PaymentStatus.Rejected);
    }

    [Fact]
    public void CardNumberLastFourShouldBeSetCorrectly()
    {
        _payment.CardNumberLastFour.Should().Be("6789");
    }
}