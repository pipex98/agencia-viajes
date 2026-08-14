using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Threading.RateLimiting;
using AgenciaViajes.API.Middlewares;
using AgenciaViajes.Application;
using AgenciaViajes.Domain;
using AgenciaViajes.Infrastructure;

namespace AgenciaViajes.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddProblemDetails(configure =>
            {
                configure.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                };
            });

            services.AddExceptionHandler<GlobalExceptionMiddleware>();

            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                {
                    options.Window = TimeSpan.FromSeconds(10);
                    options.PermitLimit = 3;
                    options.QueueLimit = 3;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer(async (document, context, cancellationToken) =>
                {
                    var authSchemeProvider = context.ApplicationServices.GetRequiredService<IAuthenticationSchemeProvider>();

                    var authenticationSchemes = await authSchemeProvider.GetAllSchemesAsync();

                    if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
                    {
                        var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                        {
                            ["Bearer"] = new OpenApiSecurityScheme
                            {
                                Type = SecuritySchemeType.Http,
                                Scheme = "bearer",
                                In = ParameterLocation.Header,
                                BearerFormat = "Json Web Token"
                            }
                        };

                        document.Components ??= new OpenApiComponents();
                        document.Components.SecuritySchemes = securitySchemes;
                    }
                });
            });

            services.AddApplicationDI()
                .AddInfrastructureDI(configuration)
                .AddDomainDI(configuration);

            return services;
        }
    }
}
