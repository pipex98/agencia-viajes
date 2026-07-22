using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Queries.Habitacion
{
    public record ObtenerReservacionesQuery(int id) : IRequest<List<ReservaDto>>;

    public class ObtenerReservacionesQueryHandler(IHabitacionRepository habitacionRepository, ILogger<ObtenerReservacionesQuery> _logger)
    : IRequestHandler<ObtenerReservacionesQuery, List<ReservaDto>>
    {
        public async Task<List<ReservaDto>> Handle(ObtenerReservacionesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await habitacionRepository.ObtenerReservaciones(request.id);
        }
    }
}
