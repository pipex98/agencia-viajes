using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Habitacion;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Application.Interfaces.Services;

namespace AgenciaViajes.Application.Commands.Huespedes
{
    public record AddHuespedCommand(AddHuespedDto dto) : IRequest<Unit>;

    public class AddHuespedCommandHandler(IHuespedRepository huespedRepository, 
    ITipoDocumentoRepository tipoDocumentoRepository, IGeneroRepository generoRepository,
    IPasswordHasherService passwordHasherService, IUnitOfWork unitOfWork, ILogger<AddHuespedCommand> _logger) 
    : IRequestHandler<AddHuespedCommand, Unit>
    {
        public async Task<Unit> Handle(AddHuespedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var tipoDocumento = await tipoDocumentoRepository.ObtenerTipoDocumentoPorIdAsync(request.dto.IdTipoDocumento);

            var genero = await generoRepository.ObtenerGeneroPorIdAsync(request.dto.IdGenero);

            var huesped = Huesped.Crear(
                genero,
                tipoDocumento,
                request.dto.Nombres,
                request.dto.Apellidos,
                request.dto.FechaNacimiento,
                request.dto.NumeroDocumento,
                request.dto.CorreoElectronico,
                passwordHasherService.Hash(request.dto.Contraseña),
                request.dto.Telefono
            );

            huespedRepository.Crear(huesped);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Huesped creado exitosamente con el ID: {IdHuesped}", huesped.IdHuesped);

            return Unit.Value;
        }
    }

}
