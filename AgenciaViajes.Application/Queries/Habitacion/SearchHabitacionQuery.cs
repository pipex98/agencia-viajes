using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Queries.Habitacion
{
    public record SearchHabitacionQuery(ParametrosBusquedaHabitacionDto dto) : IRequest<List<HabitacionDto>>;

    public class SearchHabitacionQueryHandler(IHabitacionRepository habitacionRepository, ILogger<SearchHabitacionQuery> _logger)
    : IRequestHandler<SearchHabitacionQuery, List<HabitacionDto>>
    {
        public async Task<List<HabitacionDto>> Handle(SearchHabitacionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await habitacionRepository.BuscarHabitacion(request.dto);
        }
    }
}
