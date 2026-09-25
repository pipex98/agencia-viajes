using AgenciaViajes.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using PasswordHashing;

namespace AgenciaViajes.Infrastructure.Services
{
    public class PasswordHasherService(ILogger<PasswordHasherService> _logger) : IPasswordHasherService
    {
        public string Hash(string password)
        {
            _logger.LogInformation("Cifrando contraseña: {Contraseña}", password);

            return PasswordHasher.Hash(password);
        }

        public bool Validate(string password, string hashedPassword)
        {
            _logger.LogInformation("Validando contraseña: {Password} contra el hash: {HashedPassword}", password, hashedPassword);

            return PasswordHasher.Validate(password, hashedPassword);
        }
    }
}
