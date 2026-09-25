using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces;

namespace AgenciaViajes.Application.Commands.Habitaciones
{
    public record EnableHabitacionCommand(int id) : IRequest<Unit>;

    public class EnableHabitacionCommandHandler(IHabitacionRepository habitacionRepository, 
    IUnitOfWork unitOfWork, ILogger<EnableHabitacionCommand> _logger) : IRequestHandler<EnableHabitacionCommand, Unit>
    {
        public async Task<Unit> Handle(EnableHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var habitacion = await habitacionRepository.ObtenerHabitacionPorIdAsync(request.id);

            habitacion.Habilitar();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Habitacion habilitada exitosamente con el ID: {IdHabitacion}", habitacion.IdHabitacion);

            return Unit.Value;
        }
    }

}
