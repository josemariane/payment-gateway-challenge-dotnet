using JetBrains.Annotations;

using FluentAssertions;

using PaymentGateway.Domain.Models;

namespace PaymentGateway.Api.Tests.Models;

[TestSubject(typeof(ExpiryDate))]
public class ExpiryDateTest
{

    [Theory]
    [InlineData(-1, 1, false)]
    [InlineData(13, 1, false)]
    [InlineData(12, 0, false)]
    [InlineData(12, 10000, false)]
    [InlineData(12, 9999, true)]
    public void TryParseShouldReturnExpected(int month, int year, bool expected)
    {
        ExpiryDate.TryParse(month, year, out _).Should().Be(expected);
    }

    [Theory]
    [InlineData(1, 1999, false)]
    [InlineData(12, 3000, true)]
    [InlineData(12, 9999, true)]
    public void IsInFutureShouldReturnExpected(int month, int year, bool expected)
    {
        ExpiryDate.TryParse(month, year, out var expiry);
        expiry?.IsInFuture().Should().Be(expected);

        ExpiryDate.TryParse(DateTime.Today.Month, DateTime.Today.Year, out expiry);

        expiry?.IsInFuture().Should().BeTrue();
    }
}