using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface ICiudadRepository
    {
        Task<Ciudad> ObtenerCiudadPorId(int id);
    }
}
