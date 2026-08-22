using AgenciaViajes.API;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.OpenTelemetry()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/log.txt",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

Log.Information("Iniciando la AgenciaViajes API...");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAppDI(builder.Configuration, builder.Environment, builder.Logging);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("AgenciaViajes API")
        .WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Http);
    });
}

app.UseHttpsRedirection();

app.UseHsts();

app.UseRateLimiter();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
