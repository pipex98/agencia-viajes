using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class GeneroRepository(AppDbContext dbContext, ILogger<GeneroRepository> _logger)
    : IGeneroRepository
    {
        public async Task<Genero> ObtenerGeneroPorIdAsync(int id)
        {
            var genero = await dbContext.Generos.FirstOrDefaultAsync(x => x.IdGenero == id);

            _logger.LogInformation("Recuperando genero con ID: {IdGenero}", id);

            if (genero == null)
            {
                _logger.LogWarning("La recuperacion del genero fallo con ID: {IdGenero} no se encontro", id);

                throw new KeyNotFoundException("Genero no encontrado");
            }

            return genero;
        }
    }
}
