using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Reserva
{
    public record AddReservaCommand(AddReservaDto dto) : IRequest<AddReservaDto>;

    public class AddReservaCommandHandler(IHabitacionRepository habitacionRepository, ILogger<AddReservaCommand> _logger) : IRequestHandler<AddReservaCommand, AddReservaDto>
    {
        public async Task<AddReservaDto> Handle(AddReservaCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await habitacionRepository.ReservarHabitacion(request.dto);
        }
    }
}

