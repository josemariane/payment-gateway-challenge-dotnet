using PaymentGateway.Api.Services;

using Microsoft.Extensions.Logging.Configuration;

using OpenTelemetry.Trace;


namespace PaymentGateway.Api.DependencyInjection;

public static class ConfigureWebApplication
{
    public static IServiceCollection ConfigureAspNetHost(this IServiceCollection services, IConfiguration config)
    {
        services.AddLogging(b =>
        {
            b.AddConfiguration();
        });
        services.AddOpenTelemetry()
            .WithLogging()
            .WithMetrics()
            .WithTracing(b => b.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter());
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSingleton<PaymentsRepository>();
        services.AddRequestDecompression();
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        return services;
    }
}