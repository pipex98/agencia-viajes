using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace AgenciaViajes.API.Tests
{
    public class AgenciaViajesApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ValidateLifetime = false;
                    options.TokenValidationParameters.RequireSignedTokens = false;
                });
            });
        }
    }
}
