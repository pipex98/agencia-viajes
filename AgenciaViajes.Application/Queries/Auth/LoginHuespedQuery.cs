using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Usuario;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces.Services;

namespace AgenciaViajes.Application.Queries.Auth
{
    public record LoginHuespedQuery(LoginDto dto) : IRequest<string>;

    public class LoginHuespedQueryHandler(IHuespedRepository huespedRepository, 
    IPasswordHasherService passwordHasherService, ITokenService tokenService, 
    ILogger<LoginHuespedQuery> _logger): IRequestHandler<LoginHuespedQuery, string>
    {
        public async Task<string> Handle(LoginHuespedQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var huesped = await huespedRepository.ObtenerHuespedPorCorreoElectronicoAsync(request.dto.CorreoElectronico!);

            if (huesped != null && passwordHasherService.Validate(request.dto.Contraseña, huesped.Contraseña))
            {
                _logger.LogInformation("Creando token para el huesped: {Nombres} {Apellidos}", huesped.Nombres, huesped.Apellidos);

                return tokenService.Create(huesped);
            }

            _logger.LogWarning("La autenticacion del huesped fallo. usuario o clave no validos");

            throw new KeyNotFoundException("Usuario o clave no validos");
        }
    }
}
