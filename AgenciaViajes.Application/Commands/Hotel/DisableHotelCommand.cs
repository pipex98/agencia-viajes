using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record DisableHotelCommand(int id) : IRequest<bool>;

    public class DisableHotelCommandHandler(IHotelRepository hotelRepository, ILogger<DisableHotelCommand> _logger) : IRequestHandler<DisableHotelCommand, bool>
    {
        public async Task<bool> Handle(DisableHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await hotelRepository.DesabilitarHotelAsync(request.id);
        }
    }
}
