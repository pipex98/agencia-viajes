using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Dto.DetalleReserva;

namespace AgenciaViajes.Application.Queries.Reservas
{
    public record ObtenerReservacionesQuery(int id) : IRequest<List<ReservaDto>>;

    public class ObtenerReservacionesQueryHandler(IReservaRepository reservaRepository, 
    ILogger<ObtenerReservacionesQuery> _logger): IRequestHandler<ObtenerReservacionesQuery, List<ReservaDto>>
    {
        public async Task<List<ReservaDto>> Handle(ObtenerReservacionesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var query = await reservaRepository.ObtenerReservasPorHuespedAsync(request.id);

            var reservaciones = query.Select(r => new ReservaDto
            {
                FechaIngreso = DateOnly.FromDateTime(r.FechaIngreso),
                FechaSalida = DateOnly.FromDateTime(r.FechaSalida),
                CantidadHuespedes = r.Habitacion.CantidadHuespedes,
                Hotel = r.Habitacion.Hotel.Nombre,
                NombreCliente = r.Huesped.Nombres + " " + r.Huesped.Apellidos,
                NumeroReserva = r.IdReserva.ToString(),
                FechaReserva = DateOnly.FromDateTime(r.FechaReserva),
                Estado = r.Estado,
                Subtotal = r.Subtotal,
                Iva = r.Iva,
                Total = r.Total,
                DetalleReserva = r.DetalleReservas.Select(dr => new ReservaDetalleReservaDto
                {
                    Cantidad = dr.Cantidad,
                    Concepto = dr.Concepto,
                    PrecioUnitario = dr.PrecioUnitario,
                    Importe = dr.Importe,
                })
                .ToList()
            })
            .ToList();

            _logger.LogInformation("Reservaciones recuperadas");

            return reservaciones;
        }
    }
}
