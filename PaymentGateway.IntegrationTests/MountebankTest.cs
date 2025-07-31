using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Payment;
using PaymentGateway.Infrastructure.ExternalApis;

using Refit;

namespace PaymentGateway.IntegrationTests;

public class MountebankTest : IAsyncLifetime
{

    private IContainer _container = new ContainerBuilder()
        .WithImage("bbyars/mountebank:2.8.1")
        .WithPortBinding(2525, 2525)
        .WithPortBinding(8080, 8080)
        .WithEntrypoint("mb")
        .WithCommand("--configfile",  "/imposters/bank_simulator.ejs", "--allowInjection")
        .WithBindMount(Path.GetFullPath("./Resources"), "/imposters")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
        .WithCleanUp(true)
        .Build();

    [Fact]
    public async Task When_BasicRequestSent_ApiShouldReturnAuthorized()
    {
        var bankApi = RestService.For<IAcquiringBankApi>("http://localhost:8080/");
        var response = await bankApi.PostPayment(new PaymentSubmissionRequest("2222405343248877", "04/2026", "GBP", 2000, "123"));
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        Assert.True(response.Content.Authorized);
        Assert.NotStrictEqual(Guid.Empty, response.Content.AuthorizationCode);
    }
    
    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();
    }
    public async ValueTask DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }


}