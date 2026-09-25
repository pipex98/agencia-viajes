
using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Commands.Habitaciones
{
    public record AssignHabitacionCommand(UpsertHabitacionDto dto) : IRequest<Unit>;

    public class AssignHabitacionCommandHandler(IHabitacionRepository habitacionRepository, 
    IHotelRepository hotelRepository, ITipoHabitacionRepository tipoHabitacionRepository, 
    IUnitOfWork unitOfWork, ILogger<AssignHabitacionCommand> _logger) : IRequestHandler<AssignHabitacionCommand, Unit>
    {
        public async Task<Unit> Handle(AssignHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var hotel = await hotelRepository.ObtenerHotelPorId(request.dto.IdHotel);

            var tipoHabitacion = await tipoHabitacionRepository.ObtenerTipoHabitacionPorIdAsync(request.dto.IdTipoHabitacion);

            var habitacion = Habitacion.Crear(
                hotel, 
                tipoHabitacion, 
                request.dto.CostoBase, 
                request.dto.Impuestos, 
                request.dto.CantidadHuespedes, 
                request.dto.Ubicacion
            );

            habitacionRepository.Crear(habitacion);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Habitacion creada exitosamente con el ID: {IdHabitacion}", habitacion.IdHabitacion);

            return Unit.Value;
        }
    }
}
