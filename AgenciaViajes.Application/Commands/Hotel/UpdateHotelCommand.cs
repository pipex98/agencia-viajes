
using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record UpdateHotelCommand(int id, UpsertHotelDto dto) : IRequest<UpsertHotelDto>;

    public class UpdateHotelCommandHandler(IHotelRepository hotelRepository, ILogger<UpdateHotelCommand> _logger) : IRequestHandler<UpdateHotelCommand, UpsertHotelDto>
    {
        public async Task<UpsertHotelDto> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await hotelRepository.UpdateHotelAsync(request.id, request.dto);
        }
    }
}
