using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class ReservaRepository(AppDbContext dbContext, ILogger<ReservaRepository> logger) 
    : IReservaRepository
    {
        public void Crear(Reserva reserva)
        {
            dbContext.Reservas.Add(reserva);
            logger.LogInformation("Creando reserva con el ID: {ID}", reserva.IdReserva);
        }

        public async Task<List<Reserva>> ObtenerReservasPorHuespedAsync(int id)
        {
            logger.LogInformation("Recuperando reservaciones con el ID: {IdHuesped}", id);

            var reservaciones = await dbContext.Reservas.AsNoTracking()
                .Include(r => r.Habitacion)
                .ThenInclude(r => r.Hotel)
                .Include(r => r.Huesped)
                .Include(r => r.DetalleReservas)
                .Where(r => r.IdHuesped == id)
                .ToListAsync();

            if (reservaciones == null)
            {
                logger.LogWarning("La recuperacion de las reservaciones fallo con el ID: {IdHuesped}", id);

                throw new KeyNotFoundException("No se encontraron reservaciones");
            }

            return reservaciones;
        }
    }
}
