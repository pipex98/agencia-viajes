using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces;

namespace AgenciaViajes.Application.Commands.Hoteles
{
    public record DisableHotelCommand(int id) : IRequest<Unit>;

    public class DisableHotelCommandHandler(IHotelRepository hotelRepository, 
    IUnitOfWork unitOfWork, ILogger<DisableHotelCommand> _logger) : IRequestHandler<DisableHotelCommand, Unit>
    {
        public async Task<Unit> Handle(DisableHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var hotel = await hotelRepository.ObtenerHotelPorId(request.id);

            hotel.Desabilitar();

            hotelRepository.Actualizar(hotel);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Hotel desabilitado exitosamente con el ID: {IdHotel}", hotel.IdHotel);

            return Unit.Value;
        }
    }
}
