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
            _logger.LogInformation("Agregando el objeto hotel al DbSet");

            dbContext.Hoteles.Add(hotel);
        }

        public void Actualizar(Hotel hotel)
        {
            _logger.LogInformation("Marcando la entidad Hotel como modificada");

            dbContext.Hoteles.Update(hotel);
        }
    }
}
