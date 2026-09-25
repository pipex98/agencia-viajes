using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Queries.Habitaciones
{
    public record SearchHabitacionQuery(ParametrosBusquedaHabitacionDto dto) : IRequest<List<HabitacionDto>>;

    public class SearchHabitacionQueryHandler(IHabitacionRepository habitacionRepository, 
    ILogger<SearchHabitacionQuery> _logger): IRequestHandler<SearchHabitacionQuery, List<HabitacionDto>>
    {
        public async Task<List<HabitacionDto>> Handle(SearchHabitacionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var query = await habitacionRepository.ObtenerHabitacionesDisponibles(request.dto);

            int diasEstancia = (request.dto.FechaSalida - request.dto.FechaIngreso).Days;

            _logger.LogInformation("Calculando los dias de estancia para el usuario");

            var habitaciones = query.Select(ha => new HabitacionDto
            {
                NumeroHabitacion = ha.IdHabitacion,
                Ciudad = ha.Hotel.Ciudad.Nombre,
                Hotel = ha.Hotel.Nombre,
                TipoHabitacion = ha.TipoHabitacion.Nombre,
                Subtotal = Math.Round(diasEstancia * ha.CostoBase, 2),
                Impuestos = Math.Round((diasEstancia * ha.CostoBase) * ha.Impuestos / 100m, 2),
                CapacidadMaxima = ha.CantidadHuespedes
            })
            .ToList();

            _logger.LogInformation("Habitaciones disponibles recuperadas");

            return habitaciones;
        }
    }
}
