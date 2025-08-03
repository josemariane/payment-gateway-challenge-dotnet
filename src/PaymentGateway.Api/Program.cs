using PaymentGateway.Api.DependencyInjection;

namespace PaymentGateway.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        builder.Services.ConfigureAspNetHost();
        builder.Services.ConfigureLocalServices();
        builder.Services.ConfigureRefitInjection();
        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/openapi/v1.json", "v1");
            });
            app.UseHttpLogging();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseRequestDecompression();
        app.UseResponseCompression();

        app.Run();
    }
}