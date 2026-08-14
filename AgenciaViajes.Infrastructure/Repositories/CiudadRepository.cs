using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class CiudadRepository(AppDbContext dbContext, ILogger<CiudadRepository> _logger) 
    : ICiudadRepository
    {
        public async Task<Ciudad> ObtenerCiudadPorId(int id)
        {
            var ciudad = await dbContext.Ciudades.FirstOrDefaultAsync(x => x.Id == id);

            _logger.LogInformation("Recuperando ciudad con ID: {IdCiudad}", id);

            if (ciudad == null)
            {
                _logger.LogWarning("La recuperacion de la ciudad fallo con ID: {IdCiudad} no se encontro", id);

                throw new KeyNotFoundException("Ciudad no encontrada");
            }

            return ciudad;
        }
    }
}
