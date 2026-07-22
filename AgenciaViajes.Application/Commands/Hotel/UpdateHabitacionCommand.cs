
using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record UpdateHabitacionCommand(int id, UpsertHabitacionDto dto) : IRequest<UpsertHabitacionDto>;

    public class UpdateHabitacionCommandHandler(IHabitacionRepository habitacionRepository, ILogger<UpdateHabitacionCommand> _logger) : IRequestHandler<UpdateHabitacionCommand, UpsertHabitacionDto>
    {
        public async Task<UpsertHabitacionDto> Handle(UpdateHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await habitacionRepository.UpdateHabitacionAsync(request.id, request.dto);
        }
    }
}
