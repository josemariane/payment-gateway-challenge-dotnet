using PaymentGateway.Api.Services;

namespace PaymentGateway.Api;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
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