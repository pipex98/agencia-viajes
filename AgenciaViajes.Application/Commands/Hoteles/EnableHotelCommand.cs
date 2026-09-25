using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces;

namespace AgenciaViajes.Application.Commands.Hoteles
{
    public record EnableHotelCommand(int id) : IRequest<Unit>;

    public class EnableHotelCommandHandler(IHotelRepository hotelRepository,
    IUnitOfWork unitOfWork, ILogger<EnableHotelCommand> _logger) : IRequestHandler<EnableHotelCommand, Unit>
    {
        public async Task<Unit> Handle(EnableHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var hotel = await hotelRepository.ObtenerHotelPorId(request.id);

            hotel.Habilitar();

            hotelRepository.Actualizar(hotel);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Hotel deshabilitado exitosamente con el ID: {IdHotel}", hotel.IdHotel);

            return Unit.Value;
        }
    }

}
