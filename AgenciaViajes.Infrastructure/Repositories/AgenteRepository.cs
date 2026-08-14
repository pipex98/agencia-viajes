using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class AgenteRepository(AppDbContext dbContext, ILogger<AgenteRepository> _logger) 
    : IAgenteRepository
    {
        public async Task<Agente> ObtenerAgentePorId(int id)
        {
            var agente = await dbContext.Agentes.FirstOrDefaultAsync(x => x.IdAgente == id);

            _logger.LogInformation("Recuperando agente con ID: {IdAgente}", id);

            if (agente == null)
            {
                _logger.LogWarning("La recuperacion del agente fallo con ID: {IdAgente} no se encontro", id);

                throw new KeyNotFoundException("Agente no encontrado");
            }

            return agente;
        }
    }
}
