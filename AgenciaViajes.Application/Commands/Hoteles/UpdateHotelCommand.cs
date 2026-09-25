
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces;
using AgenciaViajes.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Application.Commands.Hoteles
{
    public record UpdateHotelCommand(int id, UpsertHotelDto dto) : IRequest<Unit>;

    public class UpdateHotelCommandHandler(IHotelRepository hotelRepository,
    IAgenteRepository agenteRepository, ICiudadRepository ciudadRepository,
    IUnitOfWork unitOfWork, ILogger<UpdateHotelCommand> _logger) : IRequestHandler<UpdateHotelCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var hotel = await hotelRepository.ObtenerHotelPorId(request.id);

            var agente = await agenteRepository.ObtenerAgentePorId(request.dto.IdAgente);

            var ciudad = await ciudadRepository.ObtenerCiudadPorId(request.dto.IdCiudad);

            hotel.Actualizar(
                agente,
                ciudad,
                request.dto.Nombre,
                request.dto.Direccion,
                request.dto.Descripcion
            );

            hotelRepository.Actualizar(hotel);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Hotel actualizado exitosamente con el ID: {IdHotel}", hotel.IdHotel);

            return Unit.Value;
        }
    }
}
