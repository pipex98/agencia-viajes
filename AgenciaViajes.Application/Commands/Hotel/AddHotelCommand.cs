
using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Commands.Hotel
{
    public record AddHotelCommand(UpsertHotelDto dto) : IRequest<UpsertHotelDto>;

    public class AddHotelCommandHandler(IHotelRepository hotelRepository, ILogger<AddHotelCommand> _logger): IRequestHandler<AddHotelCommand, UpsertHotelDto>
    {
        public async Task<UpsertHotelDto> Handle(AddHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await hotelRepository.AddHotelAsync(request.dto);
        }
    }

}
