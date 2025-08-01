using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using PaymentGateway.Api.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureAspNetHost(builder.Configuration);
builder.Services.ConfigureRefitInjection(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpLogging();
}
app.UseHealthChecks(new PathString("/healthcheck"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseRequestDecompression();
app.UseResponseCompression();

app.Run();
