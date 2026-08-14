using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class TipoHabitacionRepository(AppDbContext dbContext, ILogger<TipoHabitacionRepository> _logger) 
    : ITipoHabitacionRepository
    {
        public async Task<TipoHabitacion> ObtenerTipoHabitacionPorIdAsync(int id)
        {
            var tipoHabitacion = await dbContext.TipoHabitaciones
            .FirstOrDefaultAsync(x => x.IdTipoHabitacion == id);

            _logger.LogInformation("Recuperando tipo de habitacion con ID: {IdTipoHabitacion}", id);

            if (tipoHabitacion == null)
            {
                _logger.LogWarning("La recuperacion del tipo de habitacion fallo con ID: {IdTipoHabitacion} no se encontro", id);

                throw new KeyNotFoundException("Tipo de habitacion no encontrada");
            }

            return tipoHabitacion;
        }
    }
}
