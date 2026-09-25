using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces;

namespace AgenciaViajes.Application.Commands.Habitaciones
{
    public record DisableHabitacionCommand(int id) : IRequest<Unit>;

    public class DisableHabitacionCommandHandler(IHabitacionRepository habitacionRepository,
    IUnitOfWork unitOfWork, ILogger<DisableHabitacionCommand> _logger) : IRequestHandler<DisableHabitacionCommand, Unit>
    {
        public async Task<Unit> Handle(DisableHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var habitacion = await habitacionRepository.ObtenerHabitacionPorIdAsync(request.id);

            habitacion.Desabilitar();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Habitacion deshabilitada exitosamente con el ID: {IdHabitacion}", habitacion.IdHabitacion);

            return Unit.Value;
        }
    }

}
