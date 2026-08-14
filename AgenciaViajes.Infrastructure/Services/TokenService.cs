using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using AgenciaViajes.Application.Interfaces.Services;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Infrastructure.Services
{
    public class TokenService(IConfiguration configuration, ILogger<TokenService> _logger) : ITokenService
    {
        public string Create(Huesped huesped)
        {
            _logger.LogInformation("Iniciando la generación de token para el huésped con ID: {HuespedId}", huesped.IdHuesped);

            var secreyKey = configuration["Jwt:Secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secreyKey!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, huesped.IdHuesped.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, huesped.CorreoElectronico)
                ]),

                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            _logger.LogInformation("Descriptor de token configurado con éxito para {HuespedEmail}. Expiración establecida en {Expiracion} minutos.", huesped.CorreoElectronico, configuration["Jwt:ExpirationInMinutes"]);

            var handler = new JsonWebTokenHandler();

            _logger.LogInformation("Token generado exitosamente para el huésped {HuespedId}.", huesped.IdHuesped);

            return handler.CreateToken(tokenDescriptor);
        }
    }
}
