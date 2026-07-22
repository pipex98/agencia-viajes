using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Habitacion;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Huesped
{
    public record AddHuespedCommand(AddHuespedDto dto) : IRequest<AddHuespedDto>;

    public class AddHuespedCommandHandler(IHuespedRepository huespedRepository, ILogger<AddHuespedCommand> _logger) : IRequestHandler<AddHuespedCommand, AddHuespedDto>
    {
        public async Task<AddHuespedDto> Handle(AddHuespedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await huespedRepository.AddHuespedAsync(request.dto);
        }
    }

}
