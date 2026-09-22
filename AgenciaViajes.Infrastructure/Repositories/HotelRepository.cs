using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HotelRepository(AppDbContext dbContext, ILogger<IHotelRepository> _logger) : IHotelRepository
    {
        public async Task<Hotel> ObtenerHotelPorId(int id)
        {
            var hotel = await dbContext.Hoteles.FirstOrDefaultAsync(x => x.IdHotel == id);

            _logger.LogInformation("Recuperando hotel con el ID: {IdHotel}", id);

            if (hotel == null)
            {
                _logger.LogWarning("La recuperacion del hotel fallo con el ID: {IdHotel} no se encontro", id);

                throw new KeyNotFoundException("Hotel no encontrado");
            }

            return hotel;
        }

        public void Crear(Hotel hotel)
        {
            dbContext.Hoteles.Add(hotel);
            _logger.LogInformation("Creando hotel con el ID: {IdHotel}", hotel.IdHotel);
        }

        public void Actualizar(Hotel hotel)
        {
            dbContext.Hoteles.Update(hotel);
            _logger.LogInformation("Actualizando hotel con el ID: {IdHotel}", hotel.IdHotel);
        }
    }
}
