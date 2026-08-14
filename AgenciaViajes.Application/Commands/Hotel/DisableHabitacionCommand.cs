using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record DisableHabitacionCommand(int id) : IRequest<bool>;

    public class DisableHabitacionCommandHandler(IHabitacionRepository habitacionRepository, ILogger<DisableHabitacionCommand> _logger) : IRequestHandler<DisableHabitacionCommand, bool>
    {
        public async Task<bool> Handle(DisableHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await habitacionRepository.DesabilitarHabitacionAsync(request.id);
        }
    }

}
