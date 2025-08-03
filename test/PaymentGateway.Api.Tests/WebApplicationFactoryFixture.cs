using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Infrastructure.ExternalApis.AcquiringBank;

namespace PaymentGateway.Api.Tests;

[UsedImplicitly]
public class WebApplicationFactoryFixture : IDisposable, IAsyncDisposable
{
    public WebApplicationFactory<Program> Factory { get; } = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(b =>
            b.ConfigureTestServices(s =>
                s.Configure<AcquiringBankApiSettings>(settings => settings.BaseUrl = new Uri("http://localhost:8080"))));

    public void Dispose() => Factory.Dispose();

    public async ValueTask DisposeAsync() => await Factory.DisposeAsync();
}