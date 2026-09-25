
using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Application.Interfaces;

namespace AgenciaViajes.Application.Commands.Hoteles
{
    public record AddHotelCommand(UpsertHotelDto dto) : IRequest<Unit>;

    public class AddHotelCommandHandler(IHotelRepository hotelRepository, IAgenteRepository agenteRepository,
    ICiudadRepository ciudadRepository, IUnitOfWork unitOfWork, ILogger<AddHotelCommand> _logger): 
    IRequestHandler<AddHotelCommand, Unit>
    {
        public async Task<Unit> Handle(AddHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            var agente = await agenteRepository.ObtenerAgentePorId(request.dto.IdAgente);

            var ciudad = await ciudadRepository.ObtenerCiudadPorId(request.dto.IdCiudad);

            var hotel = Hotel.Crear(
                agente, 
                ciudad, 
                request.dto.Nombre,
                request.dto.Direccion,
                request.dto.Descripcion
            );

            hotelRepository.Crear(hotel);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Hotel creado exitosamente con el ID: {IdHotel}", hotel.IdHotel);

            return Unit.Value;
        }
    }

}
