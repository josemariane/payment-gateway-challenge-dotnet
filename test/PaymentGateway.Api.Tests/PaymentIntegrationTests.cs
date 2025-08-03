using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

using PaymentGateway.Api.Services;
using PaymentGateway.Domain.Merchant;
using PaymentGateway.Domain.Models.Payment;

namespace PaymentGateway.Api.Tests;

public class PaymentIntegrationTests(WebApplicationFactoryFixture factory) : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly WebApplicationFactory<Program> _factory = factory.Factory;
    private readonly Random _random = new();
    private readonly HttpClient _client = factory.Factory.CreateClient();

    [Theory]
    [InlineData("2", PaymentStatus.Declined)]
    [InlineData("3", PaymentStatus.Authorized)]
    public async Task When_CardValid_RequestShouldBeAccepted(string cardLastNumber, PaymentStatus expectedStatus)
    {
        var token = TestContext.Current.CancellationToken;
        var jsonContent = JsonContent.Create(new PaymentRequestFromMerchant
        {
            ExpiryYear = _random.Next(2026, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = (uint)_random.Next(1, 10000),
            CardNumberSensitive = new string(_random.GetItems("123456789".ToCharArray(), 15)) + cardLastNumber,
            Currency = "GBP",
            Cvv = "123",
        });

        //assert payment is submitted
        var response = await _client.PostAsync(new Uri("/api/Payments"), jsonContent, token);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var merchantResponse = await response.Content.ReadFromJsonAsync<PaymentResponseToMerchant>(token);
        merchantResponse?.Id.Should().NotBeEmpty();
        merchantResponse?.Status.Should().Be(expectedStatus);

        //assert payment has been stored and is available on GET
        response = await _client.GetAsync(new Uri($"/api/Payments/{merchantResponse?.Id}"), token);
        response.IsSuccessStatusCode.Should().BeTrue();
        var getOrderBack = await response.Content.ReadFromJsonAsync<PaymentResponseToMerchant>(token);
        merchantResponse.Should().BeEquivalentTo(getOrderBack);
    }

    [Fact]
    public async Task When_PostPaymentThrowsException_ControllerShouldReturnError()
    {
        var factory = _factory.WithWebHostBuilder(b =>
            b.ConfigureServices(s =>
                s.AddSingleton(_ =>
                {
                    var repository = Substitute.For<IPaymentsRepository>();
                    repository.Configure()
                        .GetForMerchant(Arg.Any<Guid>())
                        .ThrowsForAnyArgs(new Exception("test exception"));
                    return repository;
                })));
        var client = factory.CreateClient();
        var token = TestContext.Current.CancellationToken;
        var response = await client.GetAsync(new Uri($"/api/Payments/{Guid.NewGuid()}"), token);
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}