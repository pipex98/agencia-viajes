using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface ITipoDocumentoRepository
    {
        Task<TipoDocumento> ObtenerTipoDocumentoPorIdAsync(int id);
    }
}
