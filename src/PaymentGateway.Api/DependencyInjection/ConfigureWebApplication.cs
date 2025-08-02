using JetBrains.Annotations;

using Microsoft.Extensions.Logging.Configuration;


namespace PaymentGateway.Api.DependencyInjection;

public static class ConfigureWebApplication
{
    [UsedImplicitly]
    public static IServiceCollection ConfigureAspNetHost(this IServiceCollection services)
    {
        services.AddLogging(b =>
        {
            b.AddConfiguration();
        });
        services.AddHttpLogging();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddOpenApi();
        services.AddRequestDecompression();
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        return services;
    }
}