using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record EnableHotelCommand(int id) : IRequest<bool>;

    public class EnableHotelCommandHandler(IHotelRepository hotelRepository, ILogger<EnableHotelCommand> _logger) : IRequestHandler<EnableHotelCommand, bool>
    {
        public async Task<bool> Handle(EnableHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await hotelRepository.HabilitarHotelAsync(request.id);
        }
    }

}
