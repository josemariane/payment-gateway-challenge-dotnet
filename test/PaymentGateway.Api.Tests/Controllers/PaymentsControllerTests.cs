using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Services;
using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Mappers;

namespace PaymentGateway.Api.Tests.Controllers;

public class PaymentsControllerTests(WebApplicationFactoryFixture fixture) : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly Random _random = new();
    private readonly WebApplicationFactory<Program> _factory = fixture.Factory;

    [Fact]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange
        var payment = new PaymentRequestFromMerchant
        {
            ExpiryYear = _random.Next(2023, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = (uint)_random.Next(1, 10000),
            CardNumberSensitive = new string(_random.GetItems("123456789".ToCharArray(), 16)),
            Currency = "GBP",
            Cvv = "123",
        }.ToPaymentEntity();
        var paymentsRepository = new PaymentsRepository();
        paymentsRepository.Add(payment);

        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        var client = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services => services
                    .AddSingleton<IPaymentsRepository>(paymentsRepository)))
            .CreateClient();

        // Act
        var token = TestContext.Current.CancellationToken;
        var response = await client.GetAsync($"/api/Payments/{payment.Id}", token);
        var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponseToMerchant>(cancellationToken: token);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paymentResponse);
    }

    [Fact]
    public async Task Returns404IfPaymentNotFound()
    {
        // Arrange
        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        var client = webApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Payments/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReturnsInternalServerErrorWhenExceptionOccurs()
    {
        // Arrange
        var paymentIntermediationService = Substitute.For<IPaymentIntermediationService>();
        paymentIntermediationService.Configure()
            .ExecutePaymentOrder(Arg.Any<PaymentRequestFromMerchant>())
            .ThrowsAsyncForAnyArgs(new Exception("simulated exception"));

        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        var client = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services => services
                    .AddSingleton(paymentIntermediationService)))
            .CreateClient();

        // Act
        var response = await client.PostAsync(("/api/Payments"),
            JsonContent.Create(new PaymentRequestFromMerchant
            {
                ExpiryYear = 1,
                ExpiryMonth = 1,
                Currency = "GBP",
                Amount = 0,
                Cvv = "123",
                CardNumberSensitive = "12345678912345"
            }), TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task ReturnsRejectedWhenInvalidData()
    {
        var client = _factory.CreateClient();
        var fromMerchant = new PaymentRequestFromMerchant
        {
            ExpiryYear = 1,
            ExpiryMonth = 1,
            Currency = "GBP",
            Amount = 0,
            Cvv = "123",
            CardNumberSensitive = "12345678912345",
        };
        var response = await client.PostAsync(("/api/Payments"), JsonContent.Create(fromMerchant with { CardNumberSensitive = "12345" }),
            TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response = await client.PostAsync(("/api/Payments"), JsonContent.Create(fromMerchant with { ExpiryYear = DateTime.Today.AddYears(-1).Year }),
            TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}