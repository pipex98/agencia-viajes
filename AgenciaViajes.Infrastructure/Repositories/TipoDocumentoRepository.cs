using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class TipoDocumentoRepository(AppDbContext dbContext, ILogger<TipoDocumentoRepository> _logger) 
    : ITipoDocumentoRepository
    {
        public async Task<TipoDocumento> ObtenerTipoDocumentoPorIdAsync(int id)
        {
            var tipoDocumento = await dbContext.TipoDocumentos
            .FirstOrDefaultAsync(x => x.IdTipoDocumento == id);

            _logger.LogInformation("Recuperando tipo de documento con ID: {IdTipoDocumnto}", id);

            if (tipoDocumento == null)
            {
                _logger.LogWarning("La recuperacion del tipo de documento fallo con ID: {IdTipoDocumento} no se encontro", id);

                throw new KeyNotFoundException("Tipo de documento no encontrado");
            }

            return tipoDocumento;
        }
    }
}
