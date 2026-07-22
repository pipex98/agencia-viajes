
using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record AssignHabitacionCommand(UpsertHabitacionDto dto) : IRequest<UpsertHabitacionDto>;

    public class AssignHabitacionCommandHandler(IHotelRepository hotelRepository, ILogger<AssignHabitacionCommand> _logger) : IRequestHandler<AssignHabitacionCommand, UpsertHabitacionDto>
    {
        public async Task<UpsertHabitacionDto> Handle(AssignHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await hotelRepository.AssignHabitacionAsync(request.dto);
        }
    }
}
