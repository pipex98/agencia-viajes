using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record EnableHabitacionCommand(int id) : IRequest<bool>;

    public class EnableHabitacionCommandHandler(IHabitacionRepository habitacionRepository, ILogger<EnableHabitacionCommand> _logger) : IRequestHandler<EnableHabitacionCommand, bool>
    {
        public async Task<bool> Handle(EnableHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await habitacionRepository.HabilitarHabitacionAsync(request.id);
        }
    }

}
