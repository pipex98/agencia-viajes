using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AgenciaViajes.API.Tests
{
    public static class Utils
    {
        public static string GetJwtToken()
        {
            var token = new JwtSecurityToken(
                issuer: "test",
                audience: "test",
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: null,
                claims:
                [
                    new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, "test@example.com")
                ]
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
