using JetBrains.Annotations;

using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.DependencyInjection;

public static class ServiceExtensions
{
    [UsedImplicitly]
    public static IServiceCollection ConfigureLocalServices(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentsRepository, PaymentsRepository>();
        services.AddSingleton<IPaymentIntermediationService, PaymentIntermediationService>();
        return services;
    }
}