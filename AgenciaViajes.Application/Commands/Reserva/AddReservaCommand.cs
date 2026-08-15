using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces.Services;

namespace AgenciaViajes.Application.Commands.Reserva
{
    public record AddReservaCommand(AddReservaDto dto) : IRequest<Unit>;

    public class AddReservaCommandHandler(IHabitacionRepository habitacionRepository,
    ILogger<AddReservaCommand> _logger, IHuespedRepository huespedRepository, IConfiguration configuration,
    IEmailService emailService) : IRequestHandler<AddReservaCommand, Unit>
    {
        public async Task<Unit> Handle(AddReservaCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            await habitacionRepository.ReservarHabitacion(request.dto);

            var huesped = await huespedRepository.ObtenerHuespedPorIdAsync(request.dto.IdHuesped);

            var habitacion = await habitacionRepository.ObtenerHabitacionConDetallesPorIdAsync(request.dto.IdHabitacion);

            _logger.LogInformation($"Iniciando el envio de correo electronico");

            List<DestinatarioEmailDto> destinatarios = new List<DestinatarioEmailDto>();

            _logger.LogInformation($"Preparando informacion para el envio de correo");

            destinatarios.Add(new DestinatarioEmailDto
            {
                Nombre = huesped.Nombres + huesped.Apellidos,
                Email = huesped.CorreoElectronico
            });

            destinatarios.Add(new DestinatarioEmailDto
            {
                Nombre = habitacion.Hotel.Agente.Nombre + habitacion.Hotel.Agente.Apellido,
                Email = habitacion.Hotel.Agente.CorreoElectronico
            });

            _logger.LogInformation($"Enviando el correo electronico");

            var bodyConfirmation = $"<p>La reservacion de la habitacion {habitacion.IdHabitacion} se ha hecho exitosamente</p>";

            await emailService.EnviarEmail(destinatarios, configuration["Mail:SubjectConfirmation"]!, bodyConfirmation);

            _logger.LogInformation($"Correo electronico enviado");

            return Unit.Value;
        }
    }
}

