
using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces;

namespace AgenciaViajes.Application.Commands.Habitaciones
{
    public record UpdateHabitacionCommand(int id, UpsertHabitacionDto dto) : IRequest<Unit>;

    public class UpdateHabitacionCommandHandler(IHabitacionRepository habitacionRepository,
    IHotelRepository hotelRepository, ITipoHabitacionRepository tipoHabitacionRepository,
    IUnitOfWork unitOfWork, ILogger<UpdateHabitacionCommand> _logger) : IRequestHandler<UpdateHabitacionCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateHabitacionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var habitacion = await habitacionRepository.ObtenerHabitacionPorIdAsync(request.id);

            var hotel = await hotelRepository.ObtenerHotelPorId(request.dto.IdHotel);

            var tipoHabitacion = await tipoHabitacionRepository.ObtenerTipoHabitacionPorIdAsync(request.dto.IdTipoHabitacion);

            habitacion.Actualizar(
                hotel, 
                tipoHabitacion, 
                request.dto.CostoBase, 
                request.dto.Impuestos, 
                request.dto.CantidadHuespedes,
                request.dto.Ubicacion
            );

            habitacionRepository.Actualizar(habitacion);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Habitacion actualizada exitosamente con el ID: {IdHabitacion}", habitacion.IdHabitacion);

            return Unit.Value;
        }
    }
}
