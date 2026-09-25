using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces.Services;
using AgenciaViajes.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Application.Commands.Reservas
{
    public record AddReservaCommand(AddReservaDto dto) : IRequest<Unit>;

    public class AddReservaCommandHandler(IReservaRepository reservaRepository,
    IHabitacionRepository habitacionRepository, ILogger<AddReservaCommand> _logger,
    IHuespedRepository huespedRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddReservaCommand, Unit>
    {
        public async Task<Unit> Handle(AddReservaCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var habitacion = await habitacionRepository.ObtenerHabitacionConDetallesPorIdAsync(request.dto.IdHabitacion);

            habitacion.Desabilitar();

            habitacionRepository.Actualizar(habitacion);

            await unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Habitacion deshabilitada exitosamente con el ID: {IdHabitacion}", habitacion.IdHabitacion);

            var huesped = await huespedRepository.ObtenerHuespedPorIdAsync(request.dto.IdHuesped);

            var reserva = Reserva.Crear(
                habitacion,
                huesped,
                request.dto.FechaIngreso,
                request.dto.FechaSalida,
                request.dto.ContactoEmergencia
            );

            foreach (var item in request.dto.DetalleReservas)
            {
                var detalleReserva = DetalleReserva.Crear(
                    item.Concepto, 
                    item.Cantidad, 
                    item.PrecioUnitario
                );

                reserva.AgregarDetalle(detalleReserva);

                _logger.LogInformation("Agregando el detalle a la reserva");
            }

            reserva.CalcularSubtotal();

            _logger.LogInformation("Subtotal calculado de la reserva: {Subtotal}", reserva.Subtotal);

            reserva.CalcularTotal();

            _logger.LogInformation("Total calculado de la reserva: {Total}", reserva.Total);

            var comisionReserva = ComisionReserva.Crear(reserva.Total);

            reserva.AgregarComision(comisionReserva);

            _logger.LogInformation("Comision calculada de la reserva: {MontoComision}", comisionReserva.MontoComision);

            reservaRepository.Crear(reserva);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Reserva creada exitosamente con el ID: {IdReserva}", reserva.IdReserva);

            return Unit.Value;
        }
    }
}

