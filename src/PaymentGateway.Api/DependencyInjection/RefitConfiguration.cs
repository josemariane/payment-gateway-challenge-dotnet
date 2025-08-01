using Microsoft.Extensions.Options;
using PaymentGateway.Infrastructure.ExternalApis;
using PaymentGateway.Infrastructure.ExternalApis.AcquiringBank;

using Refit;

namespace PaymentGateway.Api.DependencyInjection;

public static class RefitConfiguration
{
    public static IServiceCollection ConfigureRefitInjection(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptionsWithValidateOnStart<AcquiringBankApiSettings>();
        services.AddRefitClient<IAcquiringBankApi>()
            .ConfigureHttpClient((provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptions<AcquiringBankApiSettings>>().Value;
                client.BaseAddress = settings.BaseUrl;
            })
            .AddStandardResilienceHandler(); //defaults should be tuned with real world data.

        return services;
    }
}